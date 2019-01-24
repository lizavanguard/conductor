using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public interface IPlanningNodeFactory
    {
        PlanningNode[] Create(OperationType[] operations);
    }
}
