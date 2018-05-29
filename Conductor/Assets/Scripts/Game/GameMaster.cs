using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Consuctor.Game
{
    public class GameMaster : MonoBehaviour
    {
        // 生成したほとんどのインスタンスにこれを食わせる
        MessageBus.Manager messageBusManager;

        /// <summary>
        /// 各Modelの生成と初期化
        /// </summary>
        private void Awake()
        {
            messageBusManager = new MessageBus.Manager();
        }

        /// <summary>
        /// 全オブジェクトの更新の発火
        /// </summary>
        private void Update()
        {
            
        }
    }
}