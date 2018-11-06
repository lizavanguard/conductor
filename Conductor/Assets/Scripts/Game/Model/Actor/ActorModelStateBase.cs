using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor.Game.View;

namespace Conductor.Game.Model
{
    public abstract class ActorModelStateBase
    {
        ActorModelBase owner;
        protected ActorModelBase Owner { get { return owner; } }

        public ActorModelStateBase(ActorModelBase owner)
        {
            this.owner = owner;
        }

        public abstract void Update();

        public abstract void Walk(bool front);

        public abstract void Rotate(bool right);

        public abstract void Attack();

        public abstract void OnEnter();

        public abstract void OnLeave();
    }
}
