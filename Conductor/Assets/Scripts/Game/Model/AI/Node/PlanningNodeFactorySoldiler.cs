using System;
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
        
        public PlanningNodeFactorySoldiler(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster)
        {
            this.owner = owner;
            this.commandRunner = commandRunner;
            this.gameMaster = gameMaster;
        }

        // FIXME: ConditionパターンのメタデータとOperationのリストから自動生成するように変更
        // SoldierとCaptainの区別もいらなくなるはず
        public PlanningNode[] Create(OperationType[] operations)
        {
            List<PlanningNode> newNodeList = new List<PlanningNode>();

            // 最も近くにいる敵対陣営のキャラのほうを向く
            {
                var beforeList = new ConditionType[]
                {
                };
                var afterList = new ConditionType[]
                {
                    ConditionType.LookToSomeEnemy,
                };
                var node = new PlanningNode(owner, commandRunner, gameMaster, new Condition(beforeList), new Condition(afterList), OperationType.LookToNearestEnemy);
                newNodeList.Add(node);
            }

            // 最も近くにいる敵対陣営のキャラを攻撃しようとする
            {
                var beforeList = new ConditionType[]
                {
                    ConditionType.CanTargetSomeEnemy,
                };
                var afterList = new ConditionType[]
                {
                    ConditionType.HittingSomeEnemy,
                };
                var disabledList = new ConditionType[]
                {
                };
                var node = new PlanningNode(owner, commandRunner, gameMaster, new Condition(beforeList), new Condition(afterList), OperationType.AttackNearestEnemy);
                newNodeList.Add(node);
            }

            // 攻撃相手が見つかるまで索敵
            {
                var beforeList = new ConditionType[]
                {
                };
                var afterList = new ConditionType[]
                {
                    ConditionType.CanTargetSomeEnemy,
                };
                var disabledList = new ConditionType[]
                {
                };
                var node = new PlanningNode(owner, commandRunner, gameMaster, new Condition(beforeList), new Condition(afterList), OperationType.SearchEnemy);
                newNodeList.Add(node);
            }

            // TargetPositionの近くに行く
            {
                var beforeList = new ConditionType[]
                {
                };
                var afterList = new ConditionType[]
                {
                    ConditionType.StayNearTargetPoint,
                };
                var disabledList = new ConditionType[]
                {
                };
                var node = new PlanningNode(owner, commandRunner, gameMaster, new Condition(beforeList), new Condition(afterList), OperationType.MoveToTargetPoint);
                newNodeList.Add(node);
            }

            return newNodeList.ToArray();
        }
    }
}
