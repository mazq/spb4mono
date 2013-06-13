using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// AttitudeSettings管理器接口
    /// </summary>
    public interface IAttitudeSettingsManager
    {
        /// <summary>
        /// 获取Attitude
        /// </summary>
        /// <returns></returns>
        AttitudeSettings Get();

        /// <summary>
        /// 保存Attitude
        /// </summary>
        /// <returns></returns>
        void Save(AttitudeSettings attitudeSettings);
    }
}
