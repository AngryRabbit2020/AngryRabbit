using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit
{
    /// <summary>
    /// 消息发送器
    /// </summary>
    public class MsgSender
    {

        private static MsgSender instance;

        public static MsgSender Instance
        {
            get
            {
                if (instance == null)
                    instance = new MsgSender();

                return instance;
            }
        }

        public void SendMsg(int msg_id, List<object> param_List)
        {
            AREntity.EntityMgr.SendMsg(msg_id, param_List);
        }
    }//end class
}// end namespace
