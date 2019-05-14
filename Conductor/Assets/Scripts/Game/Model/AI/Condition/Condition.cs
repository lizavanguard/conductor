using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public interface ICondition
    {

    }

    public enum SoldierConditionType
    {
        // 誰でもいいから敵の方を向いている
        LookToSomeEnemy,

        // 移動して攻撃可能な敵を発見している
        CanTargetSomeEnemy,

        // 敵に攻撃している 達成不可能候補
        HittingSomeEnemy,

        // TargetPointの近くにいる
        StayNearTargetPoint,
    }

    public class Condition
    {
        // ConditionTypeの0～31に対応するフラグ
        // 該当bitがtrue => その条件は成立している
        // 該当bitがfalse => その条件の成否は不成立
        // 明示的に不成立の条件を入れる時は、専用のbitを作る
        uint conditionFlag;
        public uint ConditionFlag { get { return conditionFlag; } }

        // 引数はConditionTypeのuintキャスト
        public Condition(int[] trueList)
        {
            conditionFlag = 0;
            foreach (var typeNum in trueList)
            {
                conditionFlag |= 1U << typeNum;
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

        // 特定のひとつを満たしているかどうか 引数はConditionTypeのintキャスト
        public bool Satisfy(int typeNum)
        {
            var mask = 1U << typeNum;
            return (conditionFlag & mask) != 0;
        }

        // 引数はConditionTypeのintキャスト
        public void SetFlag(int typeNum, bool flag)
        {
            var mask = 1U << typeNum;

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

    public enum CaptainConditionType
    {
        // 円陣が完成している
        CaptainCompleteCircleFormation,
    }
}