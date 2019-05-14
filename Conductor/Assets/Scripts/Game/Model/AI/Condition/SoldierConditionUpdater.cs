using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class SoldierConditionUpdater : IConditionUpdater
    {
        ActorModelBase owner;
        GameMaster gameMaster;

        public SoldierConditionUpdater(ActorModelBase owner, GameMaster gameMaster)
        {
            this.owner = owner;
            this.gameMaster = gameMaster;
        }

        // FIXME: 共有範囲で更新ロジックを区切ったほうがいい 例えば戦場全体で共通な状態は1フレームに1回更新すればよい
        public void Update(Condition target)
        {
            UpdateLookToEnemyCondition(target);
            UpdateCanTargetSomeEnemyCondition(target);
            UpdateHittingSomeEnemyCondition(target);
            UpdateStayNearTargetPointCondition(target);
        }


        #region 状態更新メソッドたち
        // 誰でもいいから敵の方を向いている
        void UpdateLookToEnemyCondition(Condition target)
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

            target.SetFlag((int)SoldierConditionType.LookToSomeEnemy, directionEqual);
        }

        // 誰でもいいから敵にタゲ取れる距離にある
        void UpdateCanTargetSomeEnemyCondition(Condition target)
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

            target.SetFlag((int)SoldierConditionType.CanTargetSomeEnemy, near);
        }

        // 敵に攻撃している
        void UpdateHittingSomeEnemyCondition(Condition target)
        {
            // 雑に攻撃状態かどうか見るだけ FIXME: 攻撃が敵に当たっているかどうかも見るべき？
            bool isHitting = owner.CurrentState as ActorModelStateSoldierAttack != null;
            target.SetFlag((int)SoldierConditionType.HittingSomeEnemy, isHitting);
        }

        // TargetPointの近くにいる
        void UpdateStayNearTargetPointCondition(Condition target)
        {
            Vector3 toTarget = owner.TargetPosition - owner.Position;

            // 一応水平距離で比較
            toTarget.y = 0.0f;

            bool near = toTarget.sqrMagnitude < Constant.ActorPositionDistanceSqEpsilon;
            target.SetFlag((int)SoldierConditionType.StayNearTargetPoint, near);
        }

        #endregion
    }
}
