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
            // OperationはすでにあるのでNodeを増築 ok
        // 必要ならConditionも拡張 ok
        // Nodeを実際に生成 ok
        // 設計上問題がなければsoldierってついてるクラスをリネーム(のちに分けるとしたら継承でpolymorphicに)
        // 実際に部隊長を置いてみる ok
        // Nodeの自動生成に着手 kokokara
            // 自動生成するコードでも作ってみるかー
            // Nodeの作成部分のコードを自動生成 ok
            // パターンとかメタ情報は下の通り
            // メタ情報の管理方法どうするかなー
            // 各operationに対して、全ConditionTypeを分類できればそれでよし
            // ScriptableObjectとかでもいける気がしないでもない
            // だるくなったらtsvとか外部ツールに逃がせばいいような気もする kokokara
            // 自動生成ロジックに関してはテストを書いて保守したい気分
        // 試しにtargetPositionガン守りと攻撃の部隊を作ってみる
            // 守りは待ち作戦を作る必要あり
            // 攻撃も「TargetActorを攻撃」とか「対象の部隊を攻撃」とかが必要かな
            // ActorにTargetXXXを数種類定義する必要はありそう
            // Nodeの生成部分、ぶっちゃけ普通に全パターン書くとだるすぎるので、外部データから自動生成したい
                // あるOperationに関して、最低限の前提条件群、クリア後の最低保証状態群を定義
                // before, after両方に含まれている→前提であり、状態がキープされる→true->trueのみ存在
                // beforeのみに含まれている→前提ではあるがその後の状態は保証しない→true->falseのみ存在（更新時に新しく出現する条件 これがあると千日手が発生しうる？）
                // afterのみに含まれている→前提ですらないが達成はされる→false->trueのみ存在
                // どっちにもふくまれていない→無関係→true->trueとfalse->falseが存在(これが前提に入ってしまうこともあるが、つまり「この条件を満たした状態でこのoperationを完遂する」というNodeになる）
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

        public ActorModelBase Owner { get { return owner; } }

        public ActorModelBase[] SubSoldiers
        {
            get { return subSoldiers; }
        }

        public CaptainAI(ActorModelBase owner, ActorModelBase[] subSoldiers, CommandRunner commandRunner, GameMaster gameMaster)
        {
            this.owner = owner;
            this.subSoldiers = subSoldiers;

            var nodeFactory = new PlanningNodeFactoryCaptain(owner, commandRunner, gameMaster);
            var conditionUpdater = new SoldierConditionUpdater(owner, gameMaster);
            planning = new SoldierPlanning(owner, commandRunner, gameMaster, nodeFactory, conditionUpdater);

            // FIXME: 本当は将軍とかプレイヤーから指示を貰う
            planning.SetGoal(new Condition(new [] { (int)CaptainConditionType.CaptainCompleteCircleFormation }));
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
