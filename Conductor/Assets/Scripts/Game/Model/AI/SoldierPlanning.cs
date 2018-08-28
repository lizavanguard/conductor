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
        // 行動はどのoperation派生クラスを用いるかの判断になる
        class Node
        {
            // 前提条件
            Condition beforeCondition;

            // 目標条件
            Condition afterCondition;

            // 行動
            
        }

        // FIXME: OperationFactoryかOperationBaseに置く
        public enum OperationType
        {
            // 特定地点へ移動する
            MoveToTargetPoint,

            // 最も近くにいる敵対陣営のキャラのほうを向く
            LookToNearestEnemy
        }

        // 条件ズ
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
            uint firstConditionFlag;
            public uint FirstConditionFlag { get { return firstConditionFlag; } }

            public Condition(ConditionType[] trueList)
            {
                firstConditionFlag = 0;
                foreach (var type in trueList)
                {
                    firstConditionFlag |= 1U << (int)type;
                }
            }

            public bool Satisfy(Condition other)
            {
                return other.firstConditionFlag == (firstConditionFlag & other.firstConditionFlag);
            }

            public bool IsSatisfiedBy(Condition other)
            {
                return other.Satisfy(this);
            }
        }

        // 候補のNode
        Node[] nodeList;

        // プランニングの最終目標
        Condition goalCondition;

        // 現在構築中のプランニング
        List<Node> currentPlanningChain;

        public SoldierPlanning()
        {
            // Nodeのモックを用意して動かしてみる
            // 1. 方向転換と攻撃のOperationを書く 方向転換作業中
            // 2. OperationTypeからOperationを作るファクトリーを書く
            // 3. Nodeのコンストラクタを書く
            // 4. 各Operationに対応したNodeを作る
            // 5. AI側にもConditionを持たせて定期更新を行う
            // 6. PlanningChain構築メソッドを書く
            // 7. 構築、Operation決定、Commandを生成までの流れを書く
        }

        public void SetGoal(Condition goal)
        {
            goalCondition = goal;
        }
    }
}
