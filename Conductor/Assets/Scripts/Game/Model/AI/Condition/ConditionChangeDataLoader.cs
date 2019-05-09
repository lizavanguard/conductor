using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Conductor.Game.Model
{
    public class ConditionChangeDataLoader
    {
        static readonly string SoldierPreconditionPath = Application.dataPath + "/Resources/Masterdata/SoldierPreconditions.tsv";
        static readonly string SoldierPostconditionPath = Application.dataPath + "/Resources/Masterdata/SoldierPostconditions.tsv";
        static readonly string CaptainPreconditionPath = Application.dataPath + "/Resources/Masterdata/CaptainPreconditions.tsv";
        static readonly string CaptainPostconditionPath = Application.dataPath + "/Resources/Masterdata/CaptainPostconditions.tsv";

        Dictionary<OperationType, ConditionChangeData> changeDataMap;

        string preconditionPath;
        string postconditionPath;

        public static ConditionChangeDataLoader CreateSoldierInstance()
        {
            return new ConditionChangeDataLoader(SoldierPreconditionPath, SoldierPostconditionPath);
        }

        public static ConditionChangeDataLoader CreateCaptainInstance()
        {
            return new ConditionChangeDataLoader(CaptainPreconditionPath, CaptainPostconditionPath);
        }

        ConditionChangeDataLoader(string prePath, string postPath)
        {
            preconditionPath = prePath;
            postconditionPath = postPath;
        }

        public void Load()
        {
            changeDataMap = new Dictionary<OperationType, ConditionChangeData>();

            LoadPreconditions();
            LoadPostconditions();
        }

        void LoadPreconditions()
        {
            StreamReader reader = new StreamReader(preconditionPath, System.Text.Encoding.UTF8);

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
            StreamReader reader = new StreamReader(postconditionPath, System.Text.Encoding.UTF8);

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
