using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common.Repositories;
using Tunynet.Common;
using Spacebuilder.Common;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 微博ownerdatagetter
    /// </summary>
    public class MicroblogOwnerDataGetter : IOwnerDataGetter
    {
        /// <summary>
        /// datakey
        /// </summary>
        public string DataKey
        {
            get { return OwnerDataKeys.Instance().ThreadCount(); }
        }


        /// <summary>
        /// 名称
        /// </summary>
        public string DataName
        {
            get { return "微博数"; }
        }


        /// <summary>
        /// 获取链接地址
        /// </summary>
        /// <param name="spaceKey">用户名</param>
        /// <param name="ownerId">用户id</param>
        /// <returns></returns>
        public string GetDataUrl(string spaceKey, long? ownerId = null)
        {
            if (string.IsNullOrEmpty(spaceKey) && ownerId.HasValue)
                spaceKey = UserIdToUserNameDictionary.GetUserName(ownerId.Value);
            return SiteUrls.Instance().Mine(spaceKey, string.Empty);

 //           string tenantTypeId = TenantTypeIds.Instance().User();
 //           IEnumerable<string> dataKeys = OwnerDataSettings.GetDataKeys(tenantTypeId);
 //            List<OwnerStatisticData> list=new List<OwnerStatisticData>();
 //           foreach (var dataKey in dataKeys)
 //           {               
 //OwnerStatisticData data=new OwnerStatisticData();
 //               var getter= OwnerDataGetterFactory.Get(dataKey);
 //               getter.DataName;
 //               var value = new OwnerDataService().GetLong(ownerId, dataKey);
 //               getter.GetDataUrl
 //                   list.Add
 //           }
        }

        /// <summary>
        /// 应用Id
        /// </summary>
        public long ApplicationId
        {
            get {  return MicroblogConfig.Instance().ApplicationId;}
        }
    }
}