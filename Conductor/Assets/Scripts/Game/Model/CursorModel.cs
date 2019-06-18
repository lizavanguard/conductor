using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor.Game.View;

namespace Conductor.Game.Model
{
    public class CursorModel
    {
        const float TargetingRange = 1.0f;

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
            var target = SearchNearestCaptain(position);

            // 近くにいたらそいつを指すようにViewに位置を与える
            if (target != null)
            {
                position = target.Position + Vector3.up * 2.0f;
            }

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

        ActorModelBase SearchNearestCaptain(Vector3 position)
        {
            // FIXME: この収集作業はUpdaterが行ってキャッシュするほうがいいと思います
            var captains = new List<ActorModelBase>();
            foreach (var friend in actorUpdater.Friends)
            {
                if (actorUpdater.GetCaptainAI(friend.Id) != null)
                {
                    captains.Add(friend);
                }
            }

            foreach (var enemy in actorUpdater.Enemies)
            {
                if (actorUpdater.GetCaptainAI(enemy.Id) != null)
                {
                    captains.Add(enemy);
                }
            }

            ActorModelBase target = null;
            float min = float.MaxValue;
            foreach (var captain in captains)
            {
                float sqDistance = (captain.Position - position).sqrMagnitude;
                if (sqDistance > TargetingRange * TargetingRange)
                {
                    continue;
                }

                if (sqDistance < min)
                {
                    min = sqDistance;
                    target = captain;
                }
            }

            return target;
        }
    }
}
