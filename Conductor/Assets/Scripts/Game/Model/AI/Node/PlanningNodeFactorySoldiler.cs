using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class PlanningNodeFactorySoldiler : IPlanningNodeFactory
    {
        static readonly ConditionType[] AllConditionTypes = Enum.GetValues(typeof(ConditionType)) as ConditionType[];

        ActorModelBase owner;
        CommandRunner commandRunner;
        GameMaster gameMaster;
        Dictionary<OperationType, ConditionChangeData> conditionChangeDataMap;

        // 再帰を簡単に書くための作業用
        List<PlanningNode> tempNodeList;

        public PlanningNodeFactorySoldiler(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster)
        {
            this.owner = owner;
            this.commandRunner = commandRunner;
            this.gameMaster = gameMaster;

            var loader = new ConditionChangeDataLoader();
            loader.Load();
            conditionChangeDataMap = loader.GetChangeDataMap();

            tempNodeList = new List<PlanningNode>();
        }

        // FIXME: ConditionパターンのメタデータとOperationのリストから自動生成するように変更
        // SoldierとCaptainの区別もいらなくなるはず
        public PlanningNode[] Create(OperationType[] operations)
        {
            List<PlanningNode> newNodeList = new List<PlanningNode>();

            foreach (var operation in operations)
            {
                if (!conditionChangeDataMap.ContainsKey(operation))
                {
                    Debug.LogError(string.Format("Operation could not be found in condition change data map. Operation name is [{0}]", operation.ToString()));
                    continue;
                }

                var changeData = conditionChangeDataMap[operation];

                var beforeList = changeData.Preconditions;
                var afterList = changeData.Postconditions;
                var node = new PlanningNode(owner, commandRunner, gameMaster, new Condition(beforeList), new Condition(afterList), operation);
                newNodeList.Add(node);
            }

            return newNodeList.ToArray();
        }

        PlanningNode[] CreateNodesOfOperation(OperationType operationType)
        {
            tempNodeList.Clear();
            AppendNewNodeRecursively(0, new ConditionType[0], new ConditionType[0], operationType);

            return tempNodeList.ToArray();
        }

        void AppendNewNodeRecursively(int nextConditionIndex, ConditionType[] prevBeforeArray, ConditionType[] prevAfterArray, OperationType operationType)
        {
            // 終了条件
            if (nextConditionIndex == AllConditionTypes.Length)
            {
                var node = new PlanningNode(owner, commandRunner, gameMaster, new Condition(prevBeforeArray), new Condition(prevAfterArray), operationType);
                tempNodeList.Add(node);
                return;
            }

            var nextCondition = AllConditionTypes[nextConditionIndex];
            var operationMeta = conditionChangeDataMap[operationType];

            if (operationMeta.Preconditions.Contains(nextCondition))
            {
                if (operationMeta.Postconditions.Contains(nextCondition))
                {
                    // before, after両方に含まれている→前提であり、状態がキープされる→true->trueのみ存在
                    var newBeforeList = new List<ConditionType>(prevBeforeArray);
                    newBeforeList.Add(nextCondition);
                    var newAfterList = new List<ConditionType>(prevAfterArray);
                    newAfterList.Add(nextCondition);
                    AppendNewNodeRecursively(nextConditionIndex + 1, newBeforeList.ToArray(), newAfterList.ToArray(), operationType);
                }
                else
                {
                    // beforeのみに含まれている→前提ではあるがその後の状態は保証しない→true->falseのみ存在（更新時に新しく出現する条件 これがあると千日手が発生しうる？）
                    var newBeforeList = new List<ConditionType>(prevBeforeArray);
                    newBeforeList.Add(nextCondition);
                    AppendNewNodeRecursively(nextConditionIndex + 1, newBeforeList.ToArray(), prevAfterArray, operationType);
                }
            }
            else
            {
                if (operationMeta.Postconditions.Contains(nextCondition))
                {
                    // afterのみに含まれている→前提ですらないが達成はされる→false->trueのみ存在
                    var newAfterList = new List<ConditionType>(prevAfterArray);
                    newAfterList.Add(nextCondition);
                    AppendNewNodeRecursively(nextConditionIndex + 1, prevBeforeArray, newAfterList.ToArray(), operationType);
                }
                else
                {
                    // どっちにもふくまれていない→無関係→true->trueとfalse->falseが存在(これが前提に入ってしまうこともあるが、つまり「この条件を満たした状態でこのoperationを完遂する」というNodeになる）
                    var newBeforeList = new List<ConditionType>(prevBeforeArray);
                    newBeforeList.Add(nextCondition);
                    var newAfterList = new List<ConditionType>(prevAfterArray);
                    newAfterList.Add(nextCondition);
                    AppendNewNodeRecursively(nextConditionIndex + 1, newBeforeList.ToArray(), newAfterList.ToArray(), operationType);
                    AppendNewNodeRecursively(nextConditionIndex + 1, prevBeforeArray, prevAfterArray, operationType);
                }
            }
        }
    }
}
