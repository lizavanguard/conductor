using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class ActorModelSoldier : ActorModelBase
    {
        static int nextId = 0;

        // FIXME: ActorViewBaseができたらその参照を持つようにする

        int id;

        public int Id { get { return id; } }

        public ActorModelSoldier()
        {
            id = nextId;
            nextId++;
        }

        public override void Update() { }
    }
}
