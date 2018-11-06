using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class SoldierNavigation
    {
        ActorModelBase owner;

        public SoldierNavigation(ActorModelBase owner)
        {
            this.owner = owner;
        }
    }
}
