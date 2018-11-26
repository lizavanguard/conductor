using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class OperationSearchEnemy : OperationBase
    {
        // 比較用イプシロン 攻撃を行うための基準なので、極小でなくてよい
        const float DistanceThreshold = 1.0f;

        // FIXME: 本当はナビゲーションのみ受け取る
        GameMaster gameMaster;

        // 索敵のための次の移動先
        Vector3 nextTargetPosition;

        public OperationSearchEnemy(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster) : base(owner, commandRunner)
        {
            this.gameMaster = gameMaster;
            nextTargetPosition = owner.Position;
        }

        public override void Run()
        {
            // FIXME: 一番よさそうな方向をナビゲーションが示すように修正

            var enemies = gameMaster.Enemies;
            float minSq = float.MaxValue;
            foreach (var enemy in enemies)
            {
                var toEnemy = enemy.Position - Owner.Position;

                // 十分近くに敵がいれば索敵完了 まだ続けるかどうかはまあ適当に
                if (toEnemy.sqrMagnitude < Constant.SoldierTargettingDistanceSqThreshold)
                {
                    return;
                }
            }

            var toTarget = nextTargetPosition - Owner.Position;
            toTarget.y = 0.0f;

            if (toTarget.sqrMagnitude < Constant.ActorPositionDistanceSqEpsilon)
            {
                // 次の移動先のランダマイズ 前方向を基準に左右90度から
                float angle = Random.Range(-90.0f, 90.0f);
                var rotation = Quaternion.AngleAxis(angle, Vector3.up);
                var direction = rotation * Owner.HorizontalDirection;

                // 雑に3mずつチェック
                nextTargetPosition = Owner.Position + direction * 3.0f;

                Debug.Log(nextTargetPosition.ToString());
            }

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
            bool move = Vector3.Dot(toTarget, Owner.HorizontalDirection) > 0.5f && distance > Constant.ActorPositionDistanceEpsilon;
            if (move)
            {
                var walkCommand = new CommandModelActorWalk(Owner, true);
                CommandRunner.Schedule(walkCommand);
            }
        }
    }
}
