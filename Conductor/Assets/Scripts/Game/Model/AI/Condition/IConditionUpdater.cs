using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public interface IConditionUpdater
    {
        void Update(Condition target);
    }
}
