using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    /// <summary>
    /// 円陣を組む作戦行動
    /// </summary>
    public class OperationCaptainCircle : OperationBase
    {
        GameMaster gameMaster;

        public OperationCaptainCircle(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster) : base(owner, commandRunner)
        {
            this.gameMaster = gameMaster;
        }

        public override void Run()
        {
            // 責任範囲の明確化はしたい
            // 兵同士の連携を密にするなら、攻撃相手とかもここから指示すべき
            // 攻めるとか耐えるとか、大雑把な命令だけ与えて局所性は兵に任せるのもあり
            // 車とか魚鱗みたいに決まった動きが必要なら前者
            // 基本的に後者に寄せて、targetPositionの調整などで連携を実現するほうがよさげ

            // 兵の数から配置を算出
            CaptainAI captain = gameMaster.ActorUpdater.GetCaptainAI(Owner.Id);
            var soldiers = captain.SubSoldiers;

            // ひとまず雑にひとつの円として実装
            // どのくらいの人数から二重にするかは悩みどころ
            float radius = 5.0f;
            var rotation = Quaternion.AngleAxis(360.0f / (float)soldiers.Length, Vector3.up);
            Vector3 direction = Owner.HorizontalDirection;
            for (int i = 0; i < soldiers.Length; i++)
            {
                var soldier = soldiers[i];
                var destination = Owner.Position + direction * radius;
                soldier.SetTargetPosition(destination);

                SoldierAI soldierAI = gameMaster.ActorUpdater.GetSoldierAI(soldier.Id);
                soldierAI.Planning.SetGoal(new Condition(new ConditionType[] { ConditionType.StayNearTargetPoint }));

                direction = rotation * direction;
            }

            // 自分自身は今の場所に留まる 場合によっては動きながらも考えるけど、今は留まったままで
            var selfAI = gameMaster.ActorUpdater.GetSoldierAI(Owner.Id);
            selfAI.Planning.SetGoal(new Condition(new ConditionType[] { ConditionType.StayNearTargetPoint }));
            Owner.SetTargetPosition(Owner.Position);

            // 各兵へのリアルタイムな指示はどうすべきだろう
            // 円陣の一部として処理されるよう上手く設計する
            // 決してtrueにならないようなCondition(円陣を維持する)を設定し、それをゴールとしたNodeを組む これは別operationとして実装
            // 他にも円陣のまま動くとかが考えられる
            // 中々上手くいかないことでRebuildが走り、その都度適切な陣形を取ることも考えられる
        }

    }
}
