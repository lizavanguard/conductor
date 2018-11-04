using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class OperationAttackNearestEnemy : OperationBase
    {
        // 比較用イプシロン 攻撃を行うための基準なので、極小でなくてよい
        const float DistanceThreshold = 1.0f;

        // FIXME: 本当はナビゲーションのみ受け取る
        GameMaster gameMaster;

        public OperationAttackNearestEnemy(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster) : base(owner, commandRunner)
        {
            this.gameMaster = gameMaster;
        }

        public override void Run()
        {
            // FIXME: 一番よさそうな方向をナビゲーションが示すように修正
            ActorModelBase target = null;
            var enemies = gameMaster.MockEnemies;
            float minSq = float.MaxValue;
            foreach (var enemy in enemies)
            {
                var toEnemy = enemy.Position - Owner.Position;
                if (minSq > toEnemy.sqrMagnitude)
                {
                    minSq = toEnemy.sqrMagnitude;
                    target = enemy;
                }
            }

            if (target == null)
            {
                return;
            }

            var toTarget = target.Position - Owner.Position;
            toTarget.y = 0.0f;
            float distance = toTarget.magnitude;
            toTarget.Normalize();
            float cross = Vector3.Cross(Owner.HorizontalDirection, toTarget).y;
            bool rotate = Mathf.Abs(cross) > Constant.ActorAngleCrossEpsilon;
            if (rotate)
            {
                bool right = cross > 0.0f;
                var rotateCommand = new CommandModelActorRotate(Owner, right);
                CommandRunner.Schedule(rotateCommand);
            }

            // 向きの一致度と距離に応じて全身後退を判別
            // FIXME: 雑なので調整
            bool move = Vector3.Dot(toTarget, Owner.HorizontalDirection) > 0.5f
                && distance > DistanceThreshold;
            if (move)
            {
                var walkCommand = new CommandModelActorWalk(Owner, true);
                CommandRunner.Schedule(walkCommand);
            }
            else if (!rotate)
            {
                var attackCommand = new CommandModelActorAttack(Owner);
                CommandRunner.Schedule(attackCommand);
            }
        }
    }
}
