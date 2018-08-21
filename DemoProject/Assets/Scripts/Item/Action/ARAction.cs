using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit
{
    /// <summary>
    /// 玩家行动
    /// </summary>
    public class ARAction
    {
        public ActionLib ActionType;
        public float ActionTime;
        public int ExpBenefit;
        public List<Benefit> BenefitList;

        public bool ActionBegin = false;
        public bool ActionIsDone = false;

        public void InitAction(ActionLib type, float time, int exp)
        {
            this.ActionType = type;
            this.ActionTime = time;
            this.ExpBenefit = exp;
        }

        public void ActionStart()
        {
            ActionBegin = true;
            Debug.Log(string.Format("Do action : [{0}], time : {1}", ActionType.ToString(), ActionTime.ToString()));
        }

        public void GainBenefit()
        {
            MsgSender.Instance.SendMsg((int)MsgLib.AddExp, new List<object>() { (object)this.ExpBenefit });
            Debug.Log("[" + ActionType.ToString() + "]已领取行动奖励: 经验 +" + ExpBenefit);
        }

    }// end class

    public class Benefit
    {
        public BenefitType Type;
        public int BenefitNum;
    }

}// end namespace