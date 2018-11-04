﻿using System.Collections;
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

        // FIXME: もしかしたらactor側の機能として実装したほうが自然かも actor都合がどのくらいかによりそう
        // FIXME: 共有範囲で更新ロジックを区切ったほうがいい 例えば戦場全体で共通な状態は1フレームに1回更新すればよい
        public void UpdateCondition(ActorModelBase owner, GameMaster gameMaster)
        {
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
            foreach (var enemy in gameMaster.MockEnemies)
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


        #endregion
    }
}