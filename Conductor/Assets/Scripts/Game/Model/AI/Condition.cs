using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public enum ConditionType
    {
        // 誰でもいいから敵の方を向いている
        LookToSomeEnemy,

        // 移動して攻撃可能な敵を発見している
        CanTargetSomeEnemy,

        // 敵に攻撃している
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

        // FIXME: もしかしたらactor側の機能として実装したほうが自然かも actor都合がどのくらいかによりそう
        // FIXME: 共有範囲で更新ロジックを区切ったほうがいい 例えば戦場全体で共通な状態は1フレームに1回更新すればよい
        public void UpdateCondition(ActorModelBase owner, GameMaster gameMaster)
        {
            UpdateLookToEnemyCondition(owner, gameMaster);
            UpdateCanTargetSomeEnemyCondition(owner, gameMaster);
            UpdateHittingSomeEnemyCondition(owner, gameMaster);
            UpdateStayNearTargetPointCondition(owner, gameMaster);
        }

        void SetFlag(ConditionType type, bool flag)
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

        #region 状態更新メソッドたち
        // 誰でもいいから敵の方を向いている
        void UpdateLookToEnemyCondition(ActorModelBase owner, GameMaster gameMaster)
        {
            Vector3 ownerDirection = owner.HorizontalDirection;

            bool directionEqual = false;
            var enemies = gameMaster.ActorUpdater.GetOppositeGroup(owner.GroupSide);
            foreach (var enemy in enemies)
            {
                Vector3 toEnemy = enemy.Position - owner.Position;
                toEnemy.y = 0.0f;

                if (toEnemy.sqrMagnitude < Constant.ActorPositionDistanceSqEpsilon)
                {
                    continue;
                }

                toEnemy.Normalize();
                float cross = Vector3.Cross(toEnemy, ownerDirection).y;
                if (Mathf.Abs(cross) < Constant.ActorAngleCrossEpsilon)
                {
                    directionEqual = true;
                    break;
                }
            }

            SetFlag(ConditionType.LookToSomeEnemy, directionEqual);
        }

        // 誰でもいいから敵にタゲ取れる距離にある
        void UpdateCanTargetSomeEnemyCondition(ActorModelBase owner, GameMaster gameMaster)
        {
            bool near = false;
            var enemies = gameMaster.ActorUpdater.GetOppositeGroup(owner.GroupSide);
            foreach (var enemy in enemies)
            {
                Vector3 toEnemy = enemy.Position - owner.Position;
                toEnemy.y = 0.0f;

                if (toEnemy.sqrMagnitude < Constant.SoldierTargettingDistanceSqThreshold)
                {
                    near = true;
                    break;
                }
            }

            SetFlag(ConditionType.CanTargetSomeEnemy, near);
        }

        // 敵に攻撃している
        void UpdateHittingSomeEnemyCondition(ActorModelBase owner, GameMaster gameMaster)
        {
            // 雑に攻撃状態かどうか見るだけ FIXME: 攻撃が敵に当たっているかどうかも見るべき？
            bool isHitting = owner.CurrentState as ActorModelStateSoldierAttack != null;
            SetFlag(ConditionType.HittingSomeEnemy, isHitting);
        }

        // TargetPointの近くにいる
        void UpdateStayNearTargetPointCondition(ActorModelBase owner, GameMaster gameMaster)
        {
            Vector3 toTarget = owner.TargetPosition - owner.Position;

            // 一応水平距離で比較
            toTarget.y = 0.0f;

            bool near = toTarget.sqrMagnitude < Constant.ActorPositionDistanceSqEpsilon;
            SetFlag(ConditionType.StayNearTargetPoint, near);
        }

        #endregion
    }
}