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

        [SerializeField]
        View.ActorPrefabReference actorPrefabReference;

        ActorFactory actorFactory;

        Model.ActorModelSoldier mockSoldier;

        /// <summary>
        /// 各Modelの生成と初期化
        /// </summary>
        private void Awake()
        {
            messageBusDispatcher = new MessageBus.Dispatcher();
            commandRunner = new CommandRunner();

            // FIXME: マップ作成

            // キャラクター作成 FIXME: 一旦モックとしてこう作るが管理クラスを設けたい
            actorFactory = new ActorFactory(actorPrefabReference);
            mockSoldier = actorFactory.CreateSoldier();
        }

        /// <summary>
        /// 全オブジェクトの更新の発火
        /// </summary>
        private void Update()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                var command = new Model.CommandModelActorWalk(messageBusDispatcher, mockSoldier.Id, true);
                commandRunner.Schedule(command);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                var command = new Model.CommandModelActorWalk(messageBusDispatcher, mockSoldier.Id, false);
                commandRunner.Schedule(command);
            }

            // FIXME: after updating each component
            commandRunner.Update();

            messageBusDispatcher.Dispatch();
        }
    }
}