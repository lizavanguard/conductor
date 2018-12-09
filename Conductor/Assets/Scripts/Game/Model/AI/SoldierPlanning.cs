﻿using System.Collections;
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
        PlannngNode[] baseNodeList;

        // プランニングの最終目標
        Condition goalCondition;

        // 現在構築済のプランニング
        List<PlannngNode> currentPlanningChain;

        Condition currentCondition;

        OperationBase currentOperation;

        ActorModelBase owner;
        CommandRunner commandRunner;
        GameMaster gameMaster;

        public OperationBase CurrentOperation { get { return currentOperation; } }

        public SoldierPlanning(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster)
        {
            // FIXME: Factoryから貰う形にしたほうがよさそう
            baseNodeList = GenerateOperationNodes(owner, commandRunner, gameMaster);

            currentCondition = new Condition(new ConditionType[] { });
            currentPlanningChain = new List<PlannngNode>();

            this.owner = owner;
            this.commandRunner = commandRunner;
            this.gameMaster = gameMaster;
        }

        public void SetGoal(Condition goal)
        {
            goalCondition = goal;
        }

        public void UpdateCurrentCondition()
        {
            currentCondition.UpdateCondition(owner, gameMaster);
        }

        public void UpdatePlanning()
        {
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
            currentPlanningChain = ChainNode(currentPlanningChain, goalCondition);

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
        }

        public bool GoalIsSatisfied()
        {
            return currentCondition.Satisfy(goalCondition);
        }

        public bool HasPlanningChain()
        {
            return currentPlanningChain != null && currentPlanningChain.Count > 0;
        }

        List<PlannngNode> ChainNode(List<PlannngNode> chain, Condition goal)
        {
            // currentStateがゴールを満たしているかどうかチェック(終了判定)
            if (currentCondition.Satisfy(goal))
            {
                return chain;
            }

            // 現在ゴールを満たすafterConditionを持ったNodeを一覧化
            var newNodeList = new List<PlannngNode>();
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
                var tempChain = new List<PlannngNode>(chain);
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

        // FIXME: 本当は外部ファイルから読み込むべき
        // 最終的には敵タイプとか味方ユニットごとに外部データから読み込み
        // 味方の方はユーザーがいじれるようにもしたい
        PlannngNode[] GenerateOperationNodes(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster)
        {
            List<PlannngNode> newNodeList = new List<PlannngNode>();

            // 最も近くにいる敵対陣営のキャラのほうを向く
            {
                var beforeList = new ConditionType[]
                {
                };
                var afterList = new ConditionType[]
                {
                    ConditionType.LookToSomeEnemy,
                };
                var node = new PlannngNode(owner, commandRunner, gameMaster, new Condition(beforeList), new Condition(afterList), OperationType.LookToNearestEnemy);
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
                var node = new PlannngNode(owner, commandRunner, gameMaster, new Condition(beforeList), new Condition(afterList), OperationType.AttackNearestEnemy);
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
                var node = new PlannngNode(owner, commandRunner, gameMaster, new Condition(beforeList), new Condition(afterList), OperationType.SearchEnemy);
                newNodeList.Add(node);
            }

            return newNodeList.ToArray();
        }
    }
}
