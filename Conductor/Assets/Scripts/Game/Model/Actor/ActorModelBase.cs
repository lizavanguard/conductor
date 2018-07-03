using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor.Game.View;

namespace Conductor.Game.Model
{
    public abstract class ActorModelBase
    {
        static int nextId = 0;

        int id;

        public int Id { get { return id; } }

        // FIXME: ActorViewBaseができたらその参照を持つようにする
        ActorViewBase viewBase;

        protected ActorViewBase ViewBase { get { return viewBase; } }

        public ActorModelBase(ActorViewBase viewBase)
        {
            this.viewBase = viewBase;
            id = nextId;
            nextId++;
        }

        public abstract void Update();
    }
}
