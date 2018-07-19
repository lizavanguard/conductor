using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class CommandModelActorRotate : CommandModelBase
    {
        int targetActorId;
        bool right;

        public CommandModelActorRotate(MessageBus.Dispatcher messageBusManager, int targetActorId, bool right)
            : base(messageBusManager)
        {
            this.targetActorId = targetActorId;
            this.right = right;
        }

        /// <summary>
        /// Updateの中で一回ずつ呼ぶ
        /// 1回で完了するコマンドの場合はHasFinishedで即時trueを返す
        /// </summary>
        public override void Run()
        {
            // 対象の相手に歩くメッセージを飛ばす
            MessageBus.ActorRotateBus.SendMessage(
                address: targetActorId,
                message: new MessageBus.ActorRotateBus.Message(right)
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
    }
}
