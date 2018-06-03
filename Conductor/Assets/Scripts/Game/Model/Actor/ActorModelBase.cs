using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public abstract class ActorModelBase
    {
        // FIXME: ActorViewBaseができたらその参照を持つようにする

        public ActorModelBase()
        { }

        public abstract void Update();
    }
}
