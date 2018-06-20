using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public abstract class ActorModelBase
    {
        static int nextId = 0;

        int id;

        public int Id { get { return id; } }

        // FIXME: ActorViewBaseができたらその参照を持つようにする

        public ActorModelBase()
        {
            id = nextId;
            nextId++;
        }

        public abstract void Update();
    }
}
