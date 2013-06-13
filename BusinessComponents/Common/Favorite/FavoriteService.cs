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
    /// �ղ��߼���
    /// </summary>
    public class FavoriteService
    {
        private IFavoriteRepository favoriteRepository;
        private string tenantTypeId;

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="tenantTypeId">�⻧����Id</param>
        public FavoriteService(string tenantTypeId)
            : this(tenantTypeId, new FavoriteRepository())
        {
            this.tenantTypeId = tenantTypeId;
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="tenantTypeId">�⻧����Id</param>
        /// <param name="favoriteRepository">�ղ����ݷ���</param>
        public FavoriteService(string tenantTypeId, IFavoriteRepository favoriteRepository)
        {
            this.tenantTypeId = tenantTypeId;
            this.favoriteRepository = favoriteRepository;
        }

        /// <summary>
        /// ����ղ�
        /// </summary>
        /// <param name="objectId">���ղض���Id</param>
        /// <param name="userId">�û�Id</param>
        /// <returns>true-�ղسɹ�,false-�ղ�ʧ��</returns>
        public bool Favorite(long objectId, long userId)
        {
            EventBus<long, FavoriteEventArgs>.Instance().OnBefore(objectId, new FavoriteEventArgs(EventOperationType.Instance().Create(), tenantTypeId, userId));
            bool result = favoriteRepository.Favorite(objectId, userId, tenantTypeId);
            EventBus<long, FavoriteEventArgs>.Instance().OnAfter(objectId, new FavoriteEventArgs(EventOperationType.Instance().Create(), tenantTypeId, userId));

            return result;
        }

        /// <summary>
        /// ȡ���ղ�
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="objectId">���ղض���Id</param>
        /// <returns>true-ȡ���ɹ�,false-ȡ��ʧ��</returns>
        public bool CancelFavorite(long objectId, long userId)
        {
            EventBus<long, FavoriteEventArgs>.Instance().OnBefore(objectId, new FavoriteEventArgs(EventOperationType.Instance().Create(), tenantTypeId, userId));
            bool result = favoriteRepository.CancelFavorited(objectId, userId, tenantTypeId);
            EventBus<long, FavoriteEventArgs>.Instance().OnAfter(objectId, new FavoriteEventArgs(EventOperationType.Instance().Create(), tenantTypeId, userId));

            return result;
        }

        /// <summary>
        /// �ж��Ƿ��ղ�
        /// </summary>
        /// <param name="objectId">���ղض���Id</param>
        /// <param name="userId">�û�Id</param>
        /// <returns>true-���ղأ�false-δ�ղ�</returns>
        public bool IsFavorited(long objectId, long userId)
        {
            return favoriteRepository.IsFavorited(objectId, userId, tenantTypeId);
        }

        /// <summary>
        /// ��ȡ�ղض���Id��ҳ����
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
        /// ��ȡǰN���ղض���Id
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="topNumber">Ҫ��ȡId�ĸ���</param>
        ///<returns></returns>
        public IEnumerable<long> GetTopObjectIds(long userId, int topNumber)
        {
            return favoriteRepository.GetTopObjectIds(userId, tenantTypeId, topNumber);
        }

        /// <summary>
        /// ��ȡȫ���ղض���Id
        /// </summary>
        /// <param name="userId">�û�Id</param>
        ///<returns></returns>
        public IEnumerable<long> GetAllObjectIds(long userId)
        {
            return favoriteRepository.GetAllObjectIds(userId, tenantTypeId);
        }

        /// <summary>
        /// �����ղض����ȡUserId
        /// </summary>
        /// <param name="objectId">�ղض���Id</param>
        /// <returns></returns>
        public IEnumerable<long> GetUserIdsOfObject(long objectId)
        {
            return favoriteRepository.GetUserIdsOfObject(objectId, tenantTypeId);
        }

        /// <summary>
        /// �����ղض����ȡ�ղ��˴˶����ǰN���û�Id����
        /// </summary>
        /// <param name="objectId">����Id</param>
        /// <param name="tenantTypeId">�⻧����Id</param>
        /// <param name="topNumber">Ҫ��ȡ��¼��</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopUserIdsOfObject(long objectId, int topNumber)
        {
            return favoriteRepository.GetTopUserIdsOfObject(objectId, tenantTypeId, topNumber);
        }

        /// <summary>
        /// �����ղض����ȡ�ղ��˴˶�����û�Id��ҳ����
        /// </summary>
        /// <param name="objectId">����Id</param>
        /// <param name="pageIndex">ҳ��</param>
        /// <returns></returns>
        public PagingDataSet<long> GetPagingUserIdsOfObject(long objectId, int pageIndex)
        {
            return favoriteRepository.GetPagingUserIdsOfObject(objectId, tenantTypeId, pageIndex);
        }

        /// <summary>
        /// �����ղض����ȡͬ���ղش˶���Ĺ�ע�û�
        /// </summary>
        /// <param name="objectId">����Id</param>
        /// <param name="userId">��ǰ�û���userId</param>
        /// <returns></returns>
        public IEnumerable<long> GetFollowedUserIdsOfObject(long objectId, long userId)
        {
            return favoriteRepository.GetFollowedUserIdsOfObject(objectId, userId, tenantTypeId);
        }

        /// <summary>
        /// ��ȡ���ղ���
        /// </summary>
        /// <param name="objectId">�ղض���Id</param>
        /// <returns></returns>
        public int GetFavoritedUserCount(long objectId)
        {
            return favoriteRepository.GetFavoritedUserCount(objectId, tenantTypeId);
        }
    }
}
