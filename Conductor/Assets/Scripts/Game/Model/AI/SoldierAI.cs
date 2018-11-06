using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class SoldierAI
    {
        public ActorModelBase owner;

        // navigation

        // planning
        SoldierPlanning planning;

        public OperationBase GetCurrentOperation { get { return planning.CurrentOperation; } }

        public SoldierAI(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster)
        {
            this.owner = owner;
            planning = new SoldierPlanning(owner, commandRunner, gameMaster);

            // FIXME: 本当は部隊長クラスからゴールを教えてもらうはず
            planning.SetGoal(new Condition(new ConditionType[] { ConditionType.HittingSomeEnemy }));
        }

        // 最初のChain構築
        public void Initialize()
        {
            planning.BuildPllaningChain();
        }

        public void Update()
        {
            planning.UpdateCurrentCondition();
            planning.UpdatePlanning();

            if (planning.CurrentOperation != null)
            {
                planning.CurrentOperation.Run();
            }

            // ゴール未達成かつchainがないならリビルド
            if (!planning.GoalIsSatisfied() && !planning.HasPlanningChain())
            {
                planning.BuildPllaningChain();
            }
        }

    }
}
