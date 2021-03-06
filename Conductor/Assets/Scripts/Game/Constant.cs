﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor
{
    public class Constant
    {
        // 角度一致判定のための内積値のepsilon
        public const float ActorAngleCrossEpsilon = 0.1f;

        // キャラクター同士が十分に近くにいるかどうかの判定用epsilon
        public const float ActorPositionDistanceEpsilon = 0.1f;
        public const float ActorPositionDistanceSqEpsilon = ActorPositionDistanceEpsilon * ActorPositionDistanceEpsilon;

        // 攻撃が届くかどうかの閾値 FIXME: 勢い余って定数化したけどこれActorModelBaseに個々に定義するやつだわ
        public const float ActorAttackDistanceThreshold = 1.0f;

        // 歩兵交戦距離の閾値 FIXME: 勢い余って定数化したけどこれActorModelBaseに個々に定義するやつだわ 数値も割とザル
        public const float SoldierTargettingDistanceThreshold = 10.0f;
        public const float SoldierTargettingDistanceSqThreshold = SoldierTargettingDistanceThreshold * SoldierTargettingDistanceThreshold;
    }
}
