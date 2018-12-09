using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    // FIXME: なんとかこれをデータ化したい
    // planningChainを作るときはafterとbeforeで変化のあったbitに着目して次の状況を作る
    class PlannngNode
    {
        // 前提条件
        Condition beforeCondition;

        // 目標条件
        Condition afterCondition;

        // 行動
        OperationType operationType;

        public Condition BeforeCondition { get { return beforeCondition; } }
        public Condition AfterCondition { get { return afterCondition; } }
        public OperationType OperationType { get { return operationType; } }


        public PlannngNode(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster, Condition before, Condition after, OperationType operationType)
        {
            this.beforeCondition = before;
            this.afterCondition = after;
            this.operationType = operationType;
        }
    }
}
