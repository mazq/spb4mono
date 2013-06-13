//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.IO;
using System.Threading;
using System.Xml.Serialization;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Repositories;
using System.Web.Helpers;

namespace Spacebuilder.Common.Repositories
{
    /// <summary>
    /// 设置Repository
    /// </summary>
    /// <typeparam name="TSettingsEntity">设置的实体类</typeparam>
    public class SettingsRepository<TSettingsEntity> : ISettingsRepository<TSettingsEntity> where TSettingsEntity : class, IEntity, new()
    {
        private static ReaderWriterLockSlim RWLock = new System.Threading.ReaderWriterLockSlim();

        // 缓存服务
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 缓存设置
        /// </summary>
        protected static RealTimeCacheHelper RealTimeCacheHelper { get { return EntityData.ForType(typeof(TSettingsEntity)).RealTimeCacheHelper; } }

        /// <summary>
        /// 默认PetaPocoDatabase实例
        /// </summary>
        private PetaPocoDatabase CreateDAO()
        {
            return PetaPocoDatabase.CreateInstance();
        }

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <returns>settings</returns>
        public TSettingsEntity Get()
        {
            string classType = GetClassType();

            TSettingsEntity result = cacheService.Get(RealTimeCacheHelper.GetCacheKeyOfEntity(classType)) as TSettingsEntity;

            if (result == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("Settings").From("tn_Settings").Where("ClassType=@0", classType);

                string settingsXml = CreateDAO().FirstOrDefault<string>(sql);

                if (settingsXml == null)
                {
                    result = new TSettingsEntity();
                    Save(result);
                }
                else
                {
                    result = Deserialize(settingsXml);
                }

                cacheService.Add(RealTimeCacheHelper.GetCacheKeyOfEntity(classType), result, CachingExpirationType.RelativelyStable);
            }

            return result;
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="settings">settings</param>
        public void Save(TSettingsEntity settings)
        {
            string classType = GetClassType();
            string settingsXml = Serialize(settings);

            var sql = PetaPoco.Sql.Builder;
            sql.Select("count(ClassType)").From("tn_Settings").Where("ClassType=@0", classType);

            RWLock.EnterWriteLock();

            PetaPocoDatabase database = CreateDAO();
            try
            {
                database.OpenSharedConnection();

                int count = database.ExecuteScalar<int>(sql);
                sql = PetaPoco.Sql.Builder;
                if (count > 0)
                    sql.Append("update tn_Settings set Settings=@0 where ClassType=@1", settingsXml, classType);
                else
                    sql.Append("insert into tn_Settings (ClassType,Settings) values (@0,@1)", classType, settingsXml);

                database.Execute(sql);
            }
            finally
            {
                database.CloseSharedConnection();
                RWLock.ExitWriteLock();
            }
            cacheService.Set(RealTimeCacheHelper.GetCacheKeyOfEntity(classType), settings, CachingExpirationType.RelativelyStable);
            RealTimeCacheHelper.IncreaseEntityCacheVersion(classType);
        }

        /// <summary>
        /// 获取ClassType
        /// </summary>
        /// <returns></returns>
        private string GetClassType()
        {
            string[] parts = typeof(TSettingsEntity).AssemblyQualifiedName.Split(',');
            return parts[0] + "," + parts[1];
        }

        /// <summary>
        /// 把TSettingsEntity对象转换成xml
        /// </summary>
        /// <param name="settingsEntity">被转换的对象</param>
        /// <returns>序列化后的xml字符串</returns>
        private string Serialize(TSettingsEntity settingsEntity)
        {
            string json = null;

            if (settingsEntity != null)
            {
                json = Json.Encode(settingsEntity);
            }
            return json;
        }


        /// <summary>
        /// 把json的字符串反序列化成SettingsEntity对象
        /// </summary>
        /// <param name="json">被反序列化的xml字符串</param>
        /// <returns>反序列化后的SettingsEntity</returns>
        private TSettingsEntity Deserialize(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                return Json.Decode<TSettingsEntity>(json);
            }
            return null;
        }
    }
}