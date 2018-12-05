using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor.Game.Model;
using Conductor.Game.View;

namespace Conductor.Game
{
    public class ActorFactory
    {
        ActorPrefabReference prefabReference;

        public ActorFactory(ActorPrefabReference prefabReference)
        {
            this.prefabReference = prefabReference;
        }

        public ActorModelSoldier CreateSoldier(ActorModelBase.ArmyGroupSide armySide)
        {
            var view = GameObject.Instantiate<ActorViewSoldier>(prefabReference.Soldier);
            var model = new ActorModelSoldier(view, armySide);

            return model;
        }
    }
}
