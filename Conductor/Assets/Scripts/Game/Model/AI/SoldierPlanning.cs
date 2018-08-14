using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    // モックとして「プレイヤーの方向を向く」と「プレイヤーを攻撃する」を実装する
    // 実際にはこれらのどっちを選ぶかは隊長クラスのplanningから判断される
    public class SoldierPlanning
    {
        class Node
        {
            // 前提条件
            Condition beforeCondition;

            // 目標条件
            Condition afterCondition;

            // 行動
        }

        //
        enum ConditionType
        {
            // 敵の方を向いている
            // 敵に攻撃できる距離にある
            // 敵に攻撃している
        }

        class Condition
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
                return firstConditionFlag == (firstConditionFlag & other.firstConditionFlag);
            }

            public bool IsSatisfiedBy(Condition other)
            {
                return other.Satisfy(this);
            }
        }

        public SoldierPlanning()
        {
        }
    }
}
