using System.Collections;
using System.Collections.Generic;
using Conductor.Game.Model;
using UnityEngine;

namespace Conductor.Game
{
    /// <summary>
    /// Updateをぐるぐる回すだけのひと
    /// TODO: あまりに仕事がないからaccessorも兼ねているが、肥大化するならインスタンス管理と処理の発火を別クラスに分けてもいい
    /// 仕事が増えたらクラス名変えるかも
    /// </summary>
    public class ActorUpdater
    {
        ActorModelBase[] friends;
        ActorModelBase[] enemies;
        SoldierAI[] friendAIs;
        SoldierAI[] enemyAIs;

        public ActorModelBase[] Enemies { get { return enemies; } }
        public ActorModelBase[] Friends { get { return friends; } }

        public ActorUpdater(View.ActorPrefabReference prefabReference, CommandRunner commandRunner, GameMaster gameMaster)
        {
            ActorFactory factory = new ActorFactory(prefabReference);

            // FIXME: 数は適当なので外部から読み込むときにYOSHINANI
            enemies = new ActorModelBase[4];
            enemyAIs = new SoldierAI[4];
            for (int i = 0; i < enemies.Length; i++)
            {
                var enemy = factory.CreateSoldier(ActorModelBase.ArmyGroupSide.Enemy);
                enemy.ViewBase.transform.localPosition = new Vector3((float)i, 0.0f, 4.0f);
                enemies[i] = enemy;

                enemyAIs[i] = new SoldierAI(enemy, commandRunner, gameMaster);
            }

            friends = new ActorModelBase[1];
            friendAIs = new SoldierAI[1];
            for (int i = 0; i < friends.Length; i++)
            {
                var friend = factory.CreateSoldier(ActorModelBase.ArmyGroupSide.Friend);

                // AI動作確認のため敵から遠くに
                friend.ViewBase.transform.localPosition = new Vector3((float)i, 0.0f, -20.0f);
                friends[i] = friend;

                friendAIs[i] = new SoldierAI(friend, commandRunner, gameMaster);
            }

            // AI初期化はActorModelを全て生成し終えてから
            foreach (var ai in friendAIs)
            {
                ai.Initialize();
            }

            foreach (var ai in enemyAIs)
            {
                ai.Initialize();
            }
        }

        public void Update()
        {
            // AI構築中はこうする そのうちデバッグメニューに移すとかinspectorに逃がすとかしたい
            if (Input.GetKey(KeyCode.Space))
            {
                foreach (var ai in friendAIs)
                {
                    ai.Update();
                }
            }

            foreach (var friend in friends)
            {
                friend.Update();
            }

            foreach (var ai in enemyAIs)
            {
                // 敵 == enemyMockとして味方用AIとしての実装しかしていないので、敵対陣営の概念を実装するまで更新切る
                // ai.Update();
            }

            foreach (var enemy in enemies)
            {
                enemy.Update();
            }
        }
    }
}
