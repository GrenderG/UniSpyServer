﻿using System.Collections.Generic;
using System.Linq;
namespace QueryReport.Entity.Structure.ReportData
{
    public class TeamData
    {
        public List<Dictionary<string, string>> KeyValue { get; protected set; }

        private readonly List<string> StandardTeamKey =
            new List<string> { "team_t", "score_t", "nn_groupid" };

        public TeamData()
        {
            KeyValue = new List<Dictionary<string, string>>();
        }

        public void Update(string teamData)
        {
            KeyValue.Clear();
            int teamCount = System.Convert.ToInt32(teamData[0]);
            teamData = teamData.Substring(1);
            string[] dataPartition = teamData.Split("\0\0", System.StringSplitOptions.RemoveEmptyEntries);
            string[] keys = dataPartition[0].Split("\0").Where(k=>k.Contains("_")).ToArray();
            string[] values = dataPartition[1].Split("\0");

            for (int i = 0; i < teamCount; i++)
            {
                Dictionary<string, string> temp = new Dictionary<string, string>();

                for (int j = 0; j < keys.Length; j++)
                {
                    temp.Add(keys[j], values[i * keys.Length + j]);
                }

                KeyValue.Add(temp);
            }
        }
    }
}
