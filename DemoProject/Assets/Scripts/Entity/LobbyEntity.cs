using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit
{
    /// <summary>
    /// 大厅模块
    /// </summary>
    public class LobbyEntity : AREntity
    {
        protected override void Init()
        {
            RegisterMsg((int)MsgLib.LobbySayHello, this);
            RegisterMsg((int)MsgLib.LobbySayGoodbye, this);
        }

        public override void HandleMsg(int msg_id, List<object> paramList)
        {
            if (msg_id == (int)MsgLib.LobbySayHello)
            {
                string text = (string)paramList[0];
                Debug.Log(text);
            }

            if (msg_id == (int)MsgLib.LobbySayGoodbye)
            {
                string text = (string)paramList[0];
                Debug.Log(text);
            }
        }

        public override void update()
        {

        }
    }//end class
}// end namespace
