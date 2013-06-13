//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Common.Repositories;
using Tunynet.Utilities;

namespace Tunynet.Common
{
    /// <summary>
    /// ����ʵ����
    /// </summary>
    [TableName("tn_Areas")]
    [PrimaryKey("AreaCode", autoIncrement = false)]
    [CacheSetting(true)]
    [Serializable]
    public class Area : IEntity
    {
        /// <summary>
        /// �½�ʵ��ʱʹ��
        /// </summary>
        public static Area New()
        {
            Area area = new Area()
            {
                Name = string.Empty,
                PostCode = string.Empty
            };
            return area;
        }

        #region ��־û�����

        /// <summary>
        ///��������
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        ///������������
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        ///��������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///��������
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        ///�������
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///���
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        ///�ӵ�������
        /// </summary>
        public int ChildCount { get; set; }


        #endregion

        List<Area> children;
        /// <summary>
        /// �ӵ����б�
        /// </summary>
        [Ignore]
        public IEnumerable<Area> Children
        {
            get
            {
                if (this.children == null)
                    children = new List<Area>();
                return children.ToReadOnly();
            }
        }

        public void AppendChild(Area area)
        {
            if (children == null)
                children = new List<Area>();

            children.Add(area);
        }

        #region IEntity ��Ա

        object IEntity.EntityId { get { return this.AreaCode; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
