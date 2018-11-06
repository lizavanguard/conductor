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
            public OperationType OperationType { get { return operationType; } }


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

        OperationBase currentOperation;

        ActorModelBase owner;
        CommandRunner commandRunner;
        GameMaster gameMaster;

        public OperationBase CurrentOperation { get { return currentOperation; } }

        public SoldierPlanning(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster)
        {
            // Nodeのモックを用意して動かしてみる
            // 1. 方向転換と攻撃のOperationを書く
            // 2. OperationTypeからOperationを作るファクトリーを書く
            // 3. Nodeのコンストラクタを書
            // 4. 各Operationに対応したNodeをPlanning内に作る
            // 5. AI側にもConditionを持たせて定期更新を行う
            // 6. PlanningChain構築メソッドを書く
            // 7. 構築、Operation決定、Commandを生成までの流れを書く たぶんおｋ
            // 8: OperationTypeを増築
            // CanHitSomeEnemyの状態更新を修正(捕捉して接近できる、に変える) ok
            // 索敵する行動を追加
            // いれば殴る、いなければ索敵する、のAIを作ってみる

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

            if (currentPlanningChain != null && currentPlanningChain.Count > 0)
            {
                // 最初のoperation作成
                var node = currentPlanningChain.Last();
                var factory = new OperationFactory(owner, commandRunner, gameMaster);
                currentOperation = factory.Create(node.OperationType);
            }
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
                    ConditionType.CanTargetSomeEnemy,
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
                    ConditionType.CanTargetSomeEnemy,
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
