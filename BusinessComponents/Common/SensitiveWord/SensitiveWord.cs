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
    /// ��д�ʵ����
    /// </summary>
    [TableName("tn_SensitiveWords")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "TypeId")]
    [Serializable]
    public class SensitiveWord : IEntity
    {
        /// <summary>
        /// �½�ʵ��ʱʹ��
        /// </summary>
        public static SensitiveWord New()
        {
            SensitiveWord sensitiveWord = new SensitiveWord()
            {
                Word = string.Empty,
                Replacement = string.Empty

            };

            return sensitiveWord;
        }

        #region ��־û�����

        /// <summary>
        ///Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ��д�
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// ��д�����Id
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// �滻����ַ�
        /// </summary>
        public string Replacement { get; set; }

        #endregion

        #region IEntity ��Ա

        object IEntity.EntityId
        {
            get
            {
                return this.Id;
            }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
