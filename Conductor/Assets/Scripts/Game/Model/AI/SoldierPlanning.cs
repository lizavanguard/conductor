using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    // モックとして「プレイヤーの方向を向く」と「プレイヤーを攻撃する」を実装する
    // 実際にはこれらのどっちを選ぶかは隊長クラスのplanningから判断される
    public class SoldierPlanning
    {
        // FIXME: なんとかこれをデータ化したい
        // planningChainを作るときはafterとbeforeで変化のあったbitに着目して次の状況を作る
        class Node
        {
            // 前提条件
            Condition beforeCondition;

            // 目標条件
            Condition afterCondition;

            // 行動
            OperationType operationType;

            public Condition BeforeCondition { get { return beforeCondition; } }
            public Condition AfterCondition { get { return afterCondition; } }


            public Node(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster, Condition before, Condition after, OperationType operationType)
            {
                this.beforeCondition = before;
                this.afterCondition = after;
                this.operationType = operationType;
            }
        }

        // 候補のNode
        Node[] baseNodeList;

        // プランニングの最終目標
        Condition goalCondition;

        // 現在構築済のプランニング
        List<Node> currentPlanningChain;

        Condition currentCondition;

        ActorModelBase owner;
        CommandRunner commandRunner;
        GameMaster gameMaster;

        public SoldierPlanning(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster)
        {
            // Nodeのモックを用意して動かしてみる
            // 1. 方向転換と攻撃のOperationを書く
            // 2. OperationTypeからOperationを作るファクトリーを書く
            // 3. Nodeのコンストラクタを書
            // 4. 各Operationに対応したNodeをPlanning内に作る
            // 5. AI側にもConditionを持たせて定期更新を行う
            // 6. PlanningChain構築メソッドを書く
            // 7. 構築、Operation決定、Commandを生成までの流れを書く kokokara
            // 8: OperationTypeを増築

            baseNodeList = GenerateOperationNodes(owner, commandRunner, gameMaster);

            currentCondition = new Condition(new ConditionType[] { });

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

        public void BuildPllaningChain()
        {
            // 最終ゴールを現在ゴールとして初期化
            currentPlanningChain.Clear();
            ChainNode(currentPlanningChain, goalCondition);
        }

        List<Node> ChainNode(List<Node> chain, Condition goal)
        {
            // currentStateがゴールを満たしているかどうかチェック(終了判定)
            if (currentCondition.Satisfy(goal))
            {
                return chain;
            }

            // 現在ゴールを満たすafterConditionを持ったNodeを一覧化
            var newNodeList = new List<Node>();
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
                var tempChain = new List<Node>(chain);
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
        Node[] GenerateOperationNodes(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster)
        {
            List<Node> newNodeList = new List<Node>();

            // 最も近くにいる敵対陣営のキャラのほうを向く
            {
                var beforeList = new ConditionType[]
                {
                    ConditionType.LookToSomeEnemy,
                    ConditionType.CanHitSomeEnemy,
                    ConditionType.HittingSomeEnemy,
                };
                var afterList = new ConditionType[]
                {
                    ConditionType.LookToSomeEnemy,
                };
                var node = new Node(owner, commandRunner, gameMaster, new Condition(beforeList), new Condition(afterList), OperationType.LookToNearestEnemy);
                newNodeList.Add(node);
            }

            // 最も近くにいる敵対陣営のキャラを攻撃しようとする
            {
                var beforeList = new ConditionType[]
                {
                    ConditionType.CanHitSomeEnemy,
                };
                var afterList = new ConditionType[]
                {
                    ConditionType.HittingSomeEnemy,
                };
                var node = new Node(owner, commandRunner, gameMaster, new Condition(beforeList), new Condition(afterList), OperationType.AttackNearestEnemy);
                newNodeList.Add(node);
            }

            return newNodeList.ToArray();
        }
    }
}
