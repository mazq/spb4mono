//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PanGu;
using System.Globalization;

namespace Spacebuilder.Search
{
    /// <summary>
    /// 关键字过滤/解析工具类
    /// </summary>
    public class ClauseScrubber
    {
        private static readonly string[] LuceneKeywords = new string[] { @"\", "+", "-", "(", ")", ":", "^", "[", "]", "{", "}", "~", "*", "?", "!", "\"", "'" };

        private static readonly string[] AreasWithFullName = new string[] { "北京市", "上海市", "天津市", "重庆市", "滨州市", "德州市", "东营市", "菏泽市", "济南市", "济宁市", "莱芜市", "聊城市", "临沂市", "青岛市", "日照市", "山东省", "泰安市", "威海市", "潍坊市", "烟台市", "淄博市", "保定市", "沧州市", "承德市", "邯郸市", "河北省", "衡水市", "廊坊市", "秦皇岛市", "石家庄市", "唐山市", "邢台市", "长治市", "大同市", "晋城市", "离石市", "临汾市", "山西省", "太原市", "忻州市", "阳泉市", "榆次市", "运城市", "包头市", "赤峰市", "东胜市", "海拉尔市", "呼和浩特市", "集宁市", "临河市", "内蒙古", "通辽市", "乌海市", "锡林浩特市", "鞍山市", "本溪市", "大连市", "丹东市", "抚顺市", "阜新市", "锦州市", "辽宁省", "辽阳市", "沈阳市", "铁岭市", "瓦房店市", "营口市", "白城市", "长春市", "浑江市", "吉林省", "吉林市", "辽源市", "梅河口市", "南敦市", "四平市", "通化市", "延吉市", "阿城市", "大庆市", "哈尔滨市", "黑河市", "黑龙江省", "加格达奇市", "佳木斯市", "牡丹江市", "齐齐哈尔市", "绥化市", "伊春市", "常熟市", "常州市", "淮阴市", "江苏省", "连云港市", "南京市", "南通市", "苏州市", "无锡市", "徐州市", "盐城市", "扬州市", "张家港市", "镇江市", "杭州市", "湖州市", "嘉兴市", "金华市", "丽水市", "临海市", "宁波市", "衢州市", "绍兴市", "温州市", "浙江省", "安徽省", "安庆市", "蚌埠市", "巢湖市", "滁洲市", "阜阳市", "合肥市", "淮北市", "黄山市", "六安市", "马鞍山市", "宿县市", "铜陵市", "芜湖市", "宣城市", "福建省", "福州市", "龙岩市", "南平市", "宁德市", "莆田市", "泉州市", "三明市", "厦门市", "漳州市", "分宜市", "赣州市", "吉安市", "江西省", "景德镇市", "九江市", "陵川市", "南昌市", "萍乡市", "上饶市", "宜春市", "安阳市", "河南省", "鹤壁市", "焦作市", "开封市", "洛阳市", "漯河市", "南阳市", "平顶山市", "濮阳市", "三门峡市", "商丘市", "新乡市", "信阳市", "许昌市", "郑州市", "周口市", "驻马店市", "鄂州市", "恩施市", "湖北省", "黄冈市", "黄石市", "荆门市", "沙市市", "十堰市", "武汉市", "咸宁市", "襄樊市", "孝感市", "宜昌市", "长沙市", "常德市", "郴州市", "衡阳市", "湖南省", "怀化市", "济源市", "娄底市", "邵阳市", "湘潭市", "益阳市", "岳阳市", "株洲市", "东莞市", "番禺市", "佛山市", "广东省", "广州市", "惠州市", "江门市", "南海市", "汕头市", "汕尾市", "深圳市", "顺德市", "中山市", "珠海市", "北海市", "防城市", "广西省", "桂林市", "河池市", "柳州市", "南宁市", "钦州市", "梧州市", "玉林市", "成都市", "达县市", "德阳市", "涪陵市", "广元市", "锦阳市", "乐山市", "泸州市", "马尔康市", "内江市", "南充市", "攀枝花市", "四川省", "万县市", "温江市", "西昌市", "雅安市", "宜宾市", "永川市", "自贡市", "安顺市", "毕节市", "都匀市", "贵阳市", "贵州省", "凯里市", "六盘水市", "铜仁市", "兴义市", "遵义市", "保山市", "楚雄市", "大理市", "个旧市", "昆明市", "曲靖市", "思茅市", "文山市", "玉溪市", "云南省", "昭通市", "拉萨市", "西藏自治区", "安康市", "宝鸡市", "汉中市", "陕西省", "商州市", "铜川市", "渭南市", "西安市", "咸阳市", "延安市", "榆林市", "白银市", "敦煌市", "甘肃省", "合作市", "嘉峪关市", "金昌市", "酒泉市", "兰州市", "临夏市", "平凉市", "天水市", "武威市", "西峰市", "玉门市", "张掖市", "德令哈市", "格尔木市", "共和市", "门源市", "平安市", "青海省", "同仁市", "西宁市", "固原市", "宁夏自治区", "青铜峡市", "石嘴山市", "吴忠市", "银川市", "阿克苏市", "昌吉市", "喀什市", "克拉玛依市", "库尔勒市", "奎屯市", "石河子市", "吐鲁番市", "乌鲁木齐市", "新疆自治区", "伊犁市", "九龙", "香港特区", "新界", "高雄市", "基隆市", "台北市", "台湾省", "台中市", "新竹市", "儋县市", "儋州市", "东方市", "海口市", "海南省", "琼山市", "三亚市", "通什市", "万宁市", "文昌市" };
        private static readonly string[] Areas = new string[] { "北京", "上海", "天津", "重庆", "滨州", "德州", "东营", "菏泽", "济南", "济宁", "莱芜", "聊城", "临沂", "青岛", "日照", "山东", "泰安", "威海", "潍坊", "烟台", "枣庄", "淄博", "保定", "沧州", "承德", "邯郸", "河北", "衡水", "廊坊", "秦皇岛", "石家庄", "唐山", "邢台", "长治", "大同", "晋城", "离石", "临汾", "山西", "太原", "忻州", "阳泉", "榆次", "运城", "包头", "赤峰", "东胜", "海拉尔", "呼和浩特", "集宁", "临河", "内蒙古", "通辽", "乌海", "锡林浩特", "鞍山", "本溪", "大连", "丹东", "抚顺", "阜新", "锦州", "辽宁", "辽阳", "沈阳", "铁岭", "瓦房店", "营口", "白城", "长春", "浑江", "吉林", "吉林", "辽源", "梅河口", "南敦", "四平", "通化", "延吉", "阿城", "大庆", "哈尔滨", "黑河", "黑龙江", "加格达奇", "佳木斯", "牡丹江", "齐齐哈尔", "绥化", "伊春", "常熟", "常州", "淮阴", "江苏", "连云港", "南京", "南通", "苏州", "无锡", "徐州", "盐城", "扬州", "张家港", "镇江", "杭州", "湖州", "嘉兴", "金华", "丽水", "临海", "宁波", "衢州", "绍兴", "温州", "浙江", "安徽", "安庆", "蚌埠", "巢湖", "滁洲", "阜阳", "合肥", "淮北", "黄山", "六安", "马鞍山", "宿县", "铜陵", "芜湖", "宣城", "福建", "福州", "龙岩", "南平", "宁德", "莆田", "泉州", "三明", "厦门", "漳州", "分宜", "赣州", "吉安", "江西", "景德镇", "九江", "陵川", "南昌", "萍乡", "上饶", "宜春", "安阳", "河南", "鹤壁", "焦作", "开封", "洛阳", "漯河", "南阳", "平顶山", "濮阳", "三门峡", "商丘", "新乡", "信阳", "许昌", "郑州", "周口", "驻马店", "鄂州", "恩施", "湖北", "黄冈", "黄石", "荆门", "沙", "十堰", "武汉", "咸宁", "襄樊", "孝感", "宜昌", "长沙", "常德", "郴州", "衡阳", "湖南", "怀化", "济源", "娄底", "邵阳", "湘潭", "益阳", "岳阳", "株洲", "东莞", "番禺", "佛山", "广东", "广州", "惠州", "江门", "南海", "汕头", "汕尾", "深圳", "顺德", "中山", "珠海", "北海", "防城", "广西", "桂林", "河池", "柳州", "南宁", "钦州", "梧州", "玉林", "成都", "达县", "德阳", "涪陵", "广元", "锦阳", "乐山", "泸州", "马尔康", "内江", "南充", "攀枝花", "四川", "万县", "温江", "西昌", "雅安", "宜宾", "永川", "自贡", "安顺", "毕节", "都匀", "贵阳", "贵州", "凯里", "六盘水", "铜仁", "兴义", "遵义", "保山", "楚雄", "大理", "个旧", "昆明", "曲靖", "思茅", "文山", "玉溪", "云南", "昭通", "拉萨", "西藏", "安康", "宝鸡", "汉中", "陕西", "商州", "铜川", "渭南", "西安", "咸阳", "延安", "榆林", "白银", "敦煌", "甘肃", "合作", "嘉峪关", "金昌", "酒泉", "兰州", "临夏", "平凉", "天水", "武威", "西峰", "玉门", "张掖", "德令哈", "格尔木", "共和", "门源", "平安", "青海", "同仁", "西宁", "固原", "宁夏", "青铜峡", "石嘴山", "吴忠", "银川", "阿克苏", "昌吉", "喀什", "克拉玛依", "库尔勒", "奎屯", "石河子", "吐鲁番", "乌鲁木齐", "新疆", "伊犁", "九龙", "香港", "高雄", "基隆", "台北", "台湾", "台中", "新竹", "澳门", "儋州", "海口", "海南", "琼山", "三亚", "通什", "万宁", "文昌", "爱尔兰", "奥地利", "澳大利亚", "波兰", "丹麦", "德国", "俄罗斯", "法国", "菲律宾", "芬兰", "韩国", "荷兰", "加拿大", "美国", "南斯拉夫", "日本", "沙特阿拉伯", "西班牙", "新加坡", "新西兰", "意大利", "印度", "英国" };
        private static readonly string[] CompanyNamePostfixs = new string[] { "科技", "股份", "有限", "责任", "公司", "集团", "厂" };
        private static readonly string[] SchoolNamePostfixs = new string[] { "小学", "中学", "学校", "学院", "大学" };

        /// <summary>
        /// 查询公司名称时过滤无意义词语
        /// </summary>
        public static string CompanyNameScrub(string companyName)
        {
            companyName = Remove(companyName, CompanyNamePostfixs);
            companyName = Remove(companyName, LuceneKeywords);
            return companyName.Trim();
        }

        /// <summary>
        /// 查询学校名称时过滤无意义词语
        /// </summary>
        public static string SchoolNameScrub(string schoolName)
        {
            schoolName = Remove(schoolName, SchoolNamePostfixs);
            schoolName = Remove(schoolName, LuceneKeywords);
            return schoolName.Trim();
        }

        /// <summary>
        /// 去除Lucene关键字
        /// </summary>
        public static string LuceneKeywordsScrub(string str)
        {
            str = Remove(str, LuceneKeywords);
            if (str != null)
                str = str.Trim();

            return str;
        }

        /// <summary>
        /// 检查字符串中是否以字母(a-z,A-Z)或数字开头
        /// </summary>
        /// <param name="str">检查的字符串</param>
        /// <returns>如果包含字母或数字返回true,否则返回false</returns>
        public static bool StartsWithLetterOrDigit(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            bool result = false;
            char startChar = str[0];
            switch (Char.GetUnicodeCategory(startChar))
            {
                case UnicodeCategory.DecimalDigitNumber:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.UppercaseLetter:
                    result = true;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 为PhraseQuery切分关键词
        /// </summary>
        /// <param name="keywords">待切分关键词</param>
        /// <returns></returns>
        public static string[] SegmentForPhraseQuery(string keywords)
        {
            ICollection<WordInfo> words = SegmentToWordInfos(keywords);
            return words.Select(n => n.Word).ToArray();
        }

        /// <summary>
        /// 根据文章标题智能解析关键字(或标签)
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string[] TitleToKeywords(string title)
        {
            ICollection<WordInfo> words = TitleToKeywordWordInfos(title);
            return words.Select(n => n.Word).Where(n => n.Length > 1).Distinct().ToArray();
        }

        #region Help Methods

        private static string Remove(string str, string[] removeStrings)
        {
            if (string.IsNullOrEmpty(str) || removeStrings == null)
                return str;

            string[] arraySplit = str.Split(removeStrings, StringSplitOptions.RemoveEmptyEntries);

            return string.Join("", arraySplit);
        }

        /// <summary>
        /// 把一个语句划分成多个词
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static ICollection<WordInfo> SegmentToWordInfos(string str)
        {
            PanGu.Segment segment = new Segment();

            PanGu.Match.MatchOptions matchOptions = new PanGu.Match.MatchOptions();

            //中文人名识别
            matchOptions.ChineseNameIdentify = false;
            //词频优先
            matchOptions.FrequencyFirst = false;
            //多元分词
            matchOptions.MultiDimensionality = true;
            //英文多元分词，这个开关，会将英文中的字母和数字分开
            matchOptions.EnglishMultiDimensionality = false;
            //过滤停用词
            matchOptions.FilterStopWords = true;
            //忽略空格、回车、Tab
            matchOptions.IgnoreSpace = true;
            //强制一元分词
            matchOptions.ForceSingleWord = false;
            //繁体中文开关
            matchOptions.TraditionalChineseEnabled = false;
            //同时输出简体和繁体
            matchOptions.OutputSimplifiedTraditional = false;
            //未登录词识别
            matchOptions.UnknownWordIdentify = false;
            //过滤英文，这个选项只有在过滤停用词选项生效时才有效
            matchOptions.FilterEnglish = false;
            //过滤数字，这个选项只有在过滤停用词选项生效时才有效
            matchOptions.FilterNumeric = false;
            //忽略英文大小写
            matchOptions.IgnoreCapital = true;
            //英文分词
            matchOptions.EnglishSegment = false;
            //同义词输出  （同义词输出功能一般用于对搜索字符串的分词，不建议在索引时使用）
            matchOptions.SynonymOutput = false;
            //通配符匹配输出 （）
            matchOptions.WildcardOutput = false;
            //对通配符匹配的结果分词
            matchOptions.WildcardSegment = false;

            PanGu.Match.MatchParameter matchParameter = new PanGu.Match.MatchParameter();
            //未登录词权值
            matchParameter.UnknowRank = 1;
            //最匹配词权值
            matchParameter.BestRank = 5;
            //次匹配词权值
            matchParameter.SecRank = 3;
            //再次匹配词权值
            matchParameter.ThirdRank = 2;
            //强行输出的单字的权值
            matchParameter.SingleRank = 1;
            //数字的权值
            matchParameter.NumericRank = 1;
            //英文词汇权值
            matchParameter.EnglishRank = 5;
            //英文词汇小写的权值
            matchParameter.EnglishLowerRank = 3;
            //英文词汇词根的权值
            matchParameter.EnglishStemRank = 2;
            //符号的权值
            matchParameter.SymbolRank = 1;
            //强制同时输出简繁汉字时，非原来文本的汉字输出权值。 比如原来文本是简体，这里就是输出的繁体字的权值，反之亦然。
            matchParameter.SimplifiedTraditionalRank = 1;
            //同义词权值
            matchParameter.SynonymRank = 1;
            //通配符匹配结果的权值
            matchParameter.WildcardRank = 1;
            //过滤英文选项生效时，过滤大于这个长度的英文
            matchParameter.FilterEnglishLength = 0;
            //过滤数字选项生效时，过滤大于这个长度的数字
            matchParameter.FilterNumericLength = 0;
            //用户自定义规则的配件文件名
            matchParameter.CustomRuleAssemblyFileName = string.Empty;
            //用户自定义规则的类的完整名，即带名字空间的名称
            matchParameter.CustomRuleFullClassName = string.Empty;
            //冗余度
            matchParameter.Redundancy = 2;

            return segment.DoSegment(str, matchOptions, matchParameter);
        }

        /// <summary>
        /// 根据文章标题智能解析关键字(或标签)
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        private static ICollection<WordInfo> TitleToKeywordWordInfos(string title)
        {
            PanGu.Segment segment = new Segment();

            PanGu.Match.MatchOptions matchOptions = new PanGu.Match.MatchOptions();

            //中文人名识别
            matchOptions.ChineseNameIdentify = false;
            //词频优先
            matchOptions.FrequencyFirst = false;
            //多元分词
            matchOptions.MultiDimensionality = false;
            //英文多元分词，这个开关，会将英文中的字母和数字分开
            matchOptions.EnglishMultiDimensionality = false;
            //过滤停用词
            matchOptions.FilterStopWords = true;
            //忽略空格、回车、Tab
            matchOptions.IgnoreSpace = true;
            //强制一元分词
            matchOptions.ForceSingleWord = false;
            //繁体中文开关
            matchOptions.TraditionalChineseEnabled = false;
            //同时输出简体和繁体
            matchOptions.OutputSimplifiedTraditional = false;
            //未登录词识别
            matchOptions.UnknownWordIdentify = false;
            //过滤英文，这个选项只有在过滤停用词选项生效时才有效
            matchOptions.FilterEnglish = true;
            //过滤数字，这个选项只有在过滤停用词选项生效时才有效
            matchOptions.FilterNumeric = true;
            //忽略英文大小写
            matchOptions.IgnoreCapital = false;
            //英文分词
            matchOptions.EnglishSegment = false;
            //同义词输出  （同义词输出功能一般用于对搜索字符串的分词，不建议在索引时使用）
            matchOptions.SynonymOutput = false;
            //通配符匹配输出 （）
            matchOptions.WildcardOutput = false;
            //对通配符匹配的结果分词
            matchOptions.WildcardSegment = false;

            PanGu.Match.MatchParameter matchParameter = new PanGu.Match.MatchParameter();
            //未登录词权值
            matchParameter.UnknowRank = 1;
            //最匹配词权值
            matchParameter.BestRank = 5;
            //次匹配词权值
            matchParameter.SecRank = 3;
            //再次匹配词权值
            matchParameter.ThirdRank = 2;
            //强行输出的单字的权值
            matchParameter.SingleRank = 1;
            //数字的权值
            matchParameter.NumericRank = 1;
            //英文词汇权值
            matchParameter.EnglishRank = 5;
            //英文词汇小写的权值
            matchParameter.EnglishLowerRank = 3;
            //英文词汇词根的权值
            matchParameter.EnglishStemRank = 2;
            //符号的权值
            matchParameter.SymbolRank = 1;
            //强制同时输出简繁汉字时，非原来文本的汉字输出权值。 比如原来文本是简体，这里就是输出的繁体字的权值，反之亦然。
            matchParameter.SimplifiedTraditionalRank = 1;
            //同义词权值
            matchParameter.SynonymRank = 1;
            //通配符匹配结果的权值
            matchParameter.WildcardRank = 1;
            //过滤英文选项生效时，过滤大于这个长度的英文
            matchParameter.FilterEnglishLength = 0;
            //过滤数字选项生效时，过滤大于这个长度的数字
            matchParameter.FilterNumericLength = 0;
            //用户自定义规则的配件文件名
            matchParameter.CustomRuleAssemblyFileName = string.Empty;
            //用户自定义规则的类的完整名，即带名字空间的名称
            matchParameter.CustomRuleFullClassName = string.Empty;
            //冗余度
            matchParameter.Redundancy = 0;

            return segment.DoSegment(title, matchOptions, matchParameter);
        }

        #endregion

    }
}
