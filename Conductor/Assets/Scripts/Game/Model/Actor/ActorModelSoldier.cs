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

        protected override Dictionary<StateType, ActorModelStateBase> CreateStateMap()
        {
            var stateMap = new Dictionary<StateType, ActorModelStateBase>();

            var idleState = new ActorModelStateIdle(this, RotateDegree);
            stateMap.Add(StateType.Idle, idleState);

            var walkState = new ActorModelStateSoldierWalk(this, WalkSpeed, RotateDegree);
            stateMap.Add(StateType.Walk, walkState);

            var attackState = new ActorModelStateSoldierAttack(this);
            stateMap.Add(StateType.Attack, attackState);

            return stateMap;
        }

        public override void Update()
        {
            CurrentState.Update();
        }

        public override void Walk(bool front)
        {
            CurrentState.Walk(front);

            // FIXME: Viewに状態通知
            
        }

        public override void Attack()
        {
            CurrentState.Attack();
        }

        public override void Rotate(bool right)
        {
            CurrentState.Rotate(right);

            // FIXME: Viewに状態通知

        }
    }
}
