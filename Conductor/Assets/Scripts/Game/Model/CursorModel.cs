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
        Camera mainCamera;

        public CursorModel(CursorView view, ActorModelBase mainActor, FieldModel field, ActorUpdater actorUpdater, Camera mainCamera)
        {
            this.view = view;
            this.mainActor = mainActor;
            this.field = field;
            this.actorUpdater = actorUpdater;
            this.mainCamera = mainCamera;
        }

        public void Update()
        {
            // 位置更新
            position = UpdatePosition();

            // 近くにいるactorをさがす

            // 近くにいたらそいつを指すようにViewに位置を与える
            view.transform.position = position;

        }

        Vector3 UpdatePosition()
        {
            // マウスカーソル方向にレイを飛ばす
            var screenPosition = Input.mousePosition;
            screenPosition.z = 1f;
            var mousePosition = mainCamera.ScreenToWorldPoint(screenPosition);
            var rayDirection = mousePosition - mainCamera.transform.position;
            if (Mathf.Abs(rayDirection.y) < 0.01)
            {
                return this.position;
            }

            float scale = (mainActor.Position.y - mainCamera.transform.position.y) / rayDirection.y;
            Vector3 ray = rayDirection * scale;

            var cuesorPosition = mainCamera.transform.position + ray;
            float fieldHeight = field.GetHeight(cuesorPosition);
            cuesorPosition.y = fieldHeight;

            return cuesorPosition;
        }
    }
}
