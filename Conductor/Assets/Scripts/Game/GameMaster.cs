using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game
{
    public class GameMaster : MonoBehaviour
    {
        CommandRunner commandRunner;

        [SerializeField]
        View.ActorPrefabReference actorPrefabReference;

        ActorUpdater actorUpdater;
        // それぞれの陣営を定義し、「敵陣営取得メソッド」を新たに定義
        public Model.ActorModelBase[] Enemies { get { return actorUpdater.Enemies; } }


        [SerializeField]
        Vector3 targetPosition;

        Model.OperationBase operation;

        /// <summary>
        /// 各Modelの生成と初期化
        /// </summary>
        private void Awake()
        {
            commandRunner = new CommandRunner();

            // FIXME: マップ作成

            // キャラクター作成
            actorUpdater = new ActorUpdater(actorPrefabReference, commandRunner, this);
        }

        /// <summary>
        /// 全オブジェクトの更新の発火
        /// </summary>
        private void Update()
        {
            actorUpdater.Update();

            // FIXME: after updating each component
            commandRunner.Update();
        }
    }
}