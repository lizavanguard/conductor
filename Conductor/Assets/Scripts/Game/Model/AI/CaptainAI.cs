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
        // Nose生成メソッドを拡張してcaptain用を作る
        // Operationも拡張
        // 実際のOperationを書く
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

        public CaptainAI(ActorModelBase owner, ActorModelBase[] subSoldiers, CommandRunner commandRunner, GameMaster gameMaster)
        {
            this.owner = owner;
            this.subSoldiers = subSoldiers;

            planning = new SoldierPlanning(owner, commandRunner, gameMaster);

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
