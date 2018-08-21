using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit
{
    /// <summary>
    /// 经验模块
    /// </summary>
    public class PlayerExpEntity : AREntity
    {
        protected override void Init()
        {
            RegisterMsg((int)MsgLib.AddExp, this);
            RegisterMsg((int)MsgLib.MinusExp, this);
        }

        public override void HandleMsg(int msg_id, List<object> paramList)
        {
            if (msg_id == (int)MsgLib.AddExp)
            {
                int value = (int)paramList[0];
                AddExp(value);
            }
        }

        public void AddExp(int value)
        {
            int exp = PlayerData.Instance.Exp.Get + value;
            EntityMgr.PlayerPrefsEntityIns.Exp = exp;
            // 处理升级问题
            int level = PlayerLevelSettingConfig.Instance.GetLevel(PlayerData.Instance.Exp.Get);
            if (level > PlayerData.Instance.Level.Get)
                EntityMgr.PlayerPrefsEntityIns.Level = level;

            Debug.LogWarning(string.Format("current player level infomation : level --> {0}, exp --> {1}", PlayerData.Instance.Level.Get, PlayerData.Instance.Exp.Get));
        }

        public override void update()
        {

        }

    }// end class
}// end namespace