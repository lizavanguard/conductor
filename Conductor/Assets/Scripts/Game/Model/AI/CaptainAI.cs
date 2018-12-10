using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    /// <summary>
    /// 部隊長としての振る舞いを記述するクラス
    /// </summary>
    public class CaptainAI
    {
        // planningを持たせる ok
        // Nose生成メソッドを拡張してcaptain用を作る ok
        // Operationも拡張 ok
        // 実際のOperationを書く
            // gameMasterに各種AI取得メソッドを書く
            // Nodeに立つフラグリストとは別に折れるフラグリストも作る(必ずしも折れなくても、多分折れるというものは入れておく)
            // SoldierAIにTargetPositionを設定
            // 「TargetPositionの近くにいる」のCondition作成
            // OperationはすでにあるのでNodeに適用
        // 必要ならConditionも拡張
        // Nodeを実際に生成
        // 設計上問題がなければsoldierってついてるクラスをリネーム(のちに分けるとしたら継承でpolymorphicに)
        // 実際に部隊長を置いてみる
        // 試しにtargetPositionガン守りと牽制の部隊を作ってみる
        // 味方側にも部隊を作ってみる
        // 操作周りができていたらサンプルステージを作ってみたい
        // 将軍作成に移行

        ActorModelBase owner;

        /// <summary>
        /// 命令下にある兵士
        /// 自分自身を除く
        /// </summary>
        ActorModelBase[] subSoldiers;

        SoldierPlanning planning;

        public ActorModelBase[] SubSoldiers
        {
            get { return subSoldiers; }
        }

        public CaptainAI(ActorModelBase owner, ActorModelBase[] subSoldiers, CommandRunner commandRunner, GameMaster gameMaster)
        {
            this.owner = owner;
            this.subSoldiers = subSoldiers;

            var nodeFactory = new PlanningNodeFactoryCaptain(owner, commandRunner, gameMaster);
            planning = new SoldierPlanning(owner, commandRunner, gameMaster, nodeFactory);

            // FIXME: 本当は将軍とかプレイヤーから指示を貰う モックも部隊長用のconditionに差し替え
            planning.SetGoal(new Condition(new ConditionType[] { ConditionType.HittingSomeEnemy }));
        }

        public void Update()
        {
            planning.UpdateCurrentCondition();
            planning.UpdatePlanning();

            if (planning.CurrentOperation != null)
            {
                planning.CurrentOperation.Run();
            }

            // ゴール未達成かつchainがないならリビルド
            if (!planning.GoalIsSatisfied() && !planning.HasPlanningChain())
            {
                planning.BuildPllaningChain();
            }
        }
    }
}
