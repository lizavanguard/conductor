using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor.Game.View;

namespace Conductor.Game.Model
{
    public abstract class ActorModelBase
    {
        static int nextId = 0;

        public enum StateType
        {
            Idle,
            Walk,
            Attack
        }

        public enum ArmyGroupSide
        {
            Friend,
            Enemy
        }

        ActorModelStateBase currentState;
        public ActorModelStateBase CurrentState { get { return currentState; } }
        StateType currentStateType;
        protected StateType CurrentStateType { get { return CurrentStateType; } } 
        Dictionary<StateType, ActorModelStateBase> stateMap;

        int id;
        public int Id { get { return id; } }

        ActorViewBase viewBase;

        ArmyGroupSide groupSide;
        public ArmyGroupSide GroupSide { get { return groupSide; } }

        public ActorViewBase ViewBase { get { return viewBase; } }

        Vector3 horizontalDirection;

        public Vector3 Position { get { return ViewBase.transform.localPosition; } }

        public Vector3 HorizontalDirection
        {
            set { horizontalDirection = value; }
            get { return horizontalDirection; }
        }

        public ActorModelBase(ActorViewBase viewBase, ArmyGroupSide armySide)
        {
            this.viewBase = viewBase;
            this.groupSide = armySide;

            // FIXME: 初期化用のinfo構造体から初期化する
            horizontalDirection = Vector3.forward;
            viewBase.transform.localPosition = Vector3.zero;

            // 初期状態をViewに反映
            viewBase.UpdateRotationByDirection(horizontalDirection);

            stateMap = CreateStateMap();
            currentStateType = StateType.Idle;
            currentState = stateMap[currentStateType];
            currentState.OnEnter();

            id = nextId;
            nextId++;
        }

        protected abstract Dictionary<StateType, ActorModelStateBase> CreateStateMap();

        public abstract void Update();

        public abstract void Walk(bool front);

        public abstract void Rotate(bool right);

        public abstract void Attack();

        public void SetState(StateType state)
        {
            if (currentStateType != state)
            {
                currentState.OnLeave();
                currentState = stateMap[state];
                currentState.OnEnter();
            }

            currentStateType = state;
        }
    }
}
