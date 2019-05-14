using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public struct ConditionChangeData
    {
        // メタデータクラスの作成 ok
        // メタとOperationTypeからNodeを生成する部分 factoryを書き換える感じで
        // テキスト形式(tsvとか)からメタの読み込み
        public ConditionChangeData(OperationType operationType)
        {
            this.operationType = operationType;
            preconditions = new List<int>();
            postconditions = new List<int>();
        }

        public void AddPrecondition(int condition)
        {
            if (preconditions.Contains(condition))
            {
                return;
            }

            preconditions.Add(condition);
        }

        public void AddPostcondition(int condition)
        {
            if (postconditions.Contains(condition))
            {
                return;
            }

            postconditions.Add(condition);
        }

        // 最低必要条件群 少なくともこれだけはなくてはoperationを開始できない
        List<int> preconditions;

        // 最低達成条件群 operationを達成したら最低でもこれだけは満たされる
        List<int> postconditions;

        OperationType operationType;

        public int[] Preconditions { get { return preconditions.ToArray(); } }

        public int[] Postconditions { get { return postconditions.ToArray(); } }

        public OperationType OperationType { get { return operationType; } }
    }
}
