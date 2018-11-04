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

            OperationBase operation;

            public Node(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster, Condition before, Condition after, OperationType operationType)
            {
                this.beforeCondition = before;
                this.afterCondition = after;
                this.operationType = operationType;

                var factory = new OperationFactory(owner, commandRunner, gameMaster);
                operation = factory.Create(operationType);
            }
        }

        // 条件ズ FIXME: 「行き先の命令を受けている」といった状態も存在する その場合の実際の座標はActorが持つ
        public enum ConditionType
        {
            // 誰でもいいから敵の方を向いている
            LookToSomeEnemy,

            // 誰でもいいから敵に攻撃できる距離にある
            CanHitSomeEnemy,

            // 敵に攻撃している
            HittingSomeEnemy,
        }

        public class Condition
        {
            // ConditionTypeの0～31に対応するフラグ
            // 該当bitがtrue => その条件は成立している
            // 該当bitがfalse => その条件の成否は不明/不問
            // 明示的に不成立の条件を入れる時は、専用のbitを作る
            uint conditionFlag;
            public uint ConditionFlag { get { return conditionFlag; } }

            public Condition(ConditionType[] trueList)
            {
                conditionFlag = 0;
                foreach (var type in trueList)
                {
                    conditionFlag |= 1U << (int)type;
                }
            }

            public bool Satisfy(Condition other)
            {
                return other.conditionFlag == (conditionFlag & other.conditionFlag);
            }

            public bool IsSatisfiedBy(Condition other)
            {
                return other.Satisfy(this);
            }

            public void SetFlag(ConditionType type, bool flag)
            {
                var mask = 1U << (int)type;

                if (flag)
                {
                    conditionFlag |= mask;
                }
                else
                {
                    conditionFlag &= ~mask;
                }
            }
        }

        // 候補のNode
        Node[] nodeList;

        // プランニングの最終目標
        Condition goalCondition;

        // 現在構築中のプランニング
        List<Node> currentPlanningChain;

        Condition currentCondition;

        ActorModelBase owner;

        public SoldierPlanning(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster)
        {
            // Nodeのモックを用意して動かしてみる
            // 1. 方向転換と攻撃のOperationを書く
            // 2. OperationTypeからOperationを作るファクトリーを書く
            // 3. Nodeのコンストラクタを書
            // 4. 各Operationに対応したNodeをPlanning内に作る
            // 5. AI側にもConditionを持たせて定期更新を行う kokokara
            // 6. PlanningChain構築メソッドを書く
            // 7. 構築、Operation決定、Commandを生成までの流れを書く
            // 8: OperationTypeを増築

            nodeList = GenerateOperationNodes(owner, commandRunner, gameMaster);

            currentCondition = new Condition(new ConditionType[] { });
        }

        public void SetGoal(Condition goal)
        {
            goalCondition = goal;
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
