using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class CommandModelActorRotate : CommandModelBase
    {
        ActorModelBase targetActor;
        bool right;

        public CommandModelActorRotate(ActorModelBase targetActor, bool right)
            : base()
        {
            this.targetActor = targetActor;
            this.right = right;
        }

        /// <summary>
        /// Updateの中で一回ずつ呼ぶ
        /// 1回で完了するコマンドの場合はHasFinishedで即時trueを返す
        /// </summary>
        public override void Run()
        {
            targetActor.Rotate(right);
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
