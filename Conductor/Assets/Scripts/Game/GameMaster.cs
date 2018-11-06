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

        ActorFactory actorFactory;

        Model.ActorModelSoldier mockSoldier;

        Model.ActorModelBase[] mockEnemies;
        public Model.ActorModelBase[] MockEnemies { get { return mockEnemies; } }

        Model.SoldierAI[] mockEnemyAIs;
        Model.SoldierAI mockSoldierAI;

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

            // キャラクター作成 FIXME: 一旦モックとしてこう作るが管理クラスを設けたい
            actorFactory = new ActorFactory(actorPrefabReference);
            mockSoldier = actorFactory.CreateSoldier();

            // AI動作確認のため敵から遠くに
            mockSoldier.ViewBase.transform.localPosition = new Vector3(0.0f, 0.0f, -20.0f);

            mockEnemies = new Model.ActorModelBase[4];
            mockEnemyAIs = new Model.SoldierAI[4];
            for (int i = 0; i < mockEnemies.Length; i++)
            {
                var enemy = actorFactory.CreateSoldier();
                enemy.ViewBase.transform.localPosition = new Vector3((float)i, 0.0f, 4.0f);
                mockEnemies[i] = enemy;

                mockEnemyAIs[i] = new Model.SoldierAI(enemy, commandRunner, this);
            }

            foreach (var ai in mockEnemyAIs)
            {
                ai.Initialize();
            }

            mockSoldierAI = new Model.SoldierAI(mockSoldier, commandRunner, this);
            mockSoldierAI.Initialize();
        }

        /// <summary>
        /// 全オブジェクトの更新の発火
        /// </summary>
        private void Update()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                var command = new Model.CommandModelActorWalk(mockSoldier, true);
                commandRunner.Schedule(command);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                var command = new Model.CommandModelActorWalk(mockSoldier, false);
                commandRunner.Schedule(command);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                var command = new Model.CommandModelActorRotate(mockSoldier, true);
                commandRunner.Schedule(command);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                var command = new Model.CommandModelActorRotate(mockSoldier, false);
                commandRunner.Schedule(command);
            }

            foreach (var ai in mockEnemyAIs)
            {
                // 敵 == enemyMockとして味方用AIとしての実装しかしていないので、敵対陣営の概念を実装するまで更新切る
                // ai.Update();
            }

            if (mockSoldier != null)
            {
                mockSoldier.Update();
            }

            foreach (var enemy in mockEnemies)
            {
                enemy.Update();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                mockSoldierAI.Update();
            }

            // FIXME: after updating each component
            commandRunner.Update();
        }
    }
}