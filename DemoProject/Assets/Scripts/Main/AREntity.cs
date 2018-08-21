using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AngryRabbit
{
    /// <summary>
    /// 模块
    /// </summary>
    public class AREntity
    {
        private static AREntity arEntity;

        public static AREntity Instance
        {
            get
            {
                if (arEntity == null)
                    arEntity = new AREntity();

                return arEntity;
            }
        }

        public int EntityID;

        private static EntityMgr entityMgr;

        private static Dictionary<int, AREntity> entityDic = null;

        public static EntityMgr EntityMgr { get { return entityMgr; } }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="entity"></param>
        public void RegisterEntity(int entityID, AREntity entity)
        {
            if (entityDic == null)
                entityDic = new Dictionary<int, AREntity>();

            int key = entityID;

            if (!entityDic.ContainsValue(entity))
            {
                entity.EntityID = key;
                entityDic.Add(entity.EntityID, entity);
                //Debug.Log((EntityLib)entity.EntityID);
            }
            else
            {
                Debug.LogError("Error in RegisterEntity ::　this entity has already been registered");
            }

            entity.Init();
        }

        /// <summary>
        /// 保存模块管理器
        /// </summary>
        /// <param name="entity"></param>
        public void SetEntityMgr(EntityMgr entity)
        {
            entityMgr = entity;
            RegisterEntity((int)EntityLib.EntityMgr, entity);
        }

        /// <summary>
        /// 用于entity的初始化
        /// </summary>
        protected virtual void Init() { }

        /// <summary>
        /// 用于entity注册功能性消息(功能模块需要接收的消息需要在Init时进行注册)
        /// </summary>
        /// <param name="msg_id"></param>
        public virtual void RegisterMsg(int msg_id, AREntity entity)
        {
            entityMgr.RegisterMsg(msg_id, entity);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg_id"></param>
        /// <param name="paramList"></param>
        public virtual void SendMsg(int msg_id, List<object> paramList)
        {
            entityMgr.SendMsg(msg_id, paramList);
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="msg_id"></param>
        /// <param name="paramList"></param>
        public virtual void HandleMsg(int msg_id, List<object> paramList) { }

        /// <summary>
        /// 驱动所有子类的update
        /// </summary>
        public virtual void update()
        {
            if (entityDic != null && entityDic.Count > 0)
            {
                foreach (var entity in entityDic)
                {
                    entity.Value.update();
                }
            }
        }
    }//end class
}// end namespace

