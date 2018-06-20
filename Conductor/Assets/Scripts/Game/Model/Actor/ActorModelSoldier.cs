using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Conductor.Game.Model
{
    public class ActorModelSoldier : ActorModelBase, IDisposable
    {
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
        }

        void DisconnectMessageBus()
        {
        }
    }
}
