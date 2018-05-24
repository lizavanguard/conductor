using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Consuctor.Game.Model
{
    public abstract class CommandModelBase
    {
        public CommandModelBase()
        { }

        /// <summary>
        /// Updateの中で一回ずつ呼ぶ
        /// 1回で完了するコマンドの場合はHasFinishedで即時trueを返す
        /// </summary>
        public virtual void Run()
        {
        }

        /// <summary>
        /// Runを呼んだ直後にこの関数で終了判定を取り、終了していたらコマンドを破棄する
        /// </summary>
        /// <returns></returns>
        public virtual bool HasFinished()
        {
            return true;
        }

        /// <summary>
        /// このコマンドを実行した際にView側で叩くべきコマンド群を生成する
        /// </summary>
        /// <returns></returns>
        public virtual View.CommandViewBase[] GenerateCommandView()
        {
            return null;
        }
    }
}
