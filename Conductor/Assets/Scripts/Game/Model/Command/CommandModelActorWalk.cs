using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class CommandModelActorWalk : CommandModelBase
    {
        ActorModelBase targetActor;
        bool front;

        public CommandModelActorWalk(ActorModelBase targetActor, bool front)
            : base()
        {
            this.targetActor = targetActor;
            this.front = front;
        }

        /// <summary>
        /// Updateの中で一回ずつ呼ぶ
        /// 1回で完了するコマンドの場合はHasFinishedで即時trueを返す
        /// </summary>
        public override void Run()
        {
            targetActor.Walk(front);
        }

        /// <summary>
        /// Runを呼んだ直後にこの関数で終了判定を取り、終了していたらコマンドを破棄する
        /// </summary>
        /// <returns></returns>
        public override bool HasFinished()
        {
            return true;
        }
    }
}
