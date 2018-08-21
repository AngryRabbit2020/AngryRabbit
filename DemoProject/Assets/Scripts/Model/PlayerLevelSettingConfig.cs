using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AngryRabbit
{
    /// <summary>
    /// 玩家等级配置文件加载
    /// </summary>
    public class PlayerLevelSettingConfig
    {
        private static PlayerLevelSettingConfig instance;
        public static PlayerLevelSettingConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerLevelSettingConfig();
                }

                return instance;
            }
        }

        Dictionary<int, PlayerLevelData> dataDic;
        public void loadData(List<PlayerLevelItem> p_list)
        {
            if (dataDic == null) dataDic = new Dictionary<int, PlayerLevelData>();
            dataDic.Clear();
            if (p_list == null)
            {
                Debug.LogError("输入列表为空！");
                return;
            }

            foreach (var v in p_list)
            {
                dataDic.Add(v.level, new PlayerLevelData(v));
            }

            foreach (var item in dataDic)
            {
                Debug.LogWarning(string.Format("LEVEL : {0} --> MIN : {1}  MAX : {2}", item.Value.level, item.Value.min, item.Value.max));
            }
        }

        public int GetLevel(int exp)
        {
            foreach (var item in dataDic)
            {
                if (exp >= item.Value.min && exp < item.Value.max)
                {
                    return item.Key;
                }
            }

            return -1;
        }
    }

    /// <summary>
    /// 玩家等级数据
    /// </summary>
    public class PlayerLevelData
    {
        public int level;
        public int min;
        public int max;
        public PlayerLevelData(PlayerLevelItem data)
        {
            this.level = data.level;
            this.min = data.minExp;
            this.max = data.maxExp;
        }
    }

    public struct PlayerLevelItem
    {
        [XmlAttribute("Level")]
        public int level;
        [XmlAttribute("MinExp")]
        public int minExp;
        [XmlAttribute("MaxExp")]
        public int maxExp;
    }

}
