using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class PlanningNodeFactoryCaptain : IPlanningNodeFactory
    {
        ActorModelBase owner;
        CommandRunner commandRunner;
        GameMaster gameMaster;

        public PlanningNodeFactoryCaptain(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster)
        {
            this.owner = owner;
            this.commandRunner = commandRunner;
            this.gameMaster = gameMaster;
        }

        // FIXME: 本当は外部ファイルから読み込むべき
        // 最終的には敵タイプとか味方ユニットごとに外部データから読み込み
        // 味方の方はユーザーがいじれるようにもしたい
        public PlanningNode[] Create()
        {
            List<PlanningNode> newNodeList = new List<PlanningNode>();

            // 円陣を組んでいるというConditionを用意
            // 無駄な更新したくないので、Conditionはsoldier用captain用みたいに分類したい
            // enum定義としては同一にする
            // IConditionUpdaterを作って対応したものにPolymorphicに割り当てるのがよい？
            // ぶっちゃけリストさえあれば要件は満たせているのだが、「どの兵種にどのConditionが対応しているのか」という情報を生成側が握るのはなんかきもい
            // 生成側はsoldierを作るんだからsoldier用って指定しとけばいいんでしょ……くらいにしか思わないはず
            // Conditionの中にUpdateType定義を切るのもまあありだけど、UpdateしないConditionも存在するからなー
            // って考えるとConditionからはUpdate機能を省いて、状態更新機能だけ公開したうえで、Updaterをplanning側に持たせてやるのが正着では？
            
            kokokara
            // というわけでConditionの更新機能をUpdaterに逃がすところから開始
            // それができたら、円陣を組んでいるというConditionを作成して、Captain用のUpdaterから更新する
            // そこまでできたらCaptainを実際に生成できる気がする？

            // 円陣を組む
            {
                var beforeList = new ConditionType[]
                {
                };
                var afterList = new ConditionType[]
                {
                    ConditionType.LookToSomeEnemy,
                };
                var node = new PlanningNode(owner, commandRunner, gameMaster, new Condition(beforeList), new Condition(afterList), OperationType.LookToNearestEnemy);
                newNodeList.Add(node);
            }

            //// 最も近くにいる敵対陣営のキャラを攻撃しようとする
            //{
            //    var beforeList = new ConditionType[]
            //    {
            //        ConditionType.CanTargetSomeEnemy,
            //    };
            //    var afterList = new ConditionType[]
            //    {
            //        ConditionType.HittingSomeEnemy,
            //    };
            //    var node = new PlanningNode(owner, commandRunner, gameMaster, new Condition(beforeList), new Condition(afterList), OperationType.AttackNearestEnemy);
            //    newNodeList.Add(node);
            //}

            //// 攻撃相手が見つかるまで索敵
            //{
            //    var beforeList = new ConditionType[]
            //    {
            //    };
            //    var afterList = new ConditionType[]
            //    {
            //        ConditionType.CanTargetSomeEnemy,
            //    };
            //    var node = new PlanningNode(owner, commandRunner, gameMaster, new Condition(beforeList), new Condition(afterList), OperationType.SearchEnemy);
            //    newNodeList.Add(node);
            //}

            return newNodeList.ToArray();
        }
    }
}
