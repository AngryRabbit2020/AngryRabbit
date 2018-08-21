using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit
{
    /// <summary>
    /// 行动模块
    /// </summary>
    public class ActionEntity : AREntity
    {
        public ARQueue<ARAction> ActionQueue;

        public Dictionary<ActionLib, List<ActionLib>> ActionCombineTypeDic;

        private bool isOnActionExcuting = false;

        private ARAction currentExcutingAction = null;

        protected override void Init()
        {
            ActionQueue = new ARQueue<ARAction>();
            ActionCombineTypeDic = new Dictionary<ActionLib, List<ActionLib>>();
            isOnActionExcuting = false;
            currentExcutingAction = null;
            RegisterMsg((int)MsgLib.AddAction, this);
            RegisterMsg((int)MsgLib.ActionFinish, this);
        }

        public override void HandleMsg(int msg_id, List<object> paramList)
        {
            if (msg_id == (int)MsgLib.AddAction)
            {
                AddAction((ActionLib)paramList[0], (float)paramList[1], (int)paramList[2]);
            }

            if (msg_id == (int)MsgLib.ActionFinish)
            {
                OnActionFinish();
            }
        }

        public void AddAction(ActionLib type, float time, int exp)
        {
            if (ActionQueue == null)
                ActionQueue = new ARQueue<ARAction>();

            if (ActionQueue.Count >= 7)
            {
                Debug.Log("Current action queue is full");
                return;
            }

            ARAction action = new ARAction();
            action.InitAction(type, time, exp);
            TestButtons.Instance.AddActionItem(action, OnActionFinish);
            ActionQueue.EnQueue(action);
        }

        public void OnActionFinish()
        {
            isOnActionExcuting = false;
            if (currentExcutingAction != null && !ActionQueue.Contains(currentExcutingAction))
                currentExcutingAction.GainBenefit();

            currentExcutingAction = null;
        }

        public void ExcuteAction()
        {
            if (ActionQueue == null || ActionQueue.Count == 0)
                return;

            if (isOnActionExcuting)
                return;

            if (currentExcutingAction != null)
                return;

            currentExcutingAction = ActionQueue.DeQueue();
            currentExcutingAction.ActionStart();
            isOnActionExcuting = true;
        }

        public override void update()
        {
            ExcuteAction();
        }
    }//end class
}// end namespace
