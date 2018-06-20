using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game
{
    public class GameMaster : MonoBehaviour
    {
        // 生成したほとんどのインスタンスにこれを食わせる
        MessageBus.Dispatcher messageBusDispatcher;
        CommandRunner commandRunner;

        /// <summary>
        /// 各Modelの生成と初期化
        /// </summary>
        private void Awake()
        {
            messageBusDispatcher = new MessageBus.Dispatcher();
            commandRunner = new CommandRunner();
        }

        /// <summary>
        /// 全オブジェクトの更新の発火
        /// </summary>
        private void Update()
        {
            // FIXME: after updating each component
            commandRunner.Update();

            messageBusDispatcher.Dispatch();
        }
    }
}