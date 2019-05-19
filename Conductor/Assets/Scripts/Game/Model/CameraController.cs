using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class CameraController
    {
        Camera targetCamera;
        ActorModelBase targetActor;
        Vector3 offset;

        public CameraController(Camera targetCamera, ActorModelBase targetActor)
        {
            this.targetCamera = targetCamera;
            this.targetActor = targetActor;
            offset = targetCamera.transform.position;
        }

        public void Update()
        {
            var position = targetActor.Position + offset;
            targetCamera.transform.position = position;
        }
    }
}
