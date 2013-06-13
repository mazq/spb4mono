//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-07-04</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2012-07-04" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common.Repositories;

namespace Tunynet.Common
{
    //done:zhengw,by mazq
    //1、数据需要缓存，缓存策略为常用；
    //2、一次取出并缓存所有数据即可（更新时同时处理缓存），没必要再分SystemDataRepository.GetLongs()和GetDecimals()
    //zhengw回复：已修改
    /// <summary>
    /// 系统数据业务逻辑
    /// </summary>
    public class SystemDataService
    {
        private ISystemDataRepository systemDataRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public SystemDataService()
            : this(new SystemDataRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="systemDataRepository">系统数据仓储</param>
        public SystemDataService(ISystemDataRepository systemDataRepository)
        {
            this.systemDataRepository = systemDataRepository;
        }

        /// <summary>
        /// 变更系统数据
        /// </summary>
        /// <param name="dataKey">数据标识</param>
        /// <param name="number">待变更的数值</param>
        public void Change(string dataKey, long number)
        {
            //当DataKey不存在时，插入新数据
            //同时更新缓存
            systemDataRepository.Change(dataKey, number);
        }

        /// <summary>
        /// 变更系统数据
        /// </summary>
        /// <param name="dataKey">数据标识</param>
        /// <param name="number">待变更的数值</param>
        public void Change(string dataKey, decimal number)
        {
            //当DataKey不存在时，插入新数据
            //同时更新缓存
            systemDataRepository.Change(dataKey, number);
        }


        /// <summary>
        /// 获取DataKey对应的长整形
        /// </summary>
        /// <param name="dataKey">DataKey</param>
        /// <returns>dataKey不存在时返回0</returns>
        public long GetLong(string dataKey)
        {
            //把所有数据（数据量不大）一次读取并存入缓存，从中获取dataKey     
            IEnumerable<SystemData> systemDatas = systemDataRepository.GetAll();
            if (systemDatas == null)
                return default(long);
            SystemData systemData = systemDatas.FirstOrDefault(n => n.Datakey == dataKey);
            if (systemData == null)
                return default(long);
            return systemData.LongValue;
        }

        /// <summary>
        /// 获取DataKey对应的Decimal
        /// </summary>
        /// <param name="dataKey">DataKey</param>
        /// <returns>dataKey不存在时返回0</returns>
        public decimal GetDecimal(string dataKey)
        {
            IEnumerable<SystemData> systemDatas = systemDataRepository.GetAll();
            if (systemDatas == null)
                return default(decimal);
            SystemData systemData = systemDatas.FirstOrDefault(n => n.Datakey == dataKey);
            if (systemData == null)
                return default(decimal);          
            return systemData.DecimalValue;
        }
    }
}
