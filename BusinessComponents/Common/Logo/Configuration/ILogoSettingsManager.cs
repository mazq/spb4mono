//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-03-20</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2012-09-06" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common.Configuration
{

    /// <summary>
    /// LogoSettings管理器接口
    /// </summary>
    public interface ILogoSettingsManager
    {
        /// <summary>
        /// 获取LogoSettings
        /// </summary>
        /// <returns></returns>
        LogoSettings Get();

        /// <summary>
        /// 保存LogoSettings
        /// </summary>
        /// <returns></returns>
        void Save(LogoSettings logoSettings);
    }
}
