using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    // FIXME: OperationFactoryかOperationBaseに置く
    public enum OperationType
    {
        // 特定地点へ移動する FIXME: この命令いけてない もうちょっと具体性が必要 「最も近くにいる敵に攻撃できる位置まで移動する」とか
        MoveToTargetPoint,

        // 最も近くにいる敵対陣営のキャラのほうを向く
        LookToNearestEnemy,

        // 最も近くにいる敵対陣営のキャラを攻撃しようとする
        AttackNearestEnemy,

        // ターゲット取れる敵が見つかるまで索敵
        SearchEnemy,
    }

    public class OperationFactory
    {
        ActorModelBase owner;
        CommandRunner commandRunner;
        GameMaster gameMaster;

        public OperationFactory(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster)
        {
            this.owner = owner;
            this.commandRunner = commandRunner;
            this.gameMaster = gameMaster;
        }

        public OperationBase Create(OperationType type)
        {
            switch (type)
            {
                case OperationType.MoveToTargetPoint:
                    // FIXME: targetPointは命令を受けたownerが持っているはずなのでownerから受け取る
                    return new OperationMoveToTargetPoint(owner, commandRunner, Vector3.zero);
                case OperationType.LookToNearestEnemy:
                    return new OperationLookToNearestEnemy(owner, commandRunner, gameMaster);
                case OperationType.AttackNearestEnemy:
                    return new OperationAttackNearestEnemy(owner, commandRunner, gameMaster);
                case OperationType.SearchEnemy:
                    return new OperationSearchEnemy(owner, commandRunner, gameMaster);
                default:
                    break;
            }
            return null;
        }
    }
}
