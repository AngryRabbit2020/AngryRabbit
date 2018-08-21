using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit
{
    /// <summary>
    /// 玩家数据模块
    /// </summary>
    public class PlayerPrefsEntity : AREntity
    {
        public int UserID
        {
            get { return PlayerPrefs.GetInt("playerPrefs_userID", 100001); }
            set
            {
                PlayerPrefs.SetInt("playerPrefs_userID", value);
                PlayerData.Instance.UserID.Set(value, this);
            }
        }

        public int Gold
        {
            get { return PlayerPrefs.GetInt("playerPrefs_gold", 100); }
            set
            {
                PlayerPrefs.SetInt("playerPrefs_gold", value);
                PlayerData.Instance.Gold.Set(value, this);
            }
        }

        public int Level
        {
            get { return PlayerPrefs.GetInt("playerPrefs_Level", 1); }
            set
            {
                PlayerPrefs.SetInt("playerPrefs_Level", value);
                PlayerData.Instance.Level.Set(value, this);
            }
        }

        public int Exp
        {
            get { return PlayerPrefs.GetInt("playerPrefs_Exp", 0); }
            set
            {
                PlayerPrefs.SetInt("playerPrefs_Exp", value);
                PlayerData.Instance.Exp.Set(value, this);
            }
        }

        public string TaskContains
        {
            get { return PlayerPrefs.GetString("playerPrefs_Task", "201"); }
            set
            {
                PlayerPrefs.SetString("playerPrefs_Task", value);
            }
        }

        public int Year
        {
            get { return PlayerPrefs.GetInt("playerPrefs_Year", 2018); }
            set
            {
                PlayerPrefs.SetInt("playerPrefs_Year", value);
                PlayerData.Instance.Year.Set(value, this);
            }
        }

        public int Week
        {
            get { return PlayerPrefs.GetInt("playerPrefs_Week", 1); }
            set
            {
                PlayerPrefs.SetInt("playerPrefs_Week", value);
                PlayerData.Instance.Week.Set(value, this);
            }
        }

        public int Hour
        {
            get { return PlayerPrefs.GetInt("playerPrefs_Hour", 0); }
            set
            {
                PlayerPrefs.SetInt("playerPrefs_Hour", value);
                PlayerData.Instance.Hour.Set(value, this);
            }
        }

        public int Minute
        {
            get { return PlayerPrefs.GetInt("playerPrefs_Minute", 0); }
            set
            {
                PlayerPrefs.SetInt("playerPrefs_Minute", value);
                PlayerData.Instance.Minute.Set(value, this);
            }
        }

        protected override void Init()
        {
            RegisterMsg((int)MsgLib.AddLevel, this);
            SynchronizePlayerData();
        }

        public override void HandleMsg(int msg_id, List<object> paramList)
        {
            if (msg_id == (int)MsgLib.AddLevel)
            {
                Level = Level + (int)paramList[0];
            }
        }

        public override void update()
        {
            
        }

        /// <summary>
        /// 同步玩家数据
        /// </summary>
        public void SynchronizePlayerData()
        {
            PlayerData.Instance.Gold = PropertyInt.CreateInt(Gold);
            PlayerData.Instance.UserID = PropertyInt.CreateInt(UserID);
            PlayerData.Instance.Level = PropertyInt.CreateInt(Level);
            PlayerData.Instance.Exp = PropertyInt.CreateInt(Exp);
            PlayerData.Instance.Year = PropertyInt.CreateInt(Year);
            PlayerData.Instance.Week = PropertyInt.CreateInt(Week);
            PlayerData.Instance.Hour = PropertyInt.CreateInt(Hour);
            PlayerData.Instance.Minute = PropertyInt.CreateInt(Minute);
        }
    }//end class
}// end namespace
