using UnityEngine;
using System.Collections;

namespace AngryRabbit
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MsgLib
    {
        LobbySayHello = 0,
        LobbySayGoodbye,
        // 金币相关
        AddGold,
        MinusGold,
        // 等级相关
        AddLevel,
        // 任务相关
        LoadTask,
        InitTask,
        GetNextTask,
        // 行动相关
        AddAction,
        ActionFinish,
        // 经验相关
        AddExp,
        MinusExp,
        // 时间相关
        TimePause,
        TimeStart,
        TimeForward,
        TimeBackward,
        TimeJump,
        AddTimePoint,
        EventActionFinish,
    }
}// end namespace
