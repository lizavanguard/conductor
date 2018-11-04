using System.Collections;
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
    }
}
