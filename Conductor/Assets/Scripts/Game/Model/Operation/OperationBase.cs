using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public abstract class OperationBase
    {
        ActorModelBase owner;
        protected ActorModelBase Owner { get { return owner; } }

        public OperationBase(ActorModelBase owner)
        {
            this.owner = owner;
        }

        public abstract void Run();

        public abstract bool HasFinished();
    }
}
