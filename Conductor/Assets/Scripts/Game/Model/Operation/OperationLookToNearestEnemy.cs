using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class OperationLookToNearestEnemy : OperationBase
    {
        // 比較用イプシロン ちょっと広めに
        static readonly float Epsilon = 0.1f;

        // FIXME: 本当はナビゲーションのみ受け取る
        GameMaster gameMaster;

        public OperationLookToNearestEnemy(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster) : base(owner, commandRunner)
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
                var toEnemy = enemy.ViewBase.transform.localPosition - Owner.ViewBase.transform.localPosition;
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

            var toTarget = target.ViewBase.transform.localPosition - Owner.ViewBase.transform.localPosition;
            toTarget.y = 0.0f;

            if (toTarget.sqrMagnitude < Epsilon * Epsilon)
            {
                return;
            }

            toTarget.Normalize();
            float cross = Vector3.Cross(Owner.HorizontalDirection, toTarget).y;
            if (Mathf.Abs(cross) < Epsilon)
            {
                return;
            }

            bool right = cross > 0.0f;

            var rotateCommand = new CommandModelActorRotate(Owner, right);
            CommandRunner.Schedule(rotateCommand);
        }
    }
}
