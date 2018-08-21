using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AngryRabbit
{
    /// <summary>
    /// 玩家任务线:等级任务 配置文件加载
    /// </summary>
    public class PlayerLevelTaskLineConfig
    {
        private static PlayerLevelTaskLineConfig instance;
        public static PlayerLevelTaskLineConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerLevelTaskLineConfig();
                }

                return instance;
            }
        }

        Dictionary<int, PlayerLevelTaskLineData> dataDic;
        public void loadData(List<PlayerLevelTaskLineItem> p_list)
        {
            if (dataDic == null) dataDic = new Dictionary<int, PlayerLevelTaskLineData>();
            dataDic.Clear();
            if (p_list == null)
            {
                Debug.LogError("输入列表为空！");
                return;
            }

            foreach (var v in p_list)
            {
                dataDic.Add(v.taskId, new PlayerLevelTaskLineData(v));
            }

            foreach (var data in dataDic)
            {
                Debug.Log(data.Value.TaskID + " : " + data.Value.taskTitle + " : " + data.Value.taskDesc);
            }
        }

        public PlayerLevelTaskLineData GetDataByID(int id)
        {
            if (dataDic.ContainsKey(id))
                return dataDic[id];
            else
                Debug.LogError("not find such data which id is : " + id);

            return null;
        }
    }

    /// <summary>
    /// 玩家等级数据
    /// </summary>
    public class PlayerLevelTaskLineData : ConfigData
    {  
        public PlayerLevelTaskLineData(PlayerLevelTaskLineItem data)
        {
            this.TaskID = data.taskId;
            this.taskEntityType = (TaskEntityType)data.taskEntityType;
            this.taskType = (TaskType)data.taskType;
            this.nextTaskId = data.nextTaskId;
            this.taskTitle = data.taskTitle;
            this.taskDesc = data.taskDesc;
            this.requirementList = new List<TaskRequirementItem>();
            this.requirementList = GetRequirementList(data.taskRequirements);
            this.taskCombineType = (TaskCombineType)data.taskCombineType;
            this.rewardList = new List<TaskRewardItem>();
            this.rewardList = GetRewardList(data.taskRewards);
            this.rewardGetType = (TaskCombineType)data.taskRewardsCombineType;

            SaveConfig(this.TaskID, this);
        }

        private List<TaskRequirementItem> GetRequirementList(string data)
        {
            List<TaskRequirementItem> requirementsList = new List<TaskRequirementItem>();

            string[] split = data.Split(',');
            for (int i = 0; i < split.Length; i++)
            {
                string[] info = split[i].Split('-');
                TaskRequirementItem item = new TaskRequirementItem(GetRequirementType(info[0]), (TaskRecordType)int.Parse(info[2]), int.Parse(info[1]));
                requirementsList.Add(item);
            }

            return requirementsList;
        }

        private List<TaskRewardItem> GetRewardList(string data)
        {
            List<TaskRewardItem> rewardList = new List<TaskRewardItem>();

            string[] split = data.Split(',');
            for (int i = 0; i < split.Length; i++)
            {
                string[] info = split[i].Split('-');
                TaskRewardItem item = new TaskRewardItem(GetRewardType(info[0]), int.Parse(info[1]));
                rewardList.Add(item);
            }

            return rewardList;
        }

        public TaskRequirementType GetRequirementType(string data)
        {
            TaskRequirementType type;
            switch (data)
            {
                case "gold":
                    type = TaskRequirementType.Gold;
                    break;
                case "level":
                    type = TaskRequirementType.Level;
                    break;
                default:
                    type = TaskRequirementType.None;
                    break;
            }
            return type;
        }

        public TaskRewardType GetRewardType(string data)
        {
            TaskRewardType type;
            switch (data)
            {
                case "gold":
                    type = TaskRewardType.Gold;
                    break;
                case "exp":
                    type = TaskRewardType.Exp;
                    break;
                default:
                    type = TaskRewardType.None;
                    break;
            }
            return type;
        }
    }

    public struct PlayerLevelTaskLineItem
    {
        [XmlAttribute("TaskId")]
        public int taskId;
        [XmlAttribute("TaskEntityType")]
        public int taskEntityType;
        [XmlAttribute("TaskType")]
        public int taskType;
        [XmlAttribute("NextTaskId")]
        public int nextTaskId;
        [XmlAttribute("TaskTitle")]
        public string taskTitle;
        [XmlAttribute("TaskDesc")]
        public string taskDesc;
        [XmlAttribute("TaskRequirements")]
        public string taskRequirements;
        [XmlAttribute("TaskCombineType")]
        public int taskCombineType;
        [XmlAttribute("TaskRewards")]
        public string taskRewards;
        [XmlAttribute("TaskRewardsCombineType")]
        public int taskRewardsCombineType;
    }
}
