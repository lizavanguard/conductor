using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public enum ConditionType
    {
        #region Soldier用
        // 誰でもいいから敵の方を向いている
        LookToSomeEnemy,

        // 移動して攻撃可能な敵を発見している
        CanTargetSomeEnemy,

        // 敵に攻撃している 達成不可能候補
        HittingSomeEnemy,

        // TargetPointの近くにいる
        StayNearTargetPoint,
        #endregion

        #region Captain用
        // 円陣が完成している
        CaptainCompleteCircleFormation,
        #endregion
    }

    public class Condition
    {
        // ConditionTypeの0～31に対応するフラグ
        // 該当bitがtrue => その条件は成立している
        // 該当bitがfalse => その条件の成否は不成立
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