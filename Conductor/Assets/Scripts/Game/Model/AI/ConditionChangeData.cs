using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public struct OperationMetaData
    {
        // メタデータクラスの作成 ok
        // メタとOperationTypeからNodeを生成する部分 factoryを書き換える感じで
        // テキスト形式(tsvとか)からメタの読み込み
        

        // 最低必要条件群 少なくともこれだけはなくてはoperationを開始できない
        ConditionType[] preconditions;

        // 最低達成条件群 operationを達成したら最低でもこれだけは満たされる
        ConditionType[] postconditions;

        OperationType operationType;

        public ConditionType[] Preconditions { get { return preconditions; } }

        public ConditionType[] Postconditions { get { return postconditions; } }

        public OperationType OperationType { get { return operationType; } }
    }
}
