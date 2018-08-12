using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public abstract class OperationBase
    {
        ActorModelBase owner;
        public OperationBase(ActorModelBase owner)
        {
            this.owner = owner;
        }
    }
}
