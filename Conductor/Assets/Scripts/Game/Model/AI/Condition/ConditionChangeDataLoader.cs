using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class ConditionChangeDataLoader
    {
        static readonly string PreconditionPath = Application.dataPath + "/Resources/Masterdata/Preconditions.tsv";
        static readonly string PostconditionPath = Application.dataPath + "/Resources/Masterdata/Postconditions.tsv";

        Dictionary<OperationType, ConditionChangeData> changeDataMap;

        public ConditionChangeDataLoader()
        {
        }

        public void Load()
        {
            changeDataMap = new Dictionary<OperationType, ConditionChangeData>();

            LoadPreconditions();
            LoadPostconditions();
        }

        void LoadPreconditions()
        {
            StreamReader reader = new StreamReader(PreconditionPath, System.Text.Encoding.UTF8);

            // remove header
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var args = line.Split('\t');
                var operation = (OperationType)Enum.Parse(typeof(OperationType), args[0]);
                var condition = (ConditionType)Enum.Parse(typeof(ConditionType), args[1]);

                // いなかったら追加
                if (!changeDataMap.ContainsKey(operation))
                {
                    changeDataMap.Add(operation, new ConditionChangeData(operation));
                }

                var changeData = changeDataMap[operation];
                changeData.AddPrecondition(condition);
            }
        }

        void LoadPostconditions()
        {
            StreamReader reader = new StreamReader(PostconditionPath, System.Text.Encoding.UTF8);

            // remove header
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var args = line.Split('\t');
                var operation = (OperationType)Enum.Parse(typeof(OperationType), args[0]);
                var condition = (ConditionType)Enum.Parse(typeof(ConditionType), args[1]);

                // いなかったら追加
                if (!changeDataMap.ContainsKey(operation))
                {
                    changeDataMap.Add(operation, new ConditionChangeData(operation));
                }

                var changeData = changeDataMap[operation];
                changeData.AddPostcondition(condition);
            }
        }

        public Dictionary<OperationType, ConditionChangeData> GetChangeDataMap()
        {
            return changeDataMap;
        }
    }
}
