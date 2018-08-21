using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit {

    /// <summary>
    /// 消息结构
    /// </summary>
    public struct Msg
    {
        public int msgId;
        public List<object> paramList;

        public Msg(int id, List<object> list)
        {
            msgId = id;
            paramList = list;
        }
    }

    /// <summary>
    /// 消息处理模块
    /// </summary>
    public class MsgEntity : AREntity {

        private Dictionary<int, List<AREntity>> msgRegisterDic = null;

        private Queue<Msg> MsgQueue = null;

        private object lockMsgQuete = new object();

        protected override void Init()
        {
            //Debug.Log("I'm in Init : " + EntityID);
        }

        public override void RegisterMsg(int msg_id, AREntity entity)
        {
            //Debug.Log("Final in MsgEntity.RegisterMsg()");
            if (msgRegisterDic == null)
                msgRegisterDic = new Dictionary<int, List<AREntity>>();

            if (msgRegisterDic.ContainsKey(msg_id))
            {
                msgRegisterDic[msg_id].Add(entity);
            }
            else
            {
                List<AREntity> paramList = new List<AREntity>() { entity };
                msgRegisterDic.Add(msg_id, paramList);
            }
        }

        public override void SendMsg(int msg_id, List<object> paramList)
        {
            Msg msg = new Msg(msg_id, paramList);

            if (MsgQueue == null)
                MsgQueue = new Queue<Msg>();

            MsgQueue.Enqueue(msg);
        }

        public override void update()
        {
            if (MsgQueue.Count > 0)
            {
                lock (lockMsgQuete)
                {

                    Msg msg = MsgQueue.Dequeue();

                    if (msgRegisterDic.ContainsKey(msg.msgId))
                    {
                        foreach (var entity in msgRegisterDic[msg.msgId])
                        {
                            entity.HandleMsg(msg.msgId, msg.paramList);
                        }
                    }
                }
            }
        }


    }//end class
}// end namespace
