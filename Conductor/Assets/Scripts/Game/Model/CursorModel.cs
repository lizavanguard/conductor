using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor.Game.View;

namespace Conductor.Game.Model
{
    public class CursorModel
    {
        CursorView view;
        ActorModelBase mainActor;
        FieldModel field;
        Vector3 position;
        ActorUpdater actorUpdater;

        public CursorModel(CursorView view, ActorModelBase mainActor, FieldModel field, ActorUpdater actorUpdater)
        {
            this.view = view;
            this.mainActor = mainActor;
            this.field = field;
            this.actorUpdater = actorUpdater;
        }

        public void Update()
        {
            // 位置更新

            // 近くにいるactorをさがす

            // 近くにいたらそいつを指すようにViewに位置を与える


        }
    }
}
