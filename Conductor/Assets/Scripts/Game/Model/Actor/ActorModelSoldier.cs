using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor.Game.View;
using System;

namespace Conductor.Game.Model
{
    public class ActorModelSoldier : ActorModelBase, IDisposable
    {
        // 分速60m換算
        static readonly float WalkSpeed = 60.0f / 3600.0f;
        int walkBusId;

        public ActorModelSoldier(ActorViewSoldier viewSoldier)
            : base(viewSoldier)
        {
            ConnectMessageBus();
        }

        public override void Update() { }

        public override void Walk(bool front)
        {
            // 位置更新
            var velocity = HorizontalDirection * WalkSpeed;
            ViewBase.transform.localPosition += front ? velocity : -velocity;

            // FIXME: Viewに状態通知
            
        }

        public void Dispose()
        {
            DisconnectMessageBus();
        }

        void ConnectMessageBus()
        {
            Action<MessageBus.ActorWalkBus.Message> walkMessageHandler = message =>
            {
                Walk(message.Front);
            };
            walkBusId = MessageBus.ActorWalkBus.Connect(this.Id, walkMessageHandler);
        }

        void DisconnectMessageBus()
        {
            MessageBus.ActorWalkBus.Disconnect(walkBusId);
        }
    }
}
