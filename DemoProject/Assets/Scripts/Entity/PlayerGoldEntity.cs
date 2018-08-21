using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace AngryRabbit
{
    /// <summary>
    /// 金币模块
    /// </summary>
    public class PlayerGoldEntity : AREntity
    {

        protected override void Init()
        {
            RegisterMsg((int)MsgLib.AddGold, this);
            RegisterMsg((int)MsgLib.MinusGold, this);
        }

        public override void HandleMsg(int msg_id, List<object> paramList)
        {
            if (msg_id == (int)MsgLib.AddGold)
            {
                int value = (int)paramList[0];
                AddGold(value);
            }

            if (msg_id == (int)MsgLib.MinusGold)
            {
                int value = (int)paramList[0];
                MinusGold(value);
            }
        }

        public override void update()
        {
            
        }

        /// <summary>
        /// 判断是否可以增加金币, 反馈或者增加
        /// </summary>
        /// <param name="value"></param>
        private void AddGold(int value)
        {
            // 这种访问方式是不允许的，需要找到一个合理的方式去处理每个属性的修改权限问题（目前：PlayerPrefsEntity保存修改至本地，修改PlayerData的数据，PlayerData中的数据只允许来自PlayerPrefsEntity的修改，并且支持全局访问）
            int gold = PlayerData.Instance.Gold.Get + value;
            EntityMgr.PlayerPrefsEntityIns.Gold = gold; 
        }

        /// <summary>
        /// 判断是否可以减少金币, 反馈或者减少
        /// </summary>
        /// <param name="value"></param>
        private void MinusGold(int value)
        {
            int gold = EntityMgr.PlayerPrefsEntityIns.Gold - value;
            if (gold < 0)
            {
                Debug.Log("金币不足");
                return;
            }
            EntityMgr.PlayerPrefsEntityIns.Gold = gold;
        }

    }//end class
}// end namespace
