//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace Spacebuilder.Common
{
    /// <summary>
    /// 字符过滤器类型Id
    /// </summary>
    public class WordFilterTypeIds
    {
        #region Instance

        private static WordFilterTypeIds _instance = new WordFilterTypeIds();

        /// <summary>
        /// 对象实例化方法
        /// </summary>
        /// <returns></returns>
        public static WordFilterTypeIds Instance()
        {
            return _instance;
        }

        private WordFilterTypeIds()
        { }

        #endregion

        /// <summary>
        /// 敏感词
        /// </summary>
        /// <returns></returns>
        public int SensitiveWord()
        {
            return 1;
        }

        /// <summary>
        /// 表情
        /// </summary>
        /// <returns></returns>
        public int Emotion()
        {
            return 2;
        }
    }
}
