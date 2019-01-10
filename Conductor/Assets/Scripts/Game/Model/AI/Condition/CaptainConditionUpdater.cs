using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class CaptainConditionUpdater : IConditionUpdater
    {
        ActorModelBase owner;
        GameMaster gameMaster;

        public CaptainConditionUpdater(ActorModelBase owner, GameMaster gameMaster)
        {
            this.owner = owner;
            this.gameMaster = gameMaster;
        }

        // FIXME: 共有範囲で更新ロジックを区切ったほうがいい 例えば戦場全体で共通な状態は1フレームに1回更新すればよい
        public void Update(Condition target)
        {
            UpdateCompleteCircleFormationCondition(target);
        }


        #region 状態更新メソッドたち
        // 円陣を組んでいる
        void UpdateCompleteCircleFormationCondition(Condition target)
        {
            // FIXME: すべての部下に対してNearTargetPositionを尋ねればよろしい
            kokokara

            target.SetFlag(ConditionType.CaptainCompleteCircleFormation, false);
        }
        
        #endregion
    }
}
