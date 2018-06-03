using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game
{
    /// <summary>
    /// コマンドを受け付けて実行する
    /// </summary>
    public class CommandRunner
    {
        List<View.CommandViewBase> scheduledCommandView;
        List<Model.CommandModelBase> scheduledCommandModel;

        List<View.CommandViewBase> runningCommandView;
        List<Model.CommandModelBase> runningCommandModel;

        public CommandRunner() { }

        public void Schedule(View.CommandViewBase commandView)
        {
            scheduledCommandView.Add(commandView);
        }

        public void Schedule(Model.CommandModelBase commandModel)
        {
            scheduledCommandModel.Add(commandModel);
        }

        public void Update()
        {
            runningCommandView.AddRange(scheduledCommandView);
            runningCommandModel.AddRange(scheduledCommandModel);

            scheduledCommandView.Clear();
            scheduledCommandModel.Clear();

            foreach (var command in runningCommandView)
            {
                command.Run();
            }

            foreach (var command in runningCommandModel)
            {
                command.Run();
            }

            runningCommandView.RemoveAll(command => command.HasFinished());
            runningCommandModel.RemoveAll(command => command.HasFinished());
        }
    }
}
