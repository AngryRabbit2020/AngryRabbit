using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;
using AngryRabbit;

public class ActionItem : MonoBehaviour {
    public Text ActionText;
    public Scrollbar Bar;

    public ARAction m_Action;
    private UnityAction callBack;
    private Image frontBar;

    public void InitItem(ARAction action, UnityAction finishCallBack)
    {
        this.ActionText.text = action.ActionType.ToString();
        this.m_Action = action;
        this.callBack = finishCallBack;
        frontBar = Bar.targetGraphic.GetComponent<Image>();
        frontBar.color = new Color32(255, 206, 0, 255);
        frontBar.gameObject.SetActive(false);
    }

    float time = 0f;
    public void FixedUpdate()
    {
        if (m_Action.ActionBegin && !m_Action.ActionIsDone)
        {
            frontBar.gameObject.SetActive(true);
            time += Time.deltaTime;
            if (time >= m_Action.ActionTime)
            {
                m_Action.ActionIsDone = true;
                m_Action.ActionBegin = false;
                time = 0f;
                if (callBack != null)
                    callBack();

                Destroy(this.gameObject);
            }
            else
            {
                //UI更新
                float progress = time / m_Action.ActionTime;
                Bar.size = progress;
            }
        }
    }
}
