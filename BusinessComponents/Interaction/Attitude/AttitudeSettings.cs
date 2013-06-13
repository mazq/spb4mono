using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;

namespace Tunynet.Common
{    /// <summary>
    /// 顶踩配置类
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class AttitudeSettings : IEntity
    {

        private float _supportWeights = 2;
        /// <summary>
        /// 顶操作统计的权重
        /// </summary>
        public float SupportWeights 
        {
            get { return _supportWeights; }
            set { _supportWeights = value; }
        }


        private float _opposeWeights = 1;
        /// <summary>
        /// 踩操作统计的权重
        /// </summary>
        public float OpposeWeights 
        { 
            get { return _opposeWeights; } 
            set { _opposeWeights = value; } 
        }


        private bool _enableCancel = false;
        /// <summary>
        /// 是否允许取消操作
        /// </summary>
        public bool EnableCancel
        {
            get { return _enableCancel; }
            set { _enableCancel = value; }
        }


        private bool _isModify = true;
        /// <summary>
        /// 是否允许修改操作
        /// </summary>
        public bool IsModify
        {
            get { return _isModify; }
            set { _isModify = value; }
        }


        #region IEntity 成员

        object IEntity.EntityId
        {
            get { return typeof(AttitudeSettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

    }
}