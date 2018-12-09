using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    interface IPlanningNodeFactory
    {
        PlanningNode[] Create();
    }
}
