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
            // gameMasterに各種AI取得メソッドを書く ok
            // SoldierAIにTargetPositionを設定 ok
            // 「TargetPositionの近くにいる」のCondition作成 ok
            // OperationはすでにあるのでNodeを増築
        // 必要ならConditionも拡張
        // Nodeを実際に生成
        // 設計上問題がなければsoldierってついてるクラスをリネーム(のちに分けるとしたら継承でpolymorphicに)
        // 実際に部隊長を置いてみる
        // 試しにtargetPositionガン守りと攻撃の部隊を作ってみる
            // 守りは待ち作戦を作る必要あり
            // 攻撃も「TargetActorを攻撃」とか「対象の部隊を攻撃」とかが必要かな
            // ActorにTargetXXXを数種類定義する必要はありそう
            // Nodeの生成部分、ぶっちゃけ普通に全パターン書くとだるすぎるので、外部データから自動生成したい
                // あるOperationに関して、1. Conditionをfalse->trueと変化する、2. 無関係、3. 終わった後にtrueとは限らない(処理場は「必ずfalseになる」という扱いで進める)の三種類に分類
                // 1はfalse->true、3はfalse->falseとした上で、2に関して全網羅
                // 例えばConditionがA～Dの四種類で、あるOperationに関してAは1、BCは2、Dが3に分類される場合、四個のNodeができあがる
                // 個数はかなり増えるが、Afterを使って検索する関係上、AfterのConditionを使って二分探索できるように並べ替えておけば検索は速くなるはず？
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
