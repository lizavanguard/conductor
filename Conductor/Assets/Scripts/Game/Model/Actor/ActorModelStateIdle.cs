using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor.Game.View;

namespace Conductor.Game.Model
{
    public class ActorModelStateIdle : ActorModelStateBase
    {
        float rotateDegree;

        public ActorModelStateIdle(ActorModelBase owner, float rotateDegree) : base(owner)
        {
            this.rotateDegree = rotateDegree;
        }

        public override void Update()
        { }

        public override void Walk(bool front)
        {
            Owner.SetState(ActorModelBase.StateType.Walk);

            // NOTE: 下手すると無限ループを起こすコードなので注意
            Owner.Walk(front);
        }

        public override void Rotate(bool right)
        {
            float delta = right ? rotateDegree : -rotateDegree;
            var rotationDelta = Quaternion.AngleAxis(delta, Vector3.up);
            var rotation = Owner.ViewBase.transform.localRotation * rotationDelta;
            Owner.ViewBase.transform.localRotation = rotation;

            // front更新
            Owner.HorizontalDirection = rotation * Vector3.forward;
        }

        public override void OnEnter()
        { }

        public override void OnLeave()
        { }
    }
}
