using System.Collections;
using System.Collections.Generic;
using Conductor.Game.Model;
using UnityEngine;

namespace Conductor.Game
{
    public class ActorUpdater
    {
        List<ActorModelBase> friends;
        List<ActorModelBase> enemies;
        public ActorUpdater(View.ActorPrefabReference prefabReference)
        {
            ActorFactory factory = new ActorFactory(prefabReference);
        }
    }
}
