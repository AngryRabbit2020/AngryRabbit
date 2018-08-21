using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit
{
    /// <summary>
    /// 任务模块
    /// </summary>
    public class TaskEntity : AREntity
    {
        public Dictionary<int, ARTask> TaskDic = null;

        protected override void Init()
        {
            if (TaskDic == null)
                TaskDic = new Dictionary<int, ARTask>();

            RegisterMsg((int)MsgLib.InitTask, this);
            RegisterMsg((int)MsgLib.GetNextTask, this);
        }

        public override void HandleMsg(int msg_id, List<object> paramList)
        {
            if (msg_id == (int)MsgLib.InitTask)
            {
                InitTask();
            }
            if (msg_id == (int)MsgLib.GetNextTask)
            {
                GetNextTask((int)paramList[0]);
            }
        }

        public void InitTask()
        {
            //----------------------------加入监听任务-------------------------------
            int taskID = int.Parse(EntityMgr.PlayerPrefsEntityIns.TaskContains);
            ConfigData data = ConfigMgr.Instance.GetConfigDataByID(taskID);

            ARTask test = new ARTask();
            test.InitTask(data);
        }

        public void GetNextTask(int nextId)
        {
            PlayerLevelTaskLineData data = PlayerLevelTaskLineConfig.Instance.GetDataByID(nextId);
            if (data != null)
            {
                ARTask test = new ARTask();
                test.InitTask(data);
                EntityMgr.PlayerPrefsEntityIns.TaskContains = nextId.ToString();
            }
        }

        public override void update()
        {

        }
    }//end class
}// end namespace
