using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
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
            // 円陣程度なら後者でよさそう？

            // 兵の数から配置を算出
        }

    }
}
