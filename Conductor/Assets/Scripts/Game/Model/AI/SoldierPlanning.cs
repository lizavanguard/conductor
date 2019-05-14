using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Conductor.Game.Model
{
    // モックとして「プレイヤーの方向を向く」と「プレイヤーを攻撃する」を実装する
    // 実際にはこれらのどっちを選ぶかは隊長クラスのplanningから判断される
    public class SoldierPlanning
    {
        // 候補のNode
        PlanningNode[] baseNodeList;

        // プランニングの最終目標
        Condition goalCondition;

        // 現在構築済のプランニング
        List<PlanningNode> currentPlanningChain;

        Condition currentCondition;

        OperationBase currentOperation;

        ActorModelBase owner;
        CommandRunner commandRunner;
        GameMaster gameMaster;
        IConditionUpdater conditionUpdater;

        // rebuildフラグ
        bool dirty;

        public Condition CurrentCondition { get { return currentCondition; } }
        public OperationBase CurrentOperation { get { return currentOperation; } }

        public SoldierPlanning(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster, IPlanningNodeFactory nodeFactory, IConditionUpdater conditionUpdater)
        {
            // FIXME: データから読み込むべき soldierかcaptainかによっても異なるはず
            var operations = new OperationType[]
            {
                OperationType.SearchEnemy,
                OperationType.AttackNearestEnemy,
            };
            baseNodeList = nodeFactory.Create(operations);

            currentCondition = new Condition(new int[] { });
            currentPlanningChain = new List<PlanningNode>();

            this.owner = owner;
            this.commandRunner = commandRunner;
            this.gameMaster = gameMaster;
            this.conditionUpdater = conditionUpdater;
        }

        public void SetGoal(Condition goal)
        {
            if (goalCondition != goal)
            {
                dirty = true;
            }

            goalCondition = goal;
        }

        public void UpdateCurrentCondition()
        {
            conditionUpdater.Update(currentCondition);
        }

        public void UpdatePlanning()
        {
            // rebuildが必要ならrebuild TODO: ここでやるべきかどうかは要検討？
            if (dirty)
            {
                BuildPllaningChain();
            }

            if (currentPlanningChain == null || currentPlanningChain.Count == 0)
            {
                currentOperation = null;
                return;
            }

            // もしかすると1フレームで複数枚抜きする必要あるかも
            var currentNode = currentPlanningChain.Last();
            if (currentCondition.Satisfy(currentNode.AfterCondition))
            {
                currentPlanningChain.RemoveAt(currentPlanningChain.Count - 1);

                if (currentPlanningChain.Count > 0)
                {
                    var nextNode = currentPlanningChain.Last();
                    var factory = new OperationFactory(owner, commandRunner, gameMaster);
                    currentOperation = factory.Create(nextNode.OperationType);
                }
                else
                {
                    // 最後の行動を完了した直後
                    currentOperation = null;
                    BuildPllaningChain();
                }
            }
        }

        public void BuildPllaningChain()
        {
            // 最終ゴールを現在ゴールとして初期化
            currentOperation = null;
            currentPlanningChain.Clear();

            var newChain = ChainNode(currentPlanningChain, goalCondition);
            if (newChain != null)
            {
                currentPlanningChain = newChain;
            }

            foreach (var node in currentPlanningChain)
            {
                Debug.Log(node.OperationType.ToString());
            }
            if (currentPlanningChain != null && currentPlanningChain.Count > 0)
            {
                // 最初のoperation作成
                var node = currentPlanningChain.Last();
                var factory = new OperationFactory(owner, commandRunner, gameMaster);
                currentOperation = factory.Create(node.OperationType);
            }

            dirty = false;
        }

        public bool GoalIsSatisfied()
        {
            return currentCondition.Satisfy(goalCondition);
        }

        public bool HasPlanningChain()
        {
            return currentPlanningChain != null && currentPlanningChain.Count > 0;
        }

        List<PlanningNode> ChainNode(List<PlanningNode> chain, Condition goal)
        {
            // currentStateがゴールを満たしているかどうかチェック(終了判定)
            if (currentCondition.Satisfy(goal))
            {
                return chain;
            }

            // 現在ゴールを満たすafterConditionを持ったNodeを一覧化
            var newNodeList = new List<PlanningNode>();
            foreach (var node in baseNodeList)
            {
                if (node.AfterCondition.Satisfy(goal))
                {
                    newNodeList.Add(node);
                }
            }

            // なかったら探索打ち止め
            if (newNodeList.Count == 0)
            {
                return null;
            }

            foreach (var newNode in newNodeList)
            {
                // 千日手チェック FIXME: 千日手の条件はもうちょっと考えたほうがよさげ
                if (chain.Contains(newNode))
                {
                    continue;
                }

                // よしなに(最初は順番に)選ぶ 現在の状態に最も近付くようなやつがいい
                var tempChain = new List<PlanningNode>(chain);
                tempChain.Add(newNode);

                // 現在ゴールを更新
                var tempGoal = newNode.BeforeCondition;

                var newChain = ChainNode(tempChain, tempGoal);

                // 最初に見つかったものを無条件に選んでいるが、複数から選択したかったらちょっと考える
                // でもそれは要するに総当りなので上のnewNodeの選択方法を改善したほうがいいと思う
                if (newChain != null)
                {
                    return newChain;
                }
            }

            // どれも行き詰まりか千日手
            return null;
        }
    }
}
