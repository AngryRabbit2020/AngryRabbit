using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data;
using Excel;

namespace AngryRabbit
{
    public class OpenFileMgr : MonoBehaviour
    {
        private static OpenFileMgr instance;
        public static OpenFileMgr Instance
        {
            get
            {
                if (instance == null)
                    instance = new OpenFileMgr();

                return instance;
            }
        }

        public string ExcelFilePath;

        public void OpenFile()
        {
            OpenFileName ofn = new OpenFileName();

            ofn.structSize = Marshal.SizeOf(ofn);

            ofn.filter = "All Files\0*.*\0\0";

            ofn.file = new string(new char[256]);

            ofn.maxFile = ofn.file.Length;

            ofn.fileTitle = new string(new char[64]);

            ofn.maxFileTitle = ofn.fileTitle.Length;
            string path = UnityEngine.Application.streamingAssetsPath;
            path = path.Replace('/', '\\');
            //默认路径  
            ofn.initialDir = path;
            //ofn.initialDir = "D:\\MyProject\\UnityOpenCV\\Assets\\StreamingAssets";  
            ofn.title = "Open Project";

            ofn.defExt = "JPG";//显示文件的类型  
                               //注意 一下项目不一定要全选 但是0x00000008项不要缺少  
            ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  

            if (WindowDll.GetOpenFileName(ofn))
            {
                Debug.Log(string.Format("Selected file with full path: {0}", ofn.file));
            }
        }

        public void OpenFileDir()
        {
            OpenDialogDir ofn2 = new OpenDialogDir();
            ofn2.pszDisplayName = new string(new char[2000]); ;     // 存放目录路径缓冲区  
            ofn2.lpszTitle = "Open Project";// 标题  
                                            //ofn2.ulFlags = BIF_NEWDIALOGSTYLE | BIF_EDITBOX; // 新的样式,带编辑框  
            IntPtr pidlPtr = DllOpenFileDialog.SHBrowseForFolder(ofn2);

            char[] charArray = new char[2000];
            for (int i = 0; i < 2000; i++)
                charArray[i] = '\0';

            DllOpenFileDialog.SHGetPathFromIDList(pidlPtr, charArray);

            ExcelFilePath = new String(charArray);

            ExcelFilePath = ExcelFilePath.Substring(0, ExcelFilePath.IndexOf('\0'));

            Debug.Log(ExcelFilePath);

            GetExcelFilesInPath();
        }

        public List<string> FilePathList = new List<string>();
        public void GetExcelFilesInPath()
        {
            string fullPath = ExcelFilePath + "/";

            //获取指定路径下面的所有资源文件
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo direction = new DirectoryInfo(fullPath);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

                Debug.Log(files.Length);

                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".xlsx"))
                    {
                        FilePathList.Add(fullPath + files[i].Name);
                    }
                }
            }
            else
            {
                Debug.LogError("not find such path : " + fullPath);
            }

             StartCoroutine(ExcuteExcel());
        }

        IEnumerator ExcuteExcel()
        {
            for (int i = 0; i < FilePathList.Count; i++)
            {
                FileStream stream = File.Open(FilePathList[i], FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                DataSet result = excelReader.AsDataSet();

                GenerateCS(result);

                //int columns = result.Tables[0].Columns.Count;
                //int rows = result.Tables[0].Rows.Count;

                //for (int j = 0; j < rows; j++)
                //{
                //    for (int x = 0; x < columns; x++)
                //    {
                //        string nvalue = result.Tables[0].Rows[j][x].ToString();
                //        Debug.Log(nvalue);
                //    }
                //}

                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// 生成CS文件
        /// </summary>
        void GenerateCS(DataSet data)
        {
            var table = data.Tables[0];
            int rows = table.Rows.Count;
            int columns = table.Columns.Count;

            string nameSpace = "";
            string className = "";
            List<string> propNameList = new List<string>();
            List<string> summaryList = new List<string>();
            List<string> typeList = new List<string>();

            for (int i = 0; i < rows; i++)
            {
                string tag = table.Rows[i][0].ToString();
                Debug.LogError("tag : " + tag);
                if (tag == ConstLib.TAG_NAMESPACE)
                {
                    if (!string.IsNullOrEmpty(table.Rows[i][1].ToString()))
                        nameSpace = table.Rows[i][1].ToString();
                }
                else if (tag == ConstLib.TAG_CLASS)
                {
                    if (!string.IsNullOrEmpty(table.Rows[i][1].ToString()))
                        className = table.Rows[i][1].ToString();
                }
                else if (tag == ConstLib.TAG_PROPNAME)
                {
                    for (int j = 1; j < columns; j++)
                    {
                        string propName = table.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(propName))
                            propNameList.Add(propName);
                    }
                }
                else if (tag == ConstLib.TAG_SUMMARY)
                {
                    for (int j = 1; j < columns; j++)
                    {
                        string summary = table.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(summary))
                            summaryList.Add(summary);
                    }
                }
                else if (tag == ConstLib.TAG_TYPE)
                {
                    for (int j = 1; j < columns; j++)
                    {
                        string type = table.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(type))
                            typeList.Add(type);
                    }
                }
            }

            if (string.IsNullOrEmpty(nameSpace) || string.IsNullOrEmpty(className) || (propNameList.Count != summaryList.Count || propNameList.Count != typeList.Count || summaryList.Count != typeList.Count))
            {
                Debug.LogError("不规范的文件!!");
                return;
            }

            FileStream file = new FileStream(ExcelFilePath + "\\ConfigFile\\" + className + ".cs", FileMode.OpenOrCreate);

            StreamWriter sw = new StreamWriter(file, Encoding.UTF8);

            sw.WriteLine("using System;\nusing System.Collections.Generic;\nusing System.IO;\n");

            sw.WriteLine("namespace " + nameSpace + "\r\n{\n");
            sw.WriteLine("public class " + className + "\r\n{");

            for (int i = 0; i < propNameList.Count; i++)
            {
                sw.WriteLine(GenerateProperty(summaryList[i], typeList[i], propNameList[i]));
            }

            sw.WriteLine("}");
            sw.WriteLine("}");

            sw.Close();
        }

        string GenerateProperty(string summary, string type, string propertyName)
        {
            string property = "";

            property = string.Format("/// <summary>\n/// {0}\n/// </summary>\npublic {1} {2} {3} get; set; {4}\n", summary, type, propertyName, "{", "}\n");

            return property;
        }

    }//end class

}//end namespace
