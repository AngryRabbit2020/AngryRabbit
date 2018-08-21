using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public enum NodeType
{
    None,
    LU,
    U,
    RU,
    R,
    RD,
    D,
    LD,
    L,
}

public class Node
{
    public Node HeadNode;
    public bool canMove;
    public int x;
    public int y;
    public float G;
    public float H;
    public float F;

    public GameObject Obj;
    private Material mat;

    public Node(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;

        Obj = GameObject.CreatePrimitive(PrimitiveType.Cube);     
        Obj.transform.position = new Vector3(x * 1.05f, 0, y * 1.05f);
        mat = Obj.GetComponent<MeshRenderer>().material;
        Block scr = Obj.AddComponent<Block>();
        canMove = true;
        scr._Node = this;
    }

    public void UpdateData(float g, float h)
    {
        G = g;
        H = h;
        F = G + H;
    }

    public void SetHead(Node node)
    {
        HeadNode = node;
    }

    public void SetBlockWall()
    {
        canMove = false;
        mat.color = Color.black;
        //Debug.LogError(x + " : " + y);
    }

    public void SetBlockStart()
    {
        canMove = false;
        mat.color = Color.blue;
    }

    public void SetBlockEnd()
    {
        canMove = true;
        mat.color = Color.green;
    }

    public void SetBlockPath()
    {
        mat.color = Color.red;
    }

    public void SetBlockFound()
    {
        mat.color = Color.yellow;
    }
}

public class NavTest : MonoBehaviour {

    public static readonly int Length = 50;
    public static readonly int Height = 50;
    private Node[,] nodeArray = new Node[Length, Height];

    private List<Node> closed = new List<Node>();

    private List<Node> opened = new List<Node>();

    private Node start;
    private Node end;

    private void Awake()
    {
        for (int i = 0; i < Length; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                nodeArray[i, j] = new Node(i, j);
            }
        }

        start = nodeArray[1, 1];
        end = nodeArray[40, 45];

        start.SetBlockStart();
        end.SetBlockEnd();
    }

    public Dictionary<NodeType, Node>  GetSurroundActiveNode(Node node)
    {
        Node got;
        Dictionary<NodeType, Node> nodes = new Dictionary<NodeType, Node>();
        int x = node.x;
        int y = node.y;

        //左上
        got = GetNode(x - 1, y + 1);
        if (got != null && got.canMove && !closed.Contains(got))
            nodes.Add(NodeType.LU, got);
            
        //上
        got = GetNode(x, y + 1);
        if (got != null && got.canMove && !closed.Contains(got))
            nodes.Add(NodeType.U, got);

        //右上
        got = GetNode(x + 1, y + 1);
        if (got != null && got.canMove && !closed.Contains(got))
            nodes.Add(NodeType.RU, got);

        //右
        got = GetNode(x + 1, y);
        if (got != null && got.canMove && !closed.Contains(got))
            nodes.Add(NodeType.R, got);

        //右下
        got = GetNode(x + 1, y - 1);
        if (got != null && got.canMove && !closed.Contains(got))
            nodes.Add(NodeType.RD, got);

        //下
        got = GetNode(x, y - 1);
        if (got != null && got.canMove && !closed.Contains(got))
            nodes.Add(NodeType.D, got);

        //左下
        got = GetNode(x - 1, y - 1);
        if (got != null && got.canMove && !closed.Contains(got))
            nodes.Add(NodeType.LD, got);

        //左
        got = GetNode(x - 1, y);
        if (got != null && got.canMove && !closed.Contains(got))
            nodes.Add(NodeType.L, got);

        return nodes;
    }

    public Node GetNode(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Length || y >= Height)
        {
            return null;
        }

        return nodeArray[x, y];
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                Block scr = hitInfo.transform.gameObject.GetComponent<Block>();
                if (scr != null)
                {
                    scr._Node.SetBlockWall();
                }
            }
        }
    }

    public void StartEditPath()
    {
        int H = Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y);
        Debug.LogError("开始计算路线 --> H : " + H);

        //GetSurroundActiveNode(start);
        GetPath(start);
    }

    public void GetPath(Node node)
    {
        if (node != start && node != end)
            node.SetBlockFound();

        Dictionary<NodeType, Node> nodeDic = GetSurroundActiveNode(node);

        foreach (var item in nodeDic)
        {
            float _gAdd = 0f;
            switch (item.Key)
            {
                case NodeType.None:
                    break;
                case NodeType.LU:
                case NodeType.RU:
                case NodeType.RD:
                case NodeType.LD:
                    _gAdd = 1.4f;
                    break;
                case NodeType.U:
                case NodeType.R:
                case NodeType.D:
                case NodeType.L:
                    _gAdd = 1f;
                    break;
                default:
                    break;
            }

            if (!opened.Contains(item.Value))
            {
                item.Value.UpdateData(node.G + _gAdd, GetH(item.Value));
                opened.Add(item.Value);
                item.Value.SetHead(node);
            }
            else
            {
                if (item.Value.HeadNode != node && node.G + _gAdd < item.Value.G)
                {
                    item.Value.UpdateData(node.G + _gAdd, GetH(item.Value));
                    item.Value.SetHead(node);
                }
            }

            // 找到最终
            if (item.Value.H == 0)
            {
                Debug.LogError("找到最终 : " + item.Value.x + " : " + item.Value.y);
                ShowPath(item.Value);
                return;
            }
        }

        opened.Remove(node);
        closed.Add(node);

        // 寻找opened中f最小的
        Node NextNode = GetSmallestInOpened();
        if (NextNode == null)
        {
            Debug.LogError("没有找到路径");
            return;
        }

        GetPath(NextNode);
    }

    public Node GetSmallestInOpened()
    {
        if (opened.Count == 0)
            return null;

        opened.Sort(delegate (Node x, Node y)
        {
            if ((int)x.F > (int)y.F)
            {
                return 1;
            }

            if ((int)x.F < (int)y.F)
            {
                return -1;
            }
            return 0;
        });

        //Debug.LogError("最小的F : " + opened[0].F);
        return opened[0];
    }

    public int GetH(Node node)
    {
        return Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);
    }

    public void ShowPath(Node node)
    {
        if (node.HeadNode!= null)
        {
            if (node.HeadNode == start)
            {
                return;
            }
            else
            {
                node.HeadNode.SetBlockPath();
                ShowPath(node.HeadNode);
            }
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 200, 100), "start"))
        {
            StartEditPath();
        }
    }
}
