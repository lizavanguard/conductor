using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Consuctor.Game
{
    public class GameMaster : MonoBehaviour
    {
        CommandRunner commandRunner;

        /// <summary>
        /// 各Modelの生成と初期化
        /// </summary>
        private void Awake()
        {
            commandRunner = new CommandRunner();
        }

        /// <summary>
        /// 全オブジェクトの更新の発火
        /// </summary>
        private void Update()
        {

            // FIXME: after updating each component
            commandRunner.Update();
        }
    }
}