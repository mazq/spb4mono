//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 字符过滤器
    /// </summary>
    public class WordFilter
    {
        private static ConcurrentDictionary<int, WordFilter> registeredFilters = new ConcurrentDictionary<int, WordFilter>();
        private static object lockObject = new object();
        private WordGroup[] _wordTable;
        private List<WordEntity> _regexWords;

        /// <summary>
        /// 私有构造器
        /// </summary>
        /// <param name="wordDict">敏感词集合(key-敏感词,value-替换字符)</param>
        private WordFilter(Dictionary<string, string> wordDict)
        {
            _wordTable = new WordGroup[(int)char.MaxValue];
            _regexWords = new List<WordEntity>();
            Initialization(wordDict);
        }

        /// <summary>
        /// 类初始化
        /// </summary>
        /// <param name="wordDict">敏感词集合(key-敏感词,value-替换字符)</param>
        private void Initialization(Dictionary<string, string> wordDict)
        {
            Array.Clear(_wordTable, 0, _wordTable.Length);

            List<WordEntity> wordList = new List<WordEntity>();
            WordFilterStatus status;

            foreach (var pair in wordDict)
            {
                StringBuilder wordBuilder = new StringBuilder();
                string tempWord = pair.Key;
                status = WordFilterStatus.Replace;

                string replacement = string.IsNullOrEmpty(pair.Value) ? "*" : pair.Value;
                if (replacement == "{Banned}") status = WordFilterStatus.Banned;

                if (pair.Key.StartsWith("\""))
                {
                    //转换成繁体再加载一个
                    WordEntity regexWord = new WordEntity()
                    {
                        Word = pair.Key.Trim('\"'),
                        Replacement = replacement,
                        WordFilterStatus = status
                    };

                    _regexWords.Add(regexWord);

                    continue;
                }

                tempWord = IgnoreCharacterProcess(pair.Key);

                for (int i = 0; i < tempWord.Length; i++)
                {
                    wordBuilder.Append((char)FastToLower(tempWord[i])); //统一转换成小写
                }

                //添加到列表
                WordEntity simpleWord = new WordEntity()
                {
                    Word = wordBuilder.ToString(),
                    Replacement = replacement,
                    WordFilterStatus = status
                };

                wordList.Add(simpleWord);
            }

            _regexWords = _regexWords.OrderByDescending(n => n.WordFilterStatus).ToList();

            //去除重复
            Comparison<WordEntity> cmp = delegate(WordEntity a, WordEntity b)
            {
                return a.Word.CompareTo(b.Word);
            };

            wordList.Sort(cmp);
            for (int idx = wordList.Count - 1; idx > 0; idx--)
            {
                if (wordList[idx].Word == wordList[idx - 1].Word)
                {
                    wordList.RemoveAt(idx);
                }
            }

            //添加到字典
            foreach (WordEntity entity in wordList)
            {
                WordGroup group = _wordTable[(int)entity.Word[0]];

                if (group == null)
                {
                    group = new WordGroup();
                    group.AppendWord(entity.Word.Substring(1), entity.WordFilterStatus, entity.Replacement);
                    _wordTable[(int)entity.Word[0]] = group;
                }
                else
                {
                    group.AppendWord(entity.Word.Substring(1), entity.WordFilterStatus, entity.Replacement);
                }
            }
        }

        /// <summary>
        /// 过滤器实例
        /// </summary>
        /// <param name="filterTypeId">过滤器类型Id</param>
        public static WordFilter Instance(int filterTypeId)
        {
            if (!registeredFilters.ContainsKey(filterTypeId) || registeredFilters[filterTypeId] == null)
            {
                lock (lockObject)
                {
                    if (!registeredFilters.ContainsKey(filterTypeId) || registeredFilters[filterTypeId] == null)
                    {
                        registeredFilters[filterTypeId] = new WordFilter(new Dictionary<string, string>());
                    }
                }
            }

            return registeredFilters[filterTypeId];
        }

        #region Filters

        /// <summary>
        /// 用于敏感词的过滤器
        /// </summary>
        public static WordFilter SensitiveWordFilter
        {
            get
            {
                if (WordFilter.Instance(WordFilterTypeIds.Instance().SensitiveWord()) != null)
                    return WordFilter.Instance(WordFilterTypeIds.Instance().SensitiveWord());

                return new WordFilter(new Dictionary<string, string>()); ;
            }
        }

        /// <summary>
        /// 用于表情的过滤器
        /// </summary>
        public static WordFilter EmotionFilter
        {
            get
            {
                if (WordFilter.Instance(WordFilterTypeIds.Instance().Emotion()) != null)
                    return WordFilter.Instance(WordFilterTypeIds.Instance().Emotion());

                return new WordFilter(new Dictionary<string, string>()); ;
            }
        }

        #endregion

        #region Add/Remove WordFilter

        /// <summary>
        /// 添加过滤器
        /// </summary>
        /// <param name="wordFilterTypeId">过滤器类型</param>
        /// <param name="wordDict">敏感词集合</param>
        /// <returns>key-待处理字符，value-用来替换的字符</returns>
        public static void Add(int wordFilterTypeId, Dictionary<string, string> wordDict)
        {
            if (wordDict == null)
                return;

            registeredFilters[wordFilterTypeId] = new WordFilter(wordDict);
        }

        /// <summary>
        /// 删除过滤器
        /// </summary>
        /// <param name="wordFilterTypeId">过滤器类型Id</param>
        public static void Remove(int wordFilterTypeId)
        {
            WordFilter filter;
            registeredFilters.TryRemove(wordFilterTypeId, out filter);
        }

        #endregion

        #region Filter/Find

        /// <summary>
        /// 测试字符串是否出现屏蔽字
        /// </summary>
        /// <param name="source">需要处理的字符串</param>
        /// <returns>最终处理方式</returns>
        public string Filter(string source)
        {
            WordFilterStatus status = WordFilterStatus.Replace;
            return Filter(source, out status);
        }

        /// <summary>
        /// 测试字符串是否出现屏蔽字
        /// </summary>
        /// <param name="source">需要处理的字符串</param>
        /// <param name="status">敏感词处理状态</param>
        /// <returns>最终处理方式</returns>
        public string Filter(string source, out WordFilterStatus status)
        {
            status = WordFilterStatus.Replace;

            if (String.IsNullOrEmpty(source)) return source;

            StringBuilder sb = new StringBuilder();

            for (int start = 0; start < source.Length; start++)
            {
                WordGroup wordGroup = _wordTable[FastToLower(source[start])];

                if (wordGroup != null)
                {
                    bool found = false;

                    for (int idx = 0; idx < wordGroup.Count; idx++)
                    {
                        WordEntity we = wordGroup.GetItem(idx);

                        int matchLength = 0;
                        if (we.Word.Length == 0 || CheckString(source, we.Word, start + 1, out matchLength))
                        {
                            if (we.WordFilterStatus == WordFilterStatus.Banned)
                            {
                                status = we.WordFilterStatus;
                                break;
                            }

                            found = true;
                            sb.Append(we.Replacement);
                            start += matchLength;
                        }
                    }

                    if (status == WordFilterStatus.Banned)
                        break;

                    if (!found)
                    {
                        sb.Append(source[start]);
                        found = false;
                    }
                }
                else
                {
                    sb.Append(source[start]);
                }
            }

            if (status == WordFilterStatus.Replace)
            {
                source = sb.ToString();
                source = RegexFilter(source, out status);
            }

            return source;
        }

        #endregion

        #region Helper Method

        /// <summary>
        /// 处理需要跳过字符
        /// </summary>
        /// <param name="word">要处理敏感词</param>
        private string IgnoreCharacterProcess(string word)
        {
            if (!word.Contains("{"))
                return word;

            int startPos = 0, endPos = 0;

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == '{')
                {
                    startPos = i;
                }
                else if (word[i] == '}')
                {
                    endPos = i;
                }

                if (endPos > startPos)
                {
                    int ignoreCount = 0;
                    int.TryParse(word.Substring(startPos + 1, endPos - startPos - 1), out ignoreCount);

                    if (ignoreCount > 0)
                    {
                        word = word.Replace("{" + ignoreCount + "}", new String('*', ignoreCount));
                        i -= ignoreCount.ToString().Length + 1;
                        ignoreCount = 0;
                    }

                    startPos = 0;
                    endPos = 0;
                }
            }

            return word;
        }

        /// <summary>
        /// 检查字符串是否匹配
        /// </summary>
        /// <param name="source">需要处理的字符串</param>
        /// <param name="word">敏感词</param>
        /// <param name="sourceStart">当前处理的位置</param>
        /// <param name="matchLength">长度</param>
        /// <returns>是否匹配</returns>
        private bool CheckString(string source, string word, int sourceStart, out int matchLength)
        {
            bool found = false, skip = false;
            int sourceOffset = 0, subOffset = 0;

            for (int keyIndex = 0; keyIndex < word.Length; keyIndex++)
            {
                if (sourceOffset + sourceStart >= source.Length) break; //原始字符串已经全部搜索完毕

                if (word[keyIndex] == '*')
                {
                    sourceOffset++;
                    skip = true;
                }
                else
                {
                    if (skip)
                    {
                        for (; subOffset <= sourceOffset; subOffset++)
                        {
                            //比较字母
                            if (FastToLower(source[subOffset + sourceStart]) == (int)word[keyIndex])
                            {
                                if (keyIndex == word.Length - 1)
                                {
                                    sourceOffset = subOffset;
                                    found = true;
                                    break;
                                }
                                else
                                {
                                    sourceOffset = subOffset;
                                    break;
                                }
                            }

                            if (subOffset + 1 > sourceOffset)
                            {
                                subOffset++;
                                sourceOffset++;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (FastToLower(source[sourceOffset + sourceStart]) == (int)word[keyIndex])
                        {
                            if (keyIndex == word.Length - 1)
                            {
                                found = true;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }

                        sourceOffset++;//移动原始字符串
                    }
                }
            }

            //如果匹配中关键字，则返回原字符串中被匹配中的文字的长度，否则返回0
            matchLength = sourceOffset + 1;
            return found;
        }

        /// <summary>
        /// 使用正则过滤敏感词
        /// </summary>
        /// <param name="source">待处理字符串</param>
        /// <param name="status">字符处理状态</param>
        private string RegexFilter(string source, out WordFilterStatus status)
        {
            string replaceStr = source;
            status = WordFilterStatus.Replace;

            foreach (var word in _regexWords)
            {
                if (Regex.IsMatch(replaceStr, word.Word))
                {
                    if (word.WordFilterStatus == WordFilterStatus.Banned)
                    {
                        status = word.WordFilterStatus;
                        return source;
                    }
                    replaceStr = Regex.Replace(replaceStr, word.Word, word.Replacement);
                }
            }

            status = WordFilterStatus.Replace;
            return replaceStr;
        }

        /// <summary>
        /// 将大写字母转换为小写
        /// </summary>
        /// <param name="character">需要处理的字符</param>
        /// <returns>处理后的小写字母</returns>
        private int FastToLower(char character)
        {
            //将大写英文字母以及全/半角的英文字母转化为小写字母
            int charVal = (int)character;
            if (charVal <= 90)
            {
                if (charVal >= 65) //字母A-Z
                    return charVal - 65 + 97;
            }
            else if (charVal >= 65313)
            {
                if (charVal <= 65338)
                    return charVal - 65313 + 97; //全角大写A-Z
                else if (charVal >= 65345 && charVal <= 65370)
                    return charVal - 65345 + 97; //全角小写a-z
            }
            return charVal;
        }

        #endregion
    }
}
