using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
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
}