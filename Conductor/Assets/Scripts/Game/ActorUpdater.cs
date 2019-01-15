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
        // FIXME: このへんの参照はActorContainerクラスに逃がすのが美しいのでは？？？？
        ActorModelBase[] friends;
        ActorModelBase[] enemies;

        SoldierAI[] friendSoldierAIs;
        SoldierAI[] enemySoldierAIs;

        CaptainAI[] frinedCaptainAIs;
        CaptainAI[] enemyCaptainAIs;

        Dictionary<int, SoldierAI> soldierAIMap;
        Dictionary<int, CaptainAI> captainAIMap;

        public ActorModelBase[] Enemies { get { return enemies; } }
        public ActorModelBase[] Friends { get { return friends; } }

        public ActorUpdater(View.ActorPrefabReference prefabReference, CommandRunner commandRunner, GameMaster gameMaster)
        {
            ActorFactory factory = new ActorFactory(prefabReference);

            // FIXME: 数は適当なので外部から読み込むときにYOSHINANI
            enemies = new ActorModelBase[4];
            enemySoldierAIs = new SoldierAI[4];
            for (int i = 0; i < enemies.Length; i++)
            {
                // FIXME: いまはsoldierしか作れないけどcaptainとかも作れるように
                var enemy = factory.CreateSoldier(ActorModelBase.ArmyGroupSide.Enemy);
                enemy.ViewBase.transform.localPosition = new Vector3((float)i, 0.0f, 4.0f);
                enemies[i] = enemy;

                enemySoldierAIs[i] = new SoldierAI(enemy, commandRunner, gameMaster);
            }

            // FIXME: 本当は部隊情報から読み込むけど一旦モックで
            enemyCaptainAIs = new CaptainAI[1];
            frinedCaptainAIs = new CaptainAI[0];
            var subSoldiers = new List<ActorModelBase>();
            for (int i = 1; i < enemies.Length; i++)
            {
                subSoldiers.Add(enemies[i]);
            }
            enemyCaptainAIs[0] = new CaptainAI(enemies[0], subSoldiers.ToArray(), commandRunner, gameMaster);

            friends = new ActorModelBase[1];
            friendSoldierAIs = new SoldierAI[1];
            for (int i = 0; i < friends.Length; i++)
            {
                var friend = factory.CreateSoldier(ActorModelBase.ArmyGroupSide.Friend);

                // AI動作確認のため敵から遠くに
                friend.ViewBase.transform.localPosition = new Vector3((float)i, 0.0f, -20.0f);
                friends[i] = friend;

                friendSoldierAIs[i] = new SoldierAI(friend, commandRunner, gameMaster);
            }

            // AI初期化はActorModelを全て生成し終えてから
            foreach (var ai in friendSoldierAIs)
            {
                ai.Initialize();
            }

            foreach (var ai in enemySoldierAIs)
            {
                ai.Initialize();
            }

            // 参照用map構築
            soldierAIMap = new Dictionary<int, SoldierAI>();
            foreach (var soldierAI in enemySoldierAIs)
            {
                soldierAIMap.Add(soldierAI.Owner.Id, soldierAI);
            }

            foreach (var soldierAI in friendSoldierAIs)
            {
                soldierAIMap.Add(soldierAI.Owner.Id, soldierAI);
            }

            captainAIMap = new Dictionary<int, CaptainAI>();
            foreach (var captainAI in enemyCaptainAIs)
            {
                captainAIMap.Add(captainAI.Owner.Id, captainAI);
            }

            foreach (var captainAI in frinedCaptainAIs)
            {
                captainAIMap.Add(captainAI.Owner.Id, captainAI);
            }
        }

        public void Update()
        {
            // AI構築中はこうする そのうちデバッグメニューに移すとかinspectorに逃がすとかしたい
            if (Input.GetKey(KeyCode.F))
            {
                foreach (var ai in friendSoldierAIs)
                {
                    ai.Update();
                }
            }

            foreach (var friend in friends)
            {
                friend.Update();
            }

            if (Input.GetKey(KeyCode.E))
            {
                foreach (var ai in enemyCaptainAIs)
                {
                    ai.Update();
                }

                foreach (var ai in enemySoldierAIs)
                {
                    ai.Update();
                }
            }

            foreach (var enemy in enemies)
            {
                enemy.Update();
            }
        }

        public ActorModelBase[] GetOppositeGroup(ActorModelBase.ArmyGroupSide selfSide)
        {
            if (selfSide == ActorModelBase.ArmyGroupSide.Friend)
            {
                return Enemies;
            }

            return Friends;
        }

        public SoldierAI GetSoldierAI(int actorId)
        {
            if (soldierAIMap.ContainsKey(actorId))
            {
                return soldierAIMap[actorId];
            }

            return null;
        }

        public CaptainAI GetCaptainAI(int actorId)
        {
            if (captainAIMap.ContainsKey(actorId))
            {
                return captainAIMap[actorId];
            }

            return null;
        }
    }
}
