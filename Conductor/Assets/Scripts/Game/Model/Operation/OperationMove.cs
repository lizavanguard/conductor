using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class OperationMove : OperationBase
    {
        // 比較用イプシロン ちょっと広めに
        static readonly float Epsilon = 0.1f;
        Vector3 targetPosition;

        public OperationMove(ActorModelBase owner, Vector3 targetPosition) : base(owner)
        {
            this.targetPosition = targetPosition;
        }

        public override void Run()
        {
            // FIXME: 目的地に一直線だが本当なら逐次ナビゲーションが行き先を示す 局所的な情報なのでPQS
            Vector3 tempTargetPosition = targetPosition;
            Vector3 currentPosition = Owner.ViewBase.transform.localPosition;

            // 向きの一致度と距離に応じて全身後退を判別
            Vector3 direction = tempTargetPosition - currentPosition;
            direction.y = 0.0f;
            if (direction.sqrMagnitude < Epsilon * Epsilon)
            {
                return;
            }

            float toTargetLength = direction.magnitude;
            direction /= toTargetLength;

            bool move = false;
            move = Vector3.Dot(direction, Owner.HorizontalDirection) < 0.5f;

            // 回転方向決定

        }

        public override bool HasFinished()
        {
            return false;
        }
    }
}
