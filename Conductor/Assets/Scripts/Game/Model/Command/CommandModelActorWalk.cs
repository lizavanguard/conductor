using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public abstract class CommandModelActorWalk : CommandModelBase
    {
        int targetActorId;

        public CommandModelActorWalk(MessageBus.Manager messageBusManager, int targetActorId)
            : base(messageBusManager)
        {
            this.targetActorId = targetActorId;
        }

        /// <summary>
        /// Updateの中で一回ずつ呼ぶ
        /// 1回で完了するコマンドの場合はHasFinishedで即時trueを返す
        /// </summary>
        public override void Run()
        {
            // 対象の相手に歩くメッセージを飛ばす
            MessageBusManager.ActorWalkBus.SendMessage(
                address: targetActorId,
                message: new MessageBus.ActorWalkBus.Message(true)
                );
        }

        /// <summary>
        /// Runを呼んだ直後にこの関数で終了判定を取り、終了していたらコマンドを破棄する
        /// </summary>
        /// <returns></returns>
        public override bool HasFinished()
        {
            return true;
        }

        /// <summary>
        /// このコマンドを実行した際にView側で叩くべきコマンド群を生成する
        /// Model経由で叩かれるものは除く
        /// UI演出などが対象
        /// </summary>
        /// <returns></returns>
        public override View.CommandViewBase[] GenerateCommandView()
        {
            return null;
        }
    }
}
