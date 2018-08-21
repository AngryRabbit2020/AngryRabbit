using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit
{
    public enum InsertType
    {
        Head,
        Bottom,
    }

    public class ARQueue<T>
    {
        private List<T> arrayList = new List<T>();

        public int Count
        {
            get
            {
                if (arrayList == null)
                    return 0;
                else
                    return arrayList.Count;
            }
        }

        public void EnQueue(T value)
        {
            if (arrayList == null)
                arrayList = new List<T>();

            arrayList.Add(value);
        }

        public T DeQueue()
        {
            if (arrayList == null || arrayList.Count == 0)
            {
                Debug.LogError("this queue has no value");
                return default(T);
            }

            T value = arrayList[0];
            arrayList.RemoveAt(0);
            return value;
        }

        public bool Contains(T value)
        {
            if (arrayList.Contains(value))
                return true;
            else
                return false;
        }

        public bool Combine(List<int> indexList, T result, InsertType insertType)
        {
            if (arrayList == null || indexList.Count > arrayList.Count || result == null)
            {
                Debug.LogError("combine not finished");
                return false;
            }

            List<T> list = new List<T>(indexList.Count);
            for (int i = 0; i < indexList.Count; i++)
            {
                list.Add(arrayList[indexList[i]]);
            }

            for (int i = 0; i < list.Count; i++)
            {
                arrayList.Remove(list[i]);
            }

            int insertIndex = 0;
            switch (insertType)
            {
                case InsertType.Head:
                    insertIndex = 0;
                    break;
                case InsertType.Bottom:
                    insertIndex = arrayList.Count;
                    break;
                default:
                    break;
            }
            arrayList.Insert(insertIndex, result);
            
            return true;
        }

        public string LogQueue()
        {
            string str = "";
            if (arrayList == null || arrayList.Count == 0)
            {
                Debug.LogError("this queue has no value");
                return str;
            }

            for (int i = 0; i < arrayList.Count; i++)
            {
                str += arrayList[i].ToString() + ", ";
            }

            return str;
        }

    }// end class
}// end namespace
