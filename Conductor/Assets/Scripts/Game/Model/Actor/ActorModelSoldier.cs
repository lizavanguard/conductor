using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor.Game.View;
using System;

namespace Conductor.Game.Model
{
    public class ActorModelSoldier : ActorModelBase
    {
        // 分速60m換算
        static readonly float WalkSpeed = 60.0f / 3600.0f;
        static readonly float RotateDegree = 1.0f;
        int walkBusId;
        int rorateBusId;

        float directionAngleDegree;

        public ActorModelSoldier(ActorViewSoldier viewSoldier)
            : base(viewSoldier)
        {
        }

        public override void Update() { }

        public override void Walk(bool front)
        {
            // 位置更新
            var velocity = HorizontalDirection * WalkSpeed;
            ViewBase.transform.localPosition += front ? velocity : -velocity;

            // FIXME: Viewに状態通知
            
        }

        public override void Rotate(bool right)
        {
            float delta = right ? RotateDegree : -RotateDegree;
            directionAngleDegree += delta;
            directionAngleDegree = Mathf.Repeat(directionAngleDegree + 180.0f, 360.0f) - 180.0f;

            var rotation = Quaternion.AngleAxis(directionAngleDegree, Vector3.up);
            ViewBase.transform.localRotation = rotation;

            // front更新
            HorizontalDirection = rotation * Vector3.forward;

            // FIXME: Viewに状態通知

        }
    }
}
