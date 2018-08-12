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

        public OperationMove(ActorModelBase owner, CommandRunner commandRunner, Vector3 targetPosition) : base(owner, commandRunner)
        {
            this.targetPosition = targetPosition;
        }

        public override void Run()
        {
            // FIXME: 目的地に一直線だが本当なら逐次ナビゲーションが行き先を示す 局所的な情報なのでPQS
            Vector3 tempTargetPosition = targetPosition;
            Vector3 currentPosition = Owner.ViewBase.transform.localPosition;

            // 回転方向決定
            Vector3 direction = tempTargetPosition - currentPosition;
            direction.y = 0.0f;
            if (direction.sqrMagnitude < Epsilon * Epsilon)
            {
                return;
            }

            // FIXME: 雑なので調整 最終的には前、前右、前左、その場の4点でスコアリングすればいい
            bool rotate = Vector3.Dot(direction, Owner.HorizontalDirection) > Mathf.Cos(2.0f * Mathf.Deg2Rad);
            if(rotate)
            {
                bool right = Vector3.Cross(Owner.HorizontalDirection, direction).z < 0;
                var rotateCommand = new CommandModelActorRotate(Owner, right);
                CommandRunner.Schedule(rotateCommand);
            }

            // 向きの一致度と距離に応じて全身後退を判別
            float toTargetLength = direction.magnitude;
            direction /= toTargetLength;

            // FIXME: 雑なので調整
            bool move = Vector3.Dot(direction, Owner.HorizontalDirection) < 0.5f;
            if (move)
            {
                var walkCommand = new CommandModelActorWalk(Owner, true);
                CommandRunner.Schedule(walkCommand);
            }
        }

        public override bool HasFinished()
        {
            Vector3 toTarget = targetPosition - Owner.ViewBase.transform.localPosition;
            return toTarget.sqrMagnitude < Epsilon * Epsilon;
        }
    }
}
