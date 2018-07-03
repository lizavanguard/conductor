using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.View
{
    public class ActorViewBase : MonoBehaviour
    {
        public void UpdateRotationByDirection(Vector3 direction)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, direction);
        }
    }
}
