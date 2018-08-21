using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit
{
    /// <summary>
    /// 配置文件管理
    /// </summary>
    public class ConfigMgr
    {
        private static ConfigMgr instance;

        public static ConfigMgr Instance
        {
            get
            {
                if (instance == null)
                    instance = new ConfigMgr();

                return instance;
            }
        }

        public Dictionary<int, ConfigData> ConfigDataDic = new Dictionary<int, ConfigData>();

        public ConfigData GetConfigDataByID(int id)
        {
            if (ConfigDataDic.ContainsKey(id))
                return ConfigDataDic[id];
            else
            {
                Debug.LogError("Can not find config data which ID is " + id);
                return null;
            }
        }

    }

    public class ConfigData
    {
        public int TaskID;
        public TaskEntityType taskEntityType;
        public TaskType taskType;
        public int nextTaskId;
        public string taskTitle;
        public string taskDesc;
        public List<TaskRequirementItem> requirementList;
        public TaskCombineType taskCombineType;
        public List<TaskRewardItem> rewardList;
        public TaskCombineType rewardGetType;

        protected void SaveConfig(int configId, ConfigData data)
        {
            if (ConfigMgr.Instance.ConfigDataDic != null)
            {
                if (ConfigMgr.Instance.ConfigDataDic.ContainsKey(configId))
                    Debug.LogError("ConfigDataDic has config data with same ID : " + configId);
                else
                    ConfigMgr.Instance.ConfigDataDic.Add(configId, data);
            }
        } 
    }
}
