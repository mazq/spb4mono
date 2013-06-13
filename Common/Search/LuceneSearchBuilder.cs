//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace Spacebuilder.Search
{
    /// <summary>
    /// Lucene的Query、Filter、Sort构建器
    /// </summary>
    public class LuceneSearchBuilder
    {
        /// <summary>
        /// PhraseQuery的Slop与多元分词设置有关系
        /// </summary>
        private int PhraseQuerySlop = 10;

        private readonly List<BooleanClause> clauses;
        private readonly List<BooleanClause> filters;
        private readonly List<SortField> sortFields;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LuceneSearchBuilder(int slop = 10)
        {
            PhraseQuerySlop = slop;
            clauses = new List<BooleanClause>();
            filters = new List<BooleanClause>();
            sortFields = new List<SortField>();
        }

        /// <summary>
        /// 添加PhraseQuery
        /// </summary>
        /// <param name="fieldName">待搜索的字段名称</param>
        /// <param name="phrase">待搜索的短语</param>
        /// <param name="boostLevel">权重级别</param>
        /// <param name="asFilter">是否作为过滤条件</param>
        /// <returns>LuceneSearchBuilder</returns>
        public LuceneSearchBuilder WithPhrase(string fieldName, string phrase, BoostLevel? boostLevel = null, bool asFilter = false)
        {
            string filteredPhrase = ClauseScrubber.LuceneKeywordsScrub(phrase);
            if (string.IsNullOrEmpty(filteredPhrase))
                return this;

            if (filteredPhrase.Length == 1)
                return WithField(fieldName, filteredPhrase, false, boostLevel, asFilter);

            string[] nameSegments = ClauseScrubber.SegmentForPhraseQuery(filteredPhrase);

            PhraseQuery phraseQuery = new PhraseQuery();
            foreach (var nameSegment in nameSegments)
                phraseQuery.Add(new Term(fieldName, nameSegment));

            phraseQuery.SetSlop(PhraseQuerySlop);

            if (boostLevel.HasValue)
                SetBoost(phraseQuery, boostLevel.Value);

            if (asFilter)
                filters.Add(new BooleanClause(phraseQuery, BooleanClause.Occur.MUST));
            else
                clauses.Add(new BooleanClause(phraseQuery, BooleanClause.Occur.MUST));

            return this;
        }


        /// <summary>
        /// 根据多个关键字添加PhraseQuery
        /// </summary>
        /// <param name="fieldName">待搜索的字段名称</param>
        /// <param name="phrases">待搜索的短语列表</param>
        /// <param name="boostLevel">权重级别</param>
        /// <param name="asFilter">是否作为过滤条件</param>
        /// <returns>LuceneSearchBuilder</returns>
        public LuceneSearchBuilder WithPhrases(string fieldName, IEnumerable<string> phrases, BoostLevel? boostLevel = null, bool asFilter = false)
        {
            BooleanQuery query = new BooleanQuery();

            foreach (string phrase in phrases)
            {
                string filteredPhrase = ClauseScrubber.LuceneKeywordsScrub(phrase);
                if (string.IsNullOrEmpty(filteredPhrase))
                    continue;

                if (filteredPhrase.Length == 1)
                {
                    Term term = new Term(fieldName, filteredPhrase);
                    Query q = new PrefixQuery(term);

                    if (boostLevel.HasValue)
                        SetBoost(q, boostLevel.Value);

                    query.Add(q, BooleanClause.Occur.SHOULD);

                    continue;
                }

                string[] nameSegments = ClauseScrubber.SegmentForPhraseQuery(filteredPhrase);

                PhraseQuery phraseQuery = new PhraseQuery();
                foreach (var nameSegment in nameSegments)
                    phraseQuery.Add(new Term(fieldName, nameSegment));

                phraseQuery.SetSlop(PhraseQuerySlop);

                if (boostLevel.HasValue)
                    SetBoost(phraseQuery, boostLevel.Value);

                query.Add(phraseQuery, BooleanClause.Occur.SHOULD);

            }


            if (asFilter)
                filters.Add(new BooleanClause(query, BooleanClause.Occur.MUST));
            else
                clauses.Add(new BooleanClause(query, BooleanClause.Occur.MUST));

            return this;
        }

        /// <summary>
        /// 批量添加PhraseQuery
        /// </summary>
        /// <param name="phrase">待搜索的短语</param>
        /// <param name="fieldNameAndBoosts">字段名称及权重集合</param>
        /// <param name="occur">搜索条件间的关系</param>
        /// <param name="asFilter">是否作为过滤器条件</param>
        /// <returns></returns>
        public LuceneSearchBuilder WithPhrases(Dictionary<string, BoostLevel> fieldNameAndBoosts, string phrase, BooleanClause.Occur occur, bool asFilter = false)
        {
            string filteredPhrase = ClauseScrubber.LuceneKeywordsScrub(phrase);
            if (string.IsNullOrEmpty(filteredPhrase))
                return this;

            string[] nameSegments = ClauseScrubber.SegmentForPhraseQuery(filteredPhrase);
            if (nameSegments.Length == 1)
            {
                return WithFields(fieldNameAndBoosts, nameSegments[0], false, occur, asFilter);
            }
            else
            {
                BooleanQuery query = new BooleanQuery();
                foreach (var fieldNameAndBoost in fieldNameAndBoosts)
                {
                    PhraseQuery phraseQuery = new PhraseQuery();
                    foreach (var nameSegment in nameSegments)
                        phraseQuery.Add(new Term(fieldNameAndBoost.Key, nameSegment));

                    phraseQuery.SetSlop(PhraseQuerySlop);
                    SetBoost(phraseQuery, fieldNameAndBoost.Value);
                    query.Add(phraseQuery, occur);
                }

                if (asFilter)
                    filters.Add(new BooleanClause(query, BooleanClause.Occur.MUST));
                else
                    clauses.Add(new BooleanClause(query, BooleanClause.Occur.MUST));

                return this;
            }
        }

        /// <summary>
        /// 批量添加PhraseQuery
        /// </summary>
        /// <param name="phrases">待搜索的短语集合</param>
        /// <param name="fieldNameAndBoosts">字段名称及权重集合</param>
        /// <param name="occur">搜索条件间的关系</param>
        /// <param name="asFilter">是否作为过滤器条件</param>
        /// <returns></returns>
        public LuceneSearchBuilder WithPhrases(Dictionary<string, BoostLevel> fieldNameAndBoosts, IEnumerable<string> phrases, BooleanClause.Occur occur, bool asFilter = false)
        {
            foreach (var fieldNameAndBoost in fieldNameAndBoosts)
            {
                BooleanQuery query = new BooleanQuery();
                foreach (string phrase in phrases)
                {
                    string filteredPhrase = ClauseScrubber.LuceneKeywordsScrub(phrase);
                    if (string.IsNullOrEmpty(filteredPhrase))
                        continue;

                    if (filteredPhrase.Length == 1)
                    {
                        Term term = new Term(fieldNameAndBoost.Key, filteredPhrase);
                        Query q = new PrefixQuery(term);

                        SetBoost(q, fieldNameAndBoost.Value);

                        query.Add(q, BooleanClause.Occur.SHOULD);

                        continue;
                    }

                    string[] nameSegments = ClauseScrubber.SegmentForPhraseQuery(filteredPhrase);

                    PhraseQuery phraseQuery = new PhraseQuery();
                    foreach (var nameSegment in nameSegments)
                        phraseQuery.Add(new Term(fieldNameAndBoost.Key, nameSegment));

                    phraseQuery.SetSlop(PhraseQuerySlop);

                    SetBoost(phraseQuery, fieldNameAndBoost.Value);

                    query.Add(phraseQuery, BooleanClause.Occur.SHOULD);

                }

                if (asFilter)
                    filters.Add(new BooleanClause(query, occur));
                else
                    clauses.Add(new BooleanClause(query, occur));
            }

            return this;
        }


        /// <summary>
        /// 添加TermQuery或PrefixQuery搜索条件
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="value">字段值</param>
        /// <param name="exactMatch">是否精确搜索</param>
        /// <param name="boostLevel">权重级别</param>
        /// <param name="asFilter">是否作为过滤条件</param>
        /// <returns>LuceneSearchBuilder</returns>
        public LuceneSearchBuilder WithField(string fieldName, string value, bool exactMatch = true, BoostLevel? boostLevel = null, bool asFilter = false)
        {
            string filteredValue = ClauseScrubber.LuceneKeywordsScrub(value);
            if (string.IsNullOrEmpty(filteredValue))
                return this;

            Term term = new Term(fieldName, filteredValue);
            Query query;
            if (exactMatch)
                query = new TermQuery(term);
            else
                query = new PrefixQuery(term);

            if (boostLevel.HasValue)
                SetBoost(query, boostLevel.Value);

            if (asFilter)
                filters.Add(new BooleanClause(query, BooleanClause.Occur.MUST));
            else
                clauses.Add(new BooleanClause(query, BooleanClause.Occur.MUST));

            return this;
        }

        /// <summary>
        /// 根据多个关键字添加TermQuery或PrefixQuery搜索条件
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="values">字段值列表</param>
        /// <param name="exactMatch">是否精确搜索</param>
        /// <param name="boostLevel">权重级别</param>
        /// <param name="asFilter">是否作为过滤条件</param>
        public LuceneSearchBuilder WithFields(string fieldName, IEnumerable<string> values, bool exactMatch = true, BoostLevel? boostLevel = null, bool asFilter = false)
        {
            BooleanQuery query = new BooleanQuery();

            foreach (string value in values)
            {
                string filteredValue = ClauseScrubber.LuceneKeywordsScrub(value);
                if (string.IsNullOrEmpty(filteredValue))
                    continue;

                Term term = new Term(fieldName, filteredValue);
                Query q;
                if (exactMatch)
                    q = new TermQuery(term);
                else
                    q = new PrefixQuery(term);

                if (boostLevel.HasValue)
                    SetBoost(q, boostLevel.Value);

                query.Add(q, BooleanClause.Occur.SHOULD);
            }

            if (asFilter)
                filters.Add(new BooleanClause(query, BooleanClause.Occur.MUST));
            else
                clauses.Add(new BooleanClause(query, BooleanClause.Occur.MUST));
            return this;
        }

        /// <summary>
        /// 添加TermQuery或PrefixQuery搜索条件
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="value">字段值</param>
        /// <param name="exactMatch">是否精确搜索</param>
        /// <param name="boostLevel">权重级别</param>
        /// <param name="asFilter">是否作为过滤条件</param>
        /// <returns>LuceneSearchBuilder</returns>
        public LuceneSearchBuilder WithField(string fieldName, bool value, bool exactMatch = true, BoostLevel? boostLevel = null, bool asFilter = false)
        {
            return WithField(fieldName, value ? 1 : 0, exactMatch, boostLevel, asFilter);
        }

        /// <summary>
        /// 添加TermQuery或PrefixQuery搜索条件
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="value">字段值</param>
        /// <param name="exactMatch">是否精确搜索</param>
        /// <param name="boostLevel">权重级别</param>
        /// <param name="asFilter">是否作为过滤条件</param>
        /// <returns>LuceneSearchBuilder</returns>
        public LuceneSearchBuilder WithField(string fieldName, int value, bool exactMatch = true, BoostLevel? boostLevel = null, bool asFilter = false)
        {
            return WithField(fieldName, value.ToString(), exactMatch, boostLevel, asFilter);
        }

        /// <summary>
        /// 批量添加TermQuery或PrefixQuery搜索条件
        /// </summary>
        /// <param name="fieldNameAndBoosts">字段名称及权重集合</param>
        /// <param name="value">字段值</param>
        /// <param name="exactMatch">是否精确搜索</param>
        /// <param name="occur">搜索条件间的关系</param>
        /// <param name="asFilter">是否作为过滤条件</param>
        /// <returns>LuceneSearchBuilder</returns>
        public LuceneSearchBuilder WithFields(Dictionary<string, BoostLevel> fieldNameAndBoosts, string value, bool exactMatch, BooleanClause.Occur occur, bool asFilter = false)
        {
            string filteredValue = ClauseScrubber.LuceneKeywordsScrub(value);
            if (string.IsNullOrEmpty(filteredValue))
                return this;

            BooleanQuery query = new BooleanQuery();

            foreach (var fieldNameAndBoost in fieldNameAndBoosts)
            {
                Term term = new Term(fieldNameAndBoost.Key, filteredValue);
                Query q;
                if (exactMatch)
                    q = new TermQuery(term);
                else
                    q = new PrefixQuery(term);

                SetBoost(q, fieldNameAndBoost.Value);

                query.Add(q, occur);
            }

            if (asFilter)
                filters.Add(new BooleanClause(query, BooleanClause.Occur.MUST));
            else
                clauses.Add(new BooleanClause(query, BooleanClause.Occur.MUST));

            return this;
        }

        /// <summary>
        /// 批量添加TermQuery或PrefixQuery搜索条件
        /// </summary>
        /// <param name="fieldNameAndBoosts">字段名称及权重集合</param>
        /// <param name="values">字段值集合</param>
        /// <param name="exactMatch">是否精确搜索</param>
        /// <param name="occur">搜索条件间的关系</param>
        /// <param name="asFilter">是否作为过滤条件</param>
        /// <returns>LuceneSearchBuilder</returns>
        public LuceneSearchBuilder WithFields(Dictionary<string, BoostLevel> fieldNameAndBoosts, IEnumerable<string> values, bool exactMatch, BooleanClause.Occur occur, bool asFilter = false)
        {
            foreach (var fieldNameAndBoost in fieldNameAndBoosts)
            {
                BooleanQuery query = new BooleanQuery();

                foreach (string value in values)
                {
                    string filteredValue = ClauseScrubber.LuceneKeywordsScrub(value);
                    if (string.IsNullOrEmpty(filteredValue))
                        continue;

                    Term term = new Term(fieldNameAndBoost.Key, filteredValue);
                    Query q;
                    if (exactMatch)
                        q = new TermQuery(term);
                    else
                        q = new PrefixQuery(term);

                    SetBoost(q, fieldNameAndBoost.Value);

                    query.Add(q, BooleanClause.Occur.SHOULD);
                }

                if (asFilter)
                    filters.Add(new BooleanClause(query, occur));
                else
                    clauses.Add(new BooleanClause(query, occur));
            }

            return this;
        }

        /// <summary>
        /// 添加TermQuery搜索排除条件
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="value">字段值</param>>
        /// <returns>LuceneSearchBuilder</returns>
        public LuceneSearchBuilder NotWithField(string fieldName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }

            Query query = new TermQuery(new Term(fieldName, value));

            clauses.Add(new BooleanClause(query, BooleanClause.Occur.MUST_NOT));

            return this;
        }

        /// <summary>
        /// 批量添加TermQuery搜索排除条件
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="values">字段值列表</param>
        /// <returns>LuceneSearchBuilder</returns>
        public LuceneSearchBuilder NotWithFields(string fieldName, IEnumerable<string> values)
        {
            if (values == null || values.Count() == 0)
            {
                return this;
            }

            foreach (string value in values)
            {
                Query query = new TermQuery(new Term(fieldName, value));
                clauses.Add(new BooleanClause(query, BooleanClause.Occur.MUST_NOT));
            }

            return this;
        }

        /// <summary>
        /// 添加数字类型的RangeQuery.
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值 </param>
        /// <returns>LuceneSearchBuilder</returns>
        public LuceneSearchBuilder WithinRange(string fieldName, int min, int max, bool asFilter = false)
        {
            Query query = NumericRangeQuery.NewIntRange(fieldName, min, max, true, true);

            if (asFilter)
                filters.Add(new BooleanClause(query, BooleanClause.Occur.MUST));
            else
                clauses.Add(new BooleanClause(query, BooleanClause.Occur.MUST));

            return this;
        }

        /// <summary>
        /// 添加字符串类型的RangeQuery.
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值 </param>
        /// <returns>LuceneSearchBuilder</returns>
        public LuceneSearchBuilder WithinRange(string fieldName, string min, string max, bool asFilter = false)
        {
            Query query = new TermRangeQuery(fieldName, min, max, true, true);

            if (asFilter)
                filters.Add(new BooleanClause(query, BooleanClause.Occur.MUST));
            else
                clauses.Add(new BooleanClause(query, BooleanClause.Occur.MUST));

            return this;
        }

        /// <summary>
        /// 按整型排序
        /// </summary>
        /// <param name="fieldName">排序字段名称</param>
        /// <param name="sortDescending">是否按倒序排列</param>
        /// <returns>LuceneSearchBuilder</returns>
        public LuceneSearchBuilder SortByInteger(string fieldName, bool sortDescending = false)
        {
            sortFields.Add(new SortField(fieldName, SortField.INT, sortDescending));
            return this;
        }

        /// <summary>
        /// 按字符串排序
        /// </summary>
        /// <param name="fieldName">排序字段名称</param>
        /// <param name="sortDescending">是否按倒序排列</param>
        /// <returns>LuceneSearchBuilder</returns>
        public LuceneSearchBuilder SortByString(string fieldName, bool sortDescending = false)
        {
            sortFields.Add(new SortField(fieldName, SortField.STRING, sortDescending));
            return this;
        }

        /// <summary>
        /// 构建Query、Filter、Sort
        /// </summary>
        /// <param name="query"><see cref="Query"/></param>
        /// <param name="filter"><see cref="Filter"/></param>
        /// <param name="sort"><see cref="Sort"/></param>
        public void BuildQuery(out Query query, out Filter filter, out Sort sort)
        {
            BooleanQuery q = new BooleanQuery();
            foreach (var clause in clauses)
            {
                q.Add(clause);
            }
            query = q;

            if (filters.Count > 0)
            {
                BooleanQuery filterQuery = new BooleanQuery();
                foreach (var _filter in filters)
                    filterQuery.Add(_filter);

                filter = new QueryWrapperFilter(filterQuery);
            }
            else
            {
                filter = null;
            }

            if (sortFields.Count > 0)
                sort = new Sort(sortFields.ToArray());
            else
                sort = null;
        }


        /// <summary>
        ///  设置Query权重
        /// </summary>
        /// <param name="query"></param>
        /// <param name="boostLevel"></param>
        private void SetBoost(Query query, BoostLevel boostLevel)
        {
            query.SetBoost((float)boostLevel);
        }

    }

    /// <summary>
    /// 权重级别
    /// </summary>
    public enum BoostLevel
    {
        /// <summary>
        /// 高 Math.Pow(3, 5)
        /// </summary>
        Hight = 243,

        /// <summary>
        /// 中 Math.Pow(3, 3)
        /// </summary>
        Medium = 27,

        /// <summary>
        /// 低 Math.Pow(3, 1)
        /// </summary>
        Low = 3
    }




}
