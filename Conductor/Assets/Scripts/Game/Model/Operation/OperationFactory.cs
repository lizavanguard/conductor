using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    // FIXME: OperationFactoryかOperationBaseに置く
    public enum OperationType
    {
        // 特定地点へ移動する
        MoveToTargetPoint,

        // 最も近くにいる敵対陣営のキャラのほうを向く
        LookToNearestEnemy,

        // 最も近くにいる敵対陣営のキャラを攻撃しようとする
        AttackNearestEnemy,
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
                    break;
                case OperationType.LookToNearestEnemy:
                    return new OperationLookToNearestEnemy(owner, commandRunner, gameMaster);
                    break;
                case OperationType.AttackNearestEnemy:
                    return new OperationAttackNearestEnemy(owner, commandRunner, gameMaster);
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}
