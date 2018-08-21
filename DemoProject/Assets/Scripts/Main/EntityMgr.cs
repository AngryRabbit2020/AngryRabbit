using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit
{
    /// <summary>
    /// 模块管理器(所有模块都不可通过外部直接访问, 必须通过EntityMgr获取到功能模块的实例)
    /// </summary>
    public class EntityMgr : AREntity
    {
        public MsgEntity MsgEntityIns;
        public LobbyEntity LobbyEntityIns;
        public PlayerPrefsEntity PlayerPrefsEntityIns;
        public PlayerGoldEntity PlayerGoldEntityIns;
        public PlayerExpEntity PlayerExpEntityIns;
        public ConfigLoadEntity ConfigLoadEntityIns;
        public TaskEntity TaskEntityIns;
        public ActionEntity ActionEntityIns;
        public TimeEntity TimeEntityIns;

        protected override void Init()
        {
            // --------注册功能性Entity--------

            // 消息处理模块
            MsgEntityIns = new MsgEntity();
            RegisterEntity((int)EntityLib.MsgEntity, MsgEntityIns);
            // 大厅模块
            LobbyEntityIns = new LobbyEntity();
            RegisterEntity((int)EntityLib.LobbyEntity, LobbyEntityIns);
            // 玩家数据保存模块
            PlayerPrefsEntityIns = new PlayerPrefsEntity();
            RegisterEntity((int)EntityLib.PlayerPrefsEntity, PlayerPrefsEntityIns);
            // 玩家金币模块
            PlayerGoldEntityIns = new PlayerGoldEntity();
            RegisterEntity((int)EntityLib.PlayerGoldEntity, PlayerGoldEntityIns);
            // 玩家经验模块
            PlayerExpEntityIns = new PlayerExpEntity();
            RegisterEntity((int)EntityLib.PlayerExpEntity, PlayerExpEntityIns);
            // 读取配置文件
            ConfigLoadEntityIns = new ConfigLoadEntity();
            RegisterEntity((int)EntityLib.ConfigLoadEntity, ConfigLoadEntityIns);
            // 玩家任务模块
            TaskEntityIns = new TaskEntity();
            RegisterEntity((int)EntityLib.TaskEntity, TaskEntityIns);
            // 玩家行动模块
            ActionEntityIns = new ActionEntity();
            RegisterEntity((int)EntityLib.ActionEntity, ActionEntityIns);
            // 时间推进模块
            TimeEntityIns = new TimeEntity();
            RegisterEntity((int)EntityLib.TimeEntity, TimeEntityIns);
        }

        public override void RegisterMsg(int msg_id, AREntity entity)
        {
            MsgEntityIns.RegisterMsg(msg_id, entity);
        }

        public override void SendMsg(int msg_id, List<object> paramList)
        {
            MsgEntityIns.SendMsg(msg_id, paramList);
        }

        public override void update()
        {
            
        }
    }//end class
}// end namespace
