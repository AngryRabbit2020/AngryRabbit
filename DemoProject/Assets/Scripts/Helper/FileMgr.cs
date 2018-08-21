using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System;

namespace AngryRabbit
{
    /// <summary>
    /// 文件处理
    /// </summary>
    public class FileMgr
    {
        //读文件
        public static string ReadFile(string path, string fileName)
        {
            fileName = path + fileName;
            return ReadFile(fileName);
        }

        /// <summary>
        /// 读取加密文件并解密
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFile(string path)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                StreamReader streamReader = new StreamReader(fs);
                streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                string strLine = streamReader.ReadLine();
                while (strLine != null)
                {
                    sb.Append(strLine);
                    strLine = streamReader.ReadLine();
                }
                streamReader.Close();
                fs.Close();
                //string str = DecryptDES(sb.ToString(), keyss);
                //return str;
                return sb.ToString();
            }
            catch (System.Exception e)
            {
                Debug.LogError("文件读取错误 : " + path + "//" + e.Message);
            }
            return null;
        }

        //写文件
        public static bool WriteFile(string path, string fileName, string content)
        {
            fileName = fileName.Replace("/", Path.DirectorySeparatorChar + "");
            fileName = path + fileName;
            //得到当前文件目录
            string curPath = fileName.Substring(0, fileName.LastIndexOf(Path.DirectorySeparatorChar) + 1);

            if (!Directory.Exists(curPath))
            {
                Directory.CreateDirectory(curPath);
            }

            content = EncryptDES(content, keyss);
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            try
            {
                using (StreamWriter m_streamWriter = new StreamWriter(fs))
                {
                    m_streamWriter.Flush();
                    m_streamWriter.WriteLine(content);
                    m_streamWriter.Flush();
                    m_streamWriter.Dispose();
                    m_streamWriter.Close();
                }
            }
            catch (System.Exception ee)
            {
                Debug.LogError(ee.Message);
            }
            finally
            {
                fs.Dispose();
                fs.Close();
            }
            return true;
        }


        #region 文件加密、解密
        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        public static string keyss = "1234567z";
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        //文件解密
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                Debug.LogError("解密失败");
                return decryptString;
            }
        }

        #endregion

    }// end class
}// end namespace