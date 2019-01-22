using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class ConditionChangeData
    {
        // メタデータクラスの作成
        // テキスト形式(tsvとか)からメタの読み込み
        // メタとOperationTypeからNodeを生成する部分
        public enum ChangeType
        {
            // 対象のOperationで達成される
            Achieve,

            // 対象のOperationが終わると未成立になる可能性がある(例えば、移動を行うので向きが変わる、など)
            Cancel,

            // 完全に無関係
            None
        }

        OperationType operationType;
        Dictionary<ConditionType, ChangeType> changeMap;

        public ConditionChangeData()
        {
            changeMap = new Dictionary<ConditionType, ChangeType>();

            foreach (var type in Enum.GetValues(typeof(ConditionType)) as ConditionType[])
            {
                changeMap.Add(type, ChangeType.None);
            }
        }

        public void SetChangeType(ConditionType conditionType, ChangeType changeType)
        {
            changeMap[conditionType] = changeType;
        }
    }
}
