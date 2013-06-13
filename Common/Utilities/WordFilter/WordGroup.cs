//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 需要过滤文字的分组
    /// </summary>
    /// <remarks>用于装载一组具有同一个字开头的过滤词</remarks>
    class WordGroup
    {
        private List<WordEntity> _words;

        public WordGroup()
        {
            _words = new List<WordEntity>();
        }

        public void AppendWord(string word, WordFilterStatus wordFilterStatus, string replaceChar)
        {
            AppendWord(new WordEntity() { Word = word, WordFilterStatus = wordFilterStatus, Replacement = replaceChar });
        }

        public void AppendWord(WordEntity word)
        {
            _words.Add(word);
        }

        public int Count
        {
            get { return _words.Count; }
        }

        public WordEntity GetItem(int index)
        {
            return _words[index];
        }
    }
}
