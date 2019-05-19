using System.Collections;
using System.Collections.Generic;
using Conductor.Game.Model;
using UnityEngine;

namespace Conductor.Game
{
    public class GameMaster : MonoBehaviour
    {
        CommandRunner commandRunner;

        [SerializeField]
        View.ActorPrefabReference actorPrefabReference;

        ActorUpdater actorUpdater;

        [SerializeField]
        Vector3 targetPosition;

        [SerializeField] Camera mainCamera;

        OperationBase operation;
        FieldModel field;
        CursorModel cursor;
        CameraController cameraController;

        public ActorUpdater ActorUpdater
        {
            get { return actorUpdater; }
        }

        /// <summary>
        /// 各Modelの生成と初期化
        /// </summary>
        private void Awake()
        {
            commandRunner = new CommandRunner();

            // FIXME: マップ作成
            var fieldView = Instantiate(actorPrefabReference.Field, transform);
            field = new FieldModel(fieldView);

            // キャラクター作成
            actorUpdater = new ActorUpdater(actorPrefabReference, commandRunner, this);

            // FIXME: CameraControllerを作ったほうがよさげ

            // カーソル
            var cursorView = Instantiate(actorPrefabReference.Cursor, transform);
            cursor = new CursorModel(cursorView, actorUpdater.Friends[0], field, actorUpdater, mainCamera);

            // カメラ
            cameraController = new CameraController(mainCamera, actorUpdater.Friends[0]);
        }

        /// <summary>
        /// 全オブジェクトの更新の発火
        /// </summary>
        private void Update()
        {
            actorUpdater.Update();

            // FIXME: after updating each component
            commandRunner.Update();

            actorUpdater.SetActorHeightOnField(field);

            cameraController.Update();

            cursor.Update();
        }
    }
}