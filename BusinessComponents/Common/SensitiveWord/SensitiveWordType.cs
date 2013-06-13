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
    /// ��д�����ʵ����
    /// </summary>
    [TableName("tn_SensitiveWordTypes")]
    [PrimaryKey("TypeId", autoIncrement = true)]
    [CacheSetting(true)]
    [Serializable]
    public class SensitiveWordType : IEntity
    {
        /// <summary>
        /// �½�ʵ��ʱʹ��
        /// </summary>
        public static SensitiveWordType New()
        {
            SensitiveWordType sensitiveWordType = new SensitiveWordType()
            {
                Name = string.Empty

            };

            return sensitiveWordType;
        }

        #region ��־û�����

        /// <summary>
        /// TypeId
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// ��д�������
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region IEntity ��Ա

        object IEntity.EntityId
        {
            get
            {
                return this.TypeId;
            }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
