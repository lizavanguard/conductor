using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor.Game.View;
using System;

namespace Conductor.Game.Model
{
    public class ActorModelSoldier : ActorModelBase, IDisposable
    {
        int walkBusId;

        public ActorModelSoldier(ActorViewSoldier viewSoldier)
            : base(viewSoldier)
        {
            ConnectMessageBus();
        }

        public override void Update() { }

        public void Dispose()
        {
            DisconnectMessageBus();
        }

        void ConnectMessageBus()
        {
            Action<MessageBus.ActorWalkBus.Message> walkMessageHandler = message =>
            {
                // 前進する
                base.ViewBase.Walk();
            };
            walkBusId = MessageBus.ActorWalkBus.Connect(this.Id, walkMessageHandler);
        }

        void DisconnectMessageBus()
        {
            MessageBus.ActorWalkBus.Disconnect(walkBusId);
        }
    }
}
