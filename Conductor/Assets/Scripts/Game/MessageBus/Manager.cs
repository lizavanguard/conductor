using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Consuctor.Game.MessageBus
{
    /// <summary>
    /// 各MessageBusインスタンスの生成、保持とDispatchのみ請け負う
    /// Dispatchに関しては一覧化することで順番の制御を容易にする目的
    /// 大体全てのクラスがこいつのインスタンスを持っていていつでもメッセージを送れる
    /// デバッグのためGetPropertyにログを仕込むなどの使い方もする想定
    /// </summary>
    public class Manager
    {
        public ActorWalkBus ActorWalkBus { get; private set; }

        public Manager()
        {
            ActorWalkBus = new ActorWalkBus();
        }

        public void Dispatch()
        {
            ActorWalkBus.Dispatch();
        }
    }
}
