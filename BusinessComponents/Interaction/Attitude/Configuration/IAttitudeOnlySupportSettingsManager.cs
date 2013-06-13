using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// AttitudeOnlySupportSettings管理器接口
    /// </summary>
    public interface IAttitudeOnlySupportSettingsManager
    {
        /// <summary>
        /// 获取Attitude
        /// </summary>
        /// <returns></returns>
        AttitudeOnlySupportSettings Get();

        /// <summary>
        /// 保存Attitude
        /// </summary>
        /// <returns></returns>
        void Save(AttitudeOnlySupportSettings attitudeOnlySupportSettings);
    }
}
