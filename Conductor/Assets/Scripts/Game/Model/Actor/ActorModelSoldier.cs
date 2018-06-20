using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Conductor.Game.Model
{
    public class ActorModelSoldier : ActorModelBase, IDisposable
    {
        int walkBusId;

        public ActorModelSoldier()
        {
            // FIXME: viewのGameObject作成 作成済みのものを引数で貰うのもあり

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
                // 前進する FIXME: implemet me, ha ha ha!
                Debug.Log("あるくよ！");
            };
            walkBusId = MessageBus.ActorWalkBus.Connect(this.Id, walkMessageHandler);
        }

        void DisconnectMessageBus()
        {
            MessageBus.ActorWalkBus.Disconnect(walkBusId);
        }
    }
}
