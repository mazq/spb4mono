//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Common.Repositories;
using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// �����߼���
    /// </summary>
    public class SubscribeService
    {
        private IFavoriteRepository favoriteRepository;
        private string tenantTypeId;

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="tenantTypeId">�⻧����Id</param>
        public SubscribeService(string tenantTypeId)
            : this(tenantTypeId, new FavoriteRepository())
        {
            this.tenantTypeId = tenantTypeId;
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="tenantTypeId">�⻧����Id</param>
        /// <param name="favoriteRepository">�������ݷ���</param>
        public SubscribeService(string tenantTypeId, IFavoriteRepository favoriteRepository)
        {
            this.tenantTypeId = tenantTypeId;
            this.favoriteRepository = favoriteRepository;
        }

        /// <summary>
        /// ��Ӷ���
        /// </summary>
        /// <param name="objectId">�����Ķ���Id</param>
        /// <param name="userId">�û�Id</param>
        /// <returns>true-���ĳɹ�,false-����ʧ��</returns>
        public bool Subscribe(long objectId, long userId)
        {
            EventBus<long, SubscribeEventArgs>.Instance().OnBefore(objectId, new SubscribeEventArgs(EventOperationType.Instance().Create(), tenantTypeId, userId));
            bool result = favoriteRepository.Favorite(objectId, userId, tenantTypeId);
            EventBus<long, SubscribeEventArgs>.Instance().OnAfter(objectId, new SubscribeEventArgs(EventOperationType.Instance().Create(), tenantTypeId, userId));

            return result;
        }

        /// <summary>
        /// ȡ������
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="objectId">�����Ķ���Id</param>
        /// <returns>true-ȡ���ɹ�,false-ȡ��ʧ��</returns>
        public bool CancelSubscribe(long objectId, long userId)
        {
            EventBus<long, SubscribeEventArgs>.Instance().OnBefore(objectId, new SubscribeEventArgs(EventOperationType.Instance().Delete(), tenantTypeId, userId));
            bool result = favoriteRepository.CancelFavorited(objectId, userId, tenantTypeId);
            EventBus<long, SubscribeEventArgs>.Instance().OnAfter(objectId, new SubscribeEventArgs(EventOperationType.Instance().Delete(), tenantTypeId, userId));

            return result;
        }

        /// <summary>
        /// �ж��Ƿ���
        /// </summary>
        /// <param name="objectId">�����Ķ���Id</param>
        /// <param name="userId">�û�Id</param>
        /// <returns>true-�Ѷ��ģ�false-δ����</returns>
        public bool IsSubscribed(long objectId, long userId)
        {
            return favoriteRepository.IsFavorited(objectId, userId, tenantTypeId);
        }

        /// <summary>
        /// ��ȡ���Ķ���Id��ҳ����
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="pageIndex">ҳ��</param>
        /// <param name="pageSize">ÿҳ��ʾ������</param>
        ///<returns></returns>
        public PagingDataSet<long> GetPagingObjectIds(long userId, int pageIndex, int? pageSize = null)
        {
            return favoriteRepository.GetPagingObjectIds(userId, tenantTypeId, pageIndex, pageSize);
        }

        /// <summary>
        /// ��ȡǰN�����Ķ���Id
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="topNumber">Ҫ��ȡId�ĸ���</param>
        ///<returns></returns>
        public IEnumerable<long> GetTopObjectIds(long userId, int topNumber)
        {
            return favoriteRepository.GetTopObjectIds(userId, tenantTypeId, topNumber);
        }

        /// <summary>
        /// ��ȡȫ�����Ķ���Id
        /// </summary>
        /// <param name="userId">�û�Id</param>
        ///<returns></returns>
        public IEnumerable<long> GetAllObjectIds(long userId)
        {
            return favoriteRepository.GetAllObjectIds(userId, tenantTypeId);
        }

        /// <summary>
        /// ���ݶ��Ķ����ȡUserId
        /// </summary>
        /// <param name="objectId">���Ķ���Id</param>
        /// <returns></returns>
        public IEnumerable<long> GetUserIdsOfObject(long objectId)
        {
            return favoriteRepository.GetUserIdsOfObject(objectId, tenantTypeId);
        }

        /// <summary>
        /// ���ݶ��Ķ����ȡ�����˴˶����ǰN���û�Id����
        /// </summary>
        /// <param name="objectId">����Id</param>
        /// <param name="topNumber">Ҫ��ȡ��¼��</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopUserIdsOfObject(long objectId, int topNumber)
        {
            return favoriteRepository.GetTopUserIdsOfObject(objectId, tenantTypeId, topNumber);
        }

        /// <summary>
        /// ���ݶ��Ķ����ȡ�����˴˶�����û�Id��ҳ����
        /// </summary>
        /// <param name="objectId">����Id</param>
        /// <param name="pageIndex">ҳ��</param>
        /// <returns></returns>
        public PagingDataSet<long> GetPagingUserIdsOfObject(long objectId, int pageIndex)
        {
            return favoriteRepository.GetPagingUserIdsOfObject(objectId, tenantTypeId, pageIndex);
        }

        /// <summary>
        /// ���ݶ��Ķ����ȡͬ�����Ĵ˶�����ҵĹ�ע�û�
        /// </summary>
        /// <param name="objectId">����Id</param>
        /// <param name="userId">��ǰ�û���userId</param>
        /// <returns></returns>
        public IEnumerable<long> GetFollowedUserIdsOfObject(long objectId, long userId)
        {
            return favoriteRepository.GetFollowedUserIdsOfObject(objectId, userId, tenantTypeId);
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="objectId">���Ķ���Id</param>
        /// <returns></returns>
        public int GetSubscribedUserCount(long objectId)
        {
            return favoriteRepository.GetFavoritedUserCount(objectId, tenantTypeId);
        }
    }
}
