using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.MessageBus
{
    /// <summary>
    /// 各MessageBusのDispatchのみ請け負う
    /// Dispatchに関しては一覧化することで順番の制御を容易にする目的
    /// 大体全てのクラスがこいつのインスタンスを持っていていつでもメッセージを送れる
    /// デバッグのためGetPropertyにログを仕込むなどの使い方もする想定
    /// </summary>
    public class Dispatcher
    {
        public void Dispatch()
        {
            ActorWalkBus.Dispatch();
        }
    }
}
