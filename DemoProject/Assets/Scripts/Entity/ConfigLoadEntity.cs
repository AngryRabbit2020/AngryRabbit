using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace AngryRabbit
{
    /// <summary>
    /// 配置文件读取模块
    /// </summary>
    public class ConfigLoadEntity : AREntity
    {
        protected override void Init()
        {
            RegisterMsg((int)MsgLib.LoadTask, this);
        }

        public override void HandleMsg(int msg_id, List<object> paramList)
        {
            if (msg_id == (int)MsgLib.LoadTask)
            {
                LoadTask();
            }
        }

        private void LoadTask()
        {
            PlayerLevelSettingConfig.Instance.loadData(Load<PlayerLevelItem>("LevelSettingConfig"));
            PlayerLevelTaskLineConfig.Instance.loadData(Load<PlayerLevelTaskLineItem>("LevelTaskLineConfig"));

            Debug.LogWarning("current player level : " + PlayerLevelSettingConfig.Instance.GetLevel(PlayerData.Instance.Exp.Get));

            MsgSender.Instance.SendMsg((int)MsgLib.InitTask, null);
        }

        public static List<T> Load<T>(string p_FileName) where T : struct
        {
            List<T> datas = null;
            try
            {
                p_FileName = Application.streamingAssetsPath + "/Config/" + p_FileName + ".xml";
                string config = FileMgr.ReadFile(p_FileName);

                XmlSerializer dataSerializer = new XmlSerializer(typeof(List<T>));
                byte[] bytes = Encoding.UTF8.GetBytes(config);
                StreamReader sr = new StreamReader(new MemoryStream(bytes));
                datas = (List<T>)dataSerializer.Deserialize(sr);
                if (datas == null && datas.Count <= 0)
                {
                    Debug.LogError("数据初始化为空");
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("配置文件解析错误: " + ex.Message);
            }
            return datas;
        }

        public override void update()
        {

        }
    }//end class
}//end namespace
