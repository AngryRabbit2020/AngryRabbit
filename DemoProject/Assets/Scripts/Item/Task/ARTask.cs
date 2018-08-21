using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit
{
    /// <summary>
    /// 任务
    /// </summary>
    public class ARTask
    {
        // 从配置文件获取
        private int taskID;     // ID
        private TaskEntityType entityType;      // 模块类型
        private TaskType taskType;      // 类型
        private int nextTaskID;     // 后置ID
        private string taskTitle;       // 标题
        private string taskDesc;        // 描述
        private List<TaskRequirement> taskRequirements;        // 完成条件列表
        private TaskCombineType taskRequirementsCombineType;        // 完成条件组合方式
        private List<TaskReward> taskRewards;      // 奖励列表
        private TaskCombineType taskRewardsCombineType;     // 奖励获取方式

        public void InitTask(ConfigData data)
        {
            taskID = data.TaskID;
            entityType = data.taskEntityType;
            taskType = data.taskType;
            nextTaskID = data.nextTaskId;
            taskTitle = data.taskTitle;
            taskDesc = data.taskDesc;

            taskRequirements = new List<TaskRequirement>();
            for (int i = 0; i < data.requirementList.Count; i++)
            {
                TaskRequirement requirement = new TaskRequirement();
                requirement.InitRequirement(CheckIsTaskFinished, data.requirementList[i].RequirementType, data.requirementList[i].RecordType, data.requirementList[i].TargetValue);
                taskRequirements.Add(requirement);
            }

            taskRewards = new List<TaskReward>();
            for (int i = 0; i < data.rewardList.Count; i++)
            {
                TaskReward reward = new TaskReward();
                reward.InitTaskReward(i, data.rewardList[i].RequirementType, data.rewardList[i].RewardValue);
                taskRewards.Add(reward);
            }
            
            taskRequirementsCombineType = data.taskCombineType;
            taskRewardsCombineType = data.rewardGetType;

            InitTaskFinish();
        }

        public void InitTask(int taskid, TaskEntityType entitytype, TaskType tasktype, int nextid, string tasktitle, string taskdesc)
        {
            // test
            taskID = taskid;
            entityType = entitytype;
            taskType = tasktype;
            nextTaskID = nextid;
            taskTitle = tasktitle;
            taskDesc = taskdesc;
            TaskRequirement r1 = new TaskRequirement();
            r1.InitRequirement(CheckIsTaskFinished, TaskRequirementType.Gold, TaskRecordType.UseCurrent, 3000);
            TaskRequirement r2 = new TaskRequirement();
            r2.InitRequirement(CheckIsTaskFinished, TaskRequirementType.Level, TaskRecordType.UseCurrent, 20);
            taskRequirements = new List<TaskRequirement>() { r1, r2 };

            TaskReward re1 = new TaskReward();
            re1.InitTaskReward(0, TaskRewardType.Exp, 100);
            TaskReward re2 = new TaskReward();
            re2.InitTaskReward(1, TaskRewardType.Gold, 50);
            taskRewards = new List<TaskReward>() { re1, re2 };

            taskRequirementsCombineType = TaskCombineType.And;
            taskRewardsCombineType = TaskCombineType.Or;

            InitTaskFinish();
        }

        private void InitTaskFinish()
        {
            for (int i = 0; i < taskRequirements.Count; i++)
            {
                taskRequirements[i].PreCheckTask();
            }
        }

        public void CheckIsTaskFinished()
        {
            bool finished = true;
            if (taskRequirementsCombineType == TaskCombineType.Or)
            {
                finished = true;
            }
            else
            {
                for (int i = 0; i < taskRequirements.Count; i++)
                {
                    if (!taskRequirements[i].TaskFinished)
                        finished = false;
                }
            }

            if (finished)
            {
                Debug.Log(string.Format("当前任务完成 --> [{0}] : {1}", taskTitle, taskDesc));
                for (int i = 0; i < taskRequirements.Count; i++)
                {
                    taskRequirements[i].DeleteTaskListener();
                }

                GetTaskReward();

                MsgSender.Instance.SendMsg((int)MsgLib.GetNextTask, new List<object>() { (object)this.nextTaskID });
            }
            else
            {
                Debug.Log("当前任务还未完成");
            }
        }

        private void GetTaskReward()
        {
            if (taskRewardsCombineType == TaskCombineType.And)
            {
                string reward = "";
                for (int i = 0; i < taskRewards.Count; i++)
                {
                    reward += GetRewardName(taskRewards[i].RewardType) + ":" + taskRewards[i].TaskRewardNum.ToString() + " ";
                    taskRewards[i].GetReward();
                }
                TestButtons.Instance.Reward.text = reward;
                TestButtons.Instance.PanelGet.gameObject.SetActive(true);
            }
            else
            {
                string text1 = GetRewardName(taskRewards[0].RewardType) + ":" + taskRewards[0].TaskRewardNum.ToString() + " ";
                string text2 = GetRewardName(taskRewards[1].RewardType) + ":" + taskRewards[1].TaskRewardNum.ToString() + " ";
                TestButtons.Instance.LeftReward.text = text1;
                TestButtons.Instance.RightReward.text = text2;
                TestButtons.Instance.ChooseLeftBtn.onClick.AddListener(() =>
                {
                    TestButtons.Instance.PanelChoose.gameObject.SetActive(false);
                    string reward = GetRewardName(taskRewards[0].RewardType) + ":" + taskRewards[0].TaskRewardNum.ToString() + " ";
                    taskRewards[0].GetReward();
                    TestButtons.Instance.Reward.text = reward;
                    TestButtons.Instance.PanelGet.gameObject.SetActive(true);
                });
                TestButtons.Instance.ChooseRightBtn.onClick.AddListener(() =>
                {
                    TestButtons.Instance.PanelChoose.gameObject.SetActive(false);
                    string reward = GetRewardName(taskRewards[1].RewardType) + ":" + taskRewards[1].TaskRewardNum.ToString() + " ";
                    taskRewards[1].GetReward();
                    TestButtons.Instance.Reward.text = reward;
                    TestButtons.Instance.PanelGet.gameObject.SetActive(true);
                });
                TestButtons.Instance.PanelChoose.gameObject.SetActive(true);
            }
        }

        public static PropertyInt GetRequirementProperty(TaskRequirementType type)
        {
            switch (type)
            {
                case TaskRequirementType.Level:
                    return PlayerData.Instance.Level;
                    break;
                case TaskRequirementType.Gold:
                    return PlayerData.Instance.Gold;
                    break;
                default:
                    return null;
                    break;
            }
        }

        public static PropertyInt GetRewardProperty(TaskRewardType type)
        {
            switch (type)
            {
                case TaskRewardType.Exp:
                    return PlayerData.Instance.Exp;
                    break;
                case TaskRewardType.Gold:
                    return PlayerData.Instance.Gold;
                    break;
                default:
                    return null;
                    break;
            }
        }

        public static string GetRewardName(TaskRewardType type)
        {
            string name = "";
            switch (type)
            {
                case TaskRewardType.Exp:
                    name = "经验";
                    break;
                case TaskRewardType.Gold:
                    name = "金币";
                    break;
                default:
                    break;
            }

            return name;
        }
    }//end class

    /// <summary>
    /// 完成条件列表
    /// </summary>
    public class TaskRequirement
    {
        public UnityAction AchieveAction;
        public TaskRequirementType RequirementType;
        private TaskRecordType RecordType;
        public int TargetValue;
        public int CurrentValue;
        public int HighestValue;
        public bool TaskFinished = false;
        private PropertyInt property;

        public void InitRequirement(UnityAction action, TaskRequirementType requirementType, TaskRecordType recordType, int target)//, int current, int highest)
        {
            AchieveAction = action;
            RequirementType = requirementType;
            RecordType = recordType;
            TargetValue = target;

            InitTaskListener(ARTask.GetRequirementProperty(RequirementType));
        }

        public void InitTaskListener(PropertyInt prop)
        {
            if (prop != null)
            {
                property = prop;
                CurrentValue = prop.Get;
                HighestValue = prop.Get;
                prop.OnPropertyChanged += onListeningPropertyChanged;
            }
        }

        public void DeleteTaskListener()
        {
            if (property != null)
            {
                property.OnPropertyChanged -= onListeningPropertyChanged;
            }
        }

        public void PreCheckTask()
        {
            if (property != null)
            {
                onListeningPropertyChanged(property, null);
            }
        }

        private void onListeningPropertyChanged(PropertyInt prop, object param)
        {
            int progress = 0;

            if (RecordType == TaskRecordType.RecordHighest)
            {
                if (prop.Get >= HighestValue)
                {
                    CurrentValue = prop.Get;
                    HighestValue = prop.Get;
                }
                progress = HighestValue;
            }
            else
            {
                CurrentValue = prop.Get;
                if (prop.Get >= HighestValue)
                    HighestValue = prop.Get;

                progress = CurrentValue;
            }

            string state = "未完成";
            if (progress < TargetValue)
            {
                Debug.Log(string.Format("[{0}] 任务记录类型: {1} 当前进度 : {2} / {3}, 最高 : {4} ", state, RecordType, CurrentValue, TargetValue, HighestValue));
            }
            else
            {
                state = "已完成";
                Debug.Log(string.Format("[{0}] 任务记录类型: {1} 当前进度 : {2} / {3}, 最高 : {4} ", state, RecordType, CurrentValue, TargetValue, HighestValue));
                TaskFinished = true;
                if (AchieveAction != null)
                    AchieveAction();
            }
        }
    }// end class

    public class TaskReward
    {
        public int TaskRewardID;
        public TaskRewardType RewardType;
        public int TaskRewardNum;

        public void InitTaskReward(int id, TaskRewardType rewardtype, int num)
        {
            this.TaskRewardID = id;
            this.RewardType = rewardtype;
            this.TaskRewardNum = num;
        }

        public void GetReward()
        {
            // todo
            switch (RewardType)
            {
                case TaskRewardType.Exp:
                    MsgSender.Instance.SendMsg((int)MsgLib.AddExp, new List<object>() { (object)this.TaskRewardNum });
                    Debug.LogWarning("已领取奖励: 经验 +" + TaskRewardNum);
                    break;
                case TaskRewardType.Gold:
                    MsgSender.Instance.SendMsg((int)MsgLib.AddGold, new List<object>() { (object)this.TaskRewardNum });
                    Debug.LogWarning("已领取奖励: 金币 +" + TaskRewardNum);
                    break;
                default:
                    break;
            }
        }
    }

    public struct TaskRequirementItem
    {
        public TaskRequirementType RequirementType;
        public TaskRecordType RecordType;
        public int TargetValue;

        public TaskRequirementItem(TaskRequirementType _type, TaskRecordType _recordType, int target)
        {
            this.RequirementType = _type;
            this.RecordType = _recordType;
            this.TargetValue = target;
        }
    }

    public struct TaskRewardItem
    {
        public TaskRewardType RequirementType;
        public int RewardValue;

        public TaskRewardItem(TaskRewardType type, int value)
        {
            this.RequirementType = type;
            this.RewardValue = value;
        }
    }


    /// <summary>
    /// 记录类型
    /// </summary>
    public enum TaskRecordType
    {
        RecordHighest = 0,  // 使用最高的记录
        UseCurrent = 1,     // 使用当前的值
    }

    /// <summary>
    /// 所属模块类型
    /// </summary>
    public enum TaskEntityType
    {
        TaskLine = 0,       // 主线
        Achievement = 1,    // 成就
        Daily = 2,          // 日常
    }

    /// <summary>
    /// 任务类型
    /// </summary>
    public enum TaskType
    {
        Coin,           // 金币
        Level,          // 等级
    }

    /// <summary>
    /// 条件组合方式
    /// </summary>
    public enum TaskCombineType
    {
        And,            // 并且
        Or,             // 或者
    }

    /// <summary>
    /// 完成条件类型
    /// </summary>
    public enum TaskRequirementType
    {
        None,
        Level,            // 等级
        Gold,           // 金币
    }

    /// <summary>
    /// 奖励类型
    /// </summary>
    public enum TaskRewardType
    {
        None,
        Exp,            // 经验
        Gold,           // 金币
    }

}// end namespace
