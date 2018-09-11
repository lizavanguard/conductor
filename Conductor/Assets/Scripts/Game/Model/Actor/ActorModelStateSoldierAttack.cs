using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor.Game.View;

namespace Conductor.Game.Model
{
    public class ActorModelStateSoldierAttack : ActorModelStateBase
    {
        const int AttackFrame = 15;
        const int FrameLength = 60;
        int frameCount;

        public ActorModelStateSoldierAttack(ActorModelBase owner) : base(owner)
        {
        }

        public override void Update()
        {
            if (frameCount == AttackFrame)
            {
                // 攻撃判定発生

            }

            // 終了判定 FIXME: animatorの状態を見たほうがいいかも？
            if (frameCount >= FrameLength)
            {
                Owner.SetState(ActorModelBase.StateType.Idle);
            }

            frameCount++;
        }

        public override void Walk(bool front)
        {
        }

        public override void Rotate(bool right)
        {
        }

        public override void Attack()
        {
        }

        public override void OnEnter()
        {
            frameCount = 0;
            Owner.ViewBase.PlayAttackAnimation();
        }

        public override void OnLeave()
        {
            Owner.ViewBase.PlayIdleAnimation();
        }
    }
}
