//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Linq;
using System.Web.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// ModelStateDictionary扩展方法
    /// </summary>
    public static class ModelStateDictionaryExtension
    {

        /// <summary>
        /// 验证是否包含不允许提交的敏感词
        /// </summary>
        /// <param name="modelStateDictionary">modelStateDictionary</param>
        /// <returns>字符串类型的参数值</returns>
        public static bool HasBannedWord(this ModelStateDictionary modelStateDictionary)
        {
            string errorMessage = string.Empty;
            return HasBannedWord(modelStateDictionary, out errorMessage);
        }

        /// <summary>
        /// 验证是否包含不允许提交的敏感词
        /// </summary>
        /// <param name="modelStateDictionary">modelStateDictionary</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>字符串类型的参数值</returns>
        public static bool HasBannedWord(this ModelStateDictionary modelStateDictionary, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!modelStateDictionary.IsValid && modelStateDictionary.Keys.Contains("SensitiveWord"))
            {
                ModelState ms = new ModelState();
                modelStateDictionary.TryGetValue("SensitiveWord", out ms);

                if (ms != null && ms.Errors != null && ms.Errors.Count > 0)
                {
                    errorMessage = ms.Errors.FirstOrDefault().ErrorMessage;
                    return true;
                }
            }

            return false;
        }
    }
}