using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit
{
    /// <summary>
    /// 玩家数据(更新UI获取数据, 只允许PlayerPrefsEntity修改数据, 允许任何模块访问数据)
    /// </summary>
    public class PlayerData
    {
        private static PlayerData instance;
        public static PlayerData Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerData();

                return instance;
            }
        }

        #region 玩家ID
        public PropertyInt UserID;
        #endregion

        #region 金币
        public PropertyInt Gold;
        #endregion

        #region 经验
        public PropertyInt Exp;
        #endregion

        #region 等级
        public PropertyInt Level;
        #endregion

        #region 年
        public PropertyInt Year;
        #endregion

        #region 周
        public PropertyInt Week;
        #endregion

        #region 时
        public PropertyInt Hour;
        #endregion

        #region 分
        public PropertyInt Minute;
        #endregion

    }//end class

    public class PropertyInt
    {
        public static PropertyInt CreateInt(int defaultValue)
        {
            return new PropertyInt(defaultValue);
        }

        public delegate void PropertyChanged(PropertyInt prop, object param);
        public PropertyChanged OnPropertyChanged = null;
        //-----------------------------------------------------------------------------
        protected int mValue;

        //-----------------------------------------------------------------------------
        public PropertyInt(int default_value)
        {
            mValue = default_value;
        }

        //-----------------------------------------------------------------------------
        public int Get
        {
            get
            {
                return mValue;
            }
        }

        //-----------------------------------------------------------------------------
        public void Set(int value, AREntity entity)
        {
            if (!HasSetPermission(entity))
            {
                Debug.LogError("Error in SetProperty ::　this entity has no permission");
                return;
            }

            mValue = value;

            if (OnPropertyChanged != null)
            {
                OnPropertyChanged(this, null);
            }
        }

        //-----------------------------------------------------------------------------
        public object GetValue()
        {
            return mValue;
        }

        /// <summary>
        /// 检测是否具有修改数据的权限
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool HasSetPermission(AREntity entity)
        {
            return entity.EntityID == (int)EntityLib.PlayerPrefsEntity;
        }
    }// end class
}// end namespace