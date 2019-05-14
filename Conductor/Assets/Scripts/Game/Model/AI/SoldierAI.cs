using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class SoldierAI
    {
        ActorModelBase owner;

        // navigation

        // planning
        SoldierPlanning planning;

        public ActorModelBase Owner { get { return owner; } }
        public SoldierPlanning Planning { get { return planning; } }
        public OperationBase GetCurrentOperation { get { return planning.CurrentOperation; } }

        public SoldierAI(ActorModelBase owner, CommandRunner commandRunner, GameMaster gameMaster)
        {
            this.owner = owner;
            var nodeFactory = new PlanningNodeFactorySoldiler(owner, commandRunner, gameMaster);
            var conditionUpdater = new SoldierConditionUpdater(owner, gameMaster);
            planning = new SoldierPlanning(owner, commandRunner, gameMaster, nodeFactory, conditionUpdater);

            // FIXME: 本当は部隊長クラスからゴールを教えてもらうはず
            planning.SetGoal(new Condition(new [] { (int)SoldierConditionType.HittingSomeEnemy }));
        }

        public void Initialize()
        {
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
