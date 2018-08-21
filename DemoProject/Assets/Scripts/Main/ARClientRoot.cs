using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit
{
    /// <summary>
    /// 游戏根脚本
    /// </summary>
    public class ARClientRoot : MonoBehaviour
    {
        AREntity entity;
        // Use this for initialization
        private void Awake()
        {
            entity = AREntity.Instance;
            // 注册EntityMgr
            entity.SetEntityMgr(new EntityMgr());
        }

        private void Start()
        {
            //----------------------------发送功能性消息-------------------------------
            object param = (object)"Hello, my ARProject !";
            MsgSender.Instance.SendMsg((int)MsgLib.LobbySayHello, new List<object>() { param });

            //----------------------------玩家信息获取-------------------------------
            Debug.Log(string.Format("PlayerData : \n UserID : {0} \n Gold : {1} \n Level : {2} \n Exp : {3}", PlayerData.Instance.UserID.Get, PlayerData.Instance.Gold.Get, PlayerData.Instance.Level.Get, PlayerData.Instance.Exp.Get));

            //----------------------------任务配置文件读取-------------------------------
            MsgSender.Instance.SendMsg((int)MsgLib.LoadTask, null);

            //----------------------------任务添加-------------------------------
            //MsgSender.Instance.SendMsg((int)MsgLib.InitTask, null);

            //----------------------------行动合并测试-------------------------------
            //ARQueue<string> strQueue = new ARQueue<string>();
            //strQueue.EnQueue("111");
            //strQueue.EnQueue("222");
            //strQueue.EnQueue("333");
            //strQueue.EnQueue("444");
            //strQueue.EnQueue("555");
            //strQueue.EnQueue("666");
            //strQueue.EnQueue("777");
            //strQueue.EnQueue("888");
            //strQueue.Combine(new List<int>() { 0, 1, 2 }, "RESULT", InsertType.Head);
            //Debug.Log(strQueue.LogQueue());

            //Debug.LogWarning("current player level : " + PlayerLevelConfig.Instance.GetLevel(PlayerData.Instance.Exp.Get));

            //----------------------------时间节点时间测试-------------------------------
            List<object> paramList = new List<object>();
            paramList.Add((object)new AREvent("第一个事件", "测试用11111", EventCheckCondition.EachWeek, new TimePoint(2018, 1, 7, 30), () => 
            {
                Debug.LogError("第一个事件检查!!");
                TestButtons.Instance.PanelEvent.gameObject.SetActive(true);
            }));
            paramList.Add((object)new AREvent("第二个事件", "测试用22222", EventCheckCondition.EachYear, new TimePoint(2018, 1, 7, 30), () => 
            {
                Debug.LogError("第二个事件检查!!");
                TestButtons.Instance.PanelEvent.gameObject.SetActive(true);
            }));
            paramList.Add((object)new AREvent("第三个事件", "测试用33333", EventCheckCondition.AppointTime, new TimePoint(2018, 1, 7, 30), () => 
            {
                Debug.LogError("第三个事件检查!!");
                TestButtons.Instance.PanelEvent.gameObject.SetActive(true);
            }));
            MsgSender.Instance.SendMsg((int)MsgLib.AddTimePoint, paramList);
        }



        // Update is called once per frame
        void Update()
        {
            entity.update();
        }
    }//end class
}// end namespace