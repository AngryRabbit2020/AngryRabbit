using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

namespace AngryRabbit
{
    /// <summary>
    /// 测试按钮，比较乱
    /// </summary>
    public class TestButtons : MonoBehaviour
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("AngryRabbit/PlayerPrefs.DeleteAll")]
#endif
        public static void Clean()
        {
            PlayerPrefs.DeleteAll();
        }

        private static TestButtons instance;
        public static TestButtons Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.Find("Canvas").GetComponent<TestButtons>();

                return instance;
            }
        }

        public Button AddGoldBtn;
        public Button MinusGoldBtn;
        public Button AddLevelBtn;

        public OpenFileMgr OpenFileMgr_Ins;


        //----------选择奖励----------
        public Transform PanelChoose;
        public Button ChooseLeftBtn;
        public Text LeftReward;
        public Button ChooseRightBtn;
        public Text RightReward;

        //----------领取奖励----------
        public Transform PanelGet;
        public Text Reward;
        public Button GetReward;

        //----------行动进度----------
        public Button AddActionBtn;
        public GridLayoutGroup Group;
        public ActionItem Item;

        //----------时间进度----------
        public Text DateText;
        public Text TimeText;

        public Button TimePauseBtn;
        public Button TimeForwardBtn;
        public Button TimeBackWardBtn;
        public Button TimeJumpBtn;

        //----------行动事件----------
        public Transform PanelEvent;
        public Text EventName;
        public Text EventDesc;
        public Button NextEventBtn;

        //----------读取文件----------
        public Button ReadFileBtn;


        UnityAction addGoldAction;

        public void TestAction()
        {
            Debug.LogError("!!!Action Can be sent!!!");
        }

        bool timeState = true;
        private void Awake()
        {
            addGoldAction = TestAction;

            // 增加金币
            AddGoldBtn.onClick.AddListener(() =>
            {
                object param = (object)100;
                List<object> paramList = new List<object>() { param, (object)addGoldAction };
                MsgSender.Instance.SendMsg((int)MsgLib.AddGold, paramList);
            });
            // 使用金币
            MinusGoldBtn.onClick.AddListener(() =>
            {
                object param = (object)200;
                List<object> paramList = new List<object>() { param };
                MsgSender.Instance.SendMsg((int)MsgLib.MinusGold, paramList);
            });
            // 增加经验
            AddLevelBtn.onClick.AddListener(() =>
            {
                object param = (object)50;
                List<object> paramList = new List<object>() { param };
                MsgSender.Instance.SendMsg((int)MsgLib.AddExp, paramList);
            });
            GetReward.onClick.AddListener(() =>
            {
                PanelGet.gameObject.SetActive(false);
            });
            AddActionBtn.onClick.AddListener(() =>
            {
                List<object> paramList = new List<object>() { (object)(ActionLib)Random.Range(0, 25), (object)Random.Range(1f, 5f), (object)10 };
                MsgSender.Instance.SendMsg((int)MsgLib.AddAction, paramList);
            });
            TimePauseBtn.onClick.AddListener(() =>
            {
                timeState = !timeState;

                if (timeState)
                    MsgSender.Instance.SendMsg((int)MsgLib.TimeStart, null);
                else
                    MsgSender.Instance.SendMsg((int)MsgLib.TimePause, null);
            });
            TimeForwardBtn.onClick.AddListener(() =>
            {
                MsgSender.Instance.SendMsg((int)MsgLib.TimeForward, null);
            });
            TimeBackWardBtn.onClick.AddListener(() =>
            {
                MsgSender.Instance.SendMsg((int)MsgLib.TimeBackward, null);
            });
            TimeJumpBtn.onClick.AddListener(() =>
            {
                MsgSender.Instance.SendMsg((int)MsgLib.TimeJump, null);
            });
            NextEventBtn.onClick.AddListener(() =>
            {
                MsgSender.Instance.SendMsg((int)MsgLib.EventActionFinish, null);
                PanelEvent.gameObject.SetActive(false);
            });
            ReadFileBtn.onClick.AddListener(() =>
            {
                OpenFileMgr_Ins.OpenFileDir();
            });
        }

        public void AddActionItem(ARAction action, UnityAction callback)
        {
            GameObject item = Object.Instantiate(Item.gameObject, Group.transform) as GameObject;
            ActionItem actionItem = item.GetComponent<ActionItem>();
            actionItem.InitItem(action, callback);
            actionItem.gameObject.SetActive(true);
        }


    }//end class
}// end namespace
