using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public abstract class OperationBase
    {
        ActorModelBase owner;
        protected ActorModelBase Owner { get { return owner; } }

        CommandRunner commandRunner;
        protected CommandRunner CommandRunner { get { return commandRunner; } }

        public OperationBase(ActorModelBase owner, CommandRunner commandRunner)
        {
            this.owner = owner;
            this.commandRunner = commandRunner;
        }

        public abstract void Run();

        public abstract bool HasFinished();
    }
}
