using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

[ExecuteInEditMode]
public class MyEditor : EditorWindow
{
    public Transform m_All;

    GameObject OilPrefab;
    GameObject RoadBlock;
    GameObject WoodRoadBlock;

    [MenuItem("MyEditor/CreatWindow")]
    static void AddWindow()
    {
        Rect wr = new Rect(0, 0, 500, 500);
        MyEditor window = (MyEditor)EditorWindow.GetWindowWithRect(typeof(MyEditor), wr, true, "CreatWindow");
        window.Show();
    }

    void OnFocus()
    {
        Debug.Log("获得焦点时调用一次");

        m_All = GameObject.Find("All").transform;
        OilPrefab = Resources.Load<GameObject>("Prefabs/OilBomb");
        RoadBlock = Resources.Load<GameObject>("Prefabs/RoadBlock");
        WoodRoadBlock = Resources.Load<GameObject>("Prefabs/WoodRoadBlock");
    }

    void OnLostFocus()
    {
        Debug.Log("丢失焦点时调用一次");
    }

    void OnHierarchyChange()
    {
        Debug.Log("Hierarchy视图中的任何对象发生改变时调用一次");
    }

    void OnProjectChange()
    {
        Debug.Log("Project视图中的资源发生改变时调用一次");
    }

    void OnInspectorUpdate()
    {
        //Debug.Log("面板的更新");
        this.Repaint();
    }

    void OnSelectionChange()
    {
        //处于开启状态，并且在Hierarchy视图中选择某游戏对象时调用  
        foreach (Transform t in Selection.transforms)
        {
            Debug.Log("OnSelectionChange" + t.name);
        }
    }

    void OnDestroy()
    {
        Debug.Log("关闭时调用");
    }

    string textLength;
    string textWidth;
    string textHeight;
    void OnGUI()
    {
        if (GUILayout.Button("创建物体", GUILayout.Width(200)))
        {
            this.CreateCube();
        }
        if (GUILayout.Button("创建预制体", GUILayout.Width(200)))
        {
            this.CreateRandomPrefab();
        }
        if (GUILayout.Button("输出物体的Position", GUILayout.Width(200)))
        {
            this.DebugItemsPosition();
        }

        textLength = EditorGUILayout.TextField("长:", textLength);
        textWidth = EditorGUILayout.TextField("宽:", textWidth);
        textHeight = EditorGUILayout.TextField("高:", textHeight);

        if (GUILayout.Button("创建特定物体", GUILayout.Width(200)))
        {
            this.CreateAdvancedCube();
        }
    }

    //在这里方法中就可以绘制面板。
    //public override void OnInspectorGUI()
    //{
    //    //得到Test对象
    //    Test test = (Test)target;
    //    //绘制一个窗口
    //    test.mRectValue = EditorGUILayout.RectField("窗口坐标",
    //            test.mRectValue);
    //    //绘制一个贴图槽
    //    test.texture = EditorGUILayout.ObjectField("增加一个贴图", test.texture, typeof(Texture), true) as Texture;

    //}

    void CreateCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.SetParent(m_All);
        cube.name = "Cube_" + m_All.transform.childCount;
    }

    void CreateOilBombPrefab()
    {
        GameObject obj = Object.Instantiate(OilPrefab) as GameObject;
        obj.transform.SetParent(m_All);
        obj.name = "OilBomb_" + m_All.transform.childCount;
    }

    void CreateRandomPrefab()
    {
        int ran = Random.Range(1, 4);

        GameObject obj;
        switch (ran)
        {
            case 1:
                obj = Object.Instantiate(OilPrefab) as GameObject;
                break;
            case 2:
                obj = Object.Instantiate(RoadBlock) as GameObject;
                break;
            case 3:
                obj = Object.Instantiate(WoodRoadBlock) as GameObject;
                break;
            default:
                obj = Object.Instantiate(OilPrefab) as GameObject;
                break;
        }

        obj.transform.SetParent(m_All);
        obj.name = "SpecialPrefab_" + m_All.transform.childCount;
    }

    void DebugItemsPosition()
    {
        List<string> configList = new List<string>();
        for (int i = 0; i < m_All.transform.childCount; i++)
        {
            configList.Add(string.Format("{0}-{1}", m_All.GetChild(i).name, m_All.GetChild(i).position));
            Debug.LogError(string.Format("{0}-{1}", m_All.GetChild(i).name, m_All.GetChild(i).position));
        }

        SaveItemPositionConfig(configList);
    }

    void SaveItemPositionConfig(List<string> itemConfig)
    {
        StringBuilder sb = new StringBuilder();//声明一个可变字符串
        for (int i = 0; i < itemConfig.Count; i++)
        {
            //循环给字符串拼接字符
            sb.Append(itemConfig[i] + '|');
        }
        //写文件 文件名为save.text
        //这里的FileMode.create是创建这个文件,如果文件名存在则覆盖重新创建
        FileStream fs = new FileStream(Application.streamingAssetsPath + "/config.txt", FileMode.Create);
        //存储时时二进制,所以这里需要把我们的字符串转成二进制
        byte[] bytes = new UTF8Encoding().GetBytes(sb.ToString());
        fs.Write(bytes, 0, bytes.Length);
        //每次读取文件后都要记得关闭文件
        fs.Close();
    }

    void CreateAdvancedCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.SetParent(m_All);
        cube.transform.localScale = new Vector3(float.Parse(textWidth), float.Parse(textHeight), float.Parse(textLength));
        cube.name = "SpecialCube_" + m_All.transform.childCount;
    }
}
