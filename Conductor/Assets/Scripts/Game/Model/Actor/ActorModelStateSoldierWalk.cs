using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor.Game.View;

namespace Conductor.Game.Model
{
    public class ActorModelStateSoldierWalk : ActorModelStateBase
    {
        float speed;
        float rotateDegree;

        public ActorModelStateSoldierWalk(ActorModelBase owner, float speed, float rotateDegree) : base(owner)
        {
            this.speed = speed;
            this.rotateDegree = rotateDegree;
        }

        public override void Update()
        { }

        public override void Walk(bool front)
        {
            var velocity = Owner.HorizontalDirection * speed;
            Owner.ViewBase.transform.localPosition += front ? velocity : -velocity;
        }

        public override void Rotate(bool right)
        {
            float delta = right ? rotateDegree : -rotateDegree;
            var rotationDelta = Quaternion.AngleAxis(delta, Vector3.up);
            var rotation = Owner.ViewBase.transform.localRotation;
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
