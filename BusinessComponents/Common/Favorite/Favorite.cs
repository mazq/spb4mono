//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// �û��ղ�ʵ����
    /// </summary>
    [TableName("tn_Favorites")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class FavoriteEntity : IEntity
    {
        /// <summary>
        /// �½�ʵ��ʱʹ��
        /// </summary>
        public static FavoriteEntity New()
        {
            FavoriteEntity favorite = new FavoriteEntity()
            {
                TenantTypeId = string.Empty

            };

            return favorite;
        }

        #region ��־û�����

        /// <summary>
        /// ��ʶ��
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///�⻧����Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///�ղ��û�Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///�ղض���Id
        /// </summary>
        public long ObjectId { get; set; }

        #endregion ��־û�����

        #region IEntity ��Ա

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}