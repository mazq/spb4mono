//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections;
using Tunynet;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 以当前时间为基准的Id生成器
    /// </summary>
    public class DefaultIdGenerator : IdGenerator
    {
        //批量生成Id的时间刻度
        private static readonly long timescale = 1000000;

        //生成Id的随机数长度
        private static readonly int randomLength = 3;

        //计算时间间隔的开始时间
        private static readonly DateTime startDateTime = new DateTime(2012, 1, 1);

        /// <summary>
        /// 随机数缓存
        /// </summary>
        private static Hashtable ht = new Hashtable();

        /// <summary>
        /// 时间戳缓存（上一次计算ID的系统时间按时间戳刻度取值）
        /// </summary>
        private long lastEndDateTimeTicks;


        /// <summary>
        /// 获取下一个long类型的Id
        /// </summary>
        /// <returns>
        /// 返回生成下一个Id
        /// </returns>
        protected override long NextLong()
        {
            //取得时间戳（当前时间按刻度取值）
            long timestamp = GetTimestamp(startDateTime, timescale);

            //新一轮时间戳更新后更新缓存
            if (timestamp != lastEndDateTimeTicks)
                ht.Clear();

            //幂
            long power = long.Parse(Math.Pow(10, randomLength).ToString());
            //随机数
            long rand = GetRandom(randomLength);
            //生成结果（Id）
            long result = timestamp * power + rand;

            //如果发现重复
            if (ht.ContainsKey(result))
            {
                bool resultIsRepeated = true;
                //在随机数长度范围内再重复查找一次
                for (int i = 0; i < power; i++)
                {
                    rand = GetRandom(randomLength);
                    result = timestamp * power + rand;
                    //发现非重复的Id
                    if (!ht.ContainsKey(result))
                    {
                        //将新的Id加入HashTable缓存
                        ht.Add(result, result);
                        resultIsRepeated = false;
                        break;//找到一个同一时间戳内的Id即退出
                    }
                }
                if (resultIsRepeated)
                    throw new Exception("生成的Id重复");
            }
            else
            {
                //将新的Id加入HashTable缓存
                ht.Add(result, result);
            }
            //记录当前一轮时间戳（当前时间按刻度取值）
            this.lastEndDateTimeTicks = timestamp;

            return result;
        }

        /// <summary>
        /// 按照时间戳刻度计算当前时间戳
        /// </summary>
        /// <param name="startDateTime">起始时间</param>
        /// <param name="timestampStyleTicks">时间戳刻度值</param>
        /// <returns>long</returns>
        private long GetTimestamp(DateTime startDateTime, long timestampStyleTicks)
        {
            if (timestampStyleTicks <= 0)
                throw new Exception("时间戳刻度样式精度值不符，不能为0或负数");

            DateTime endDateTime = DateTime.Now;
            long ticks = (endDateTime.Ticks - startDateTime.Ticks) / timestampStyleTicks;

            return ticks;
        }

        /// <summary>
        /// 静态随机数生成器
        /// </summary>
        private static Random random;
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="length">随机数长度</param>
        /// <returns></returns>
        private long GetRandom(int length)
        {
            if (length <= 0)
                throw new ArgumentException("随机数长度设置错误，长度必须大于0");

            if (random == null)
                random = new Random();

            int minValue = 0;
            int maxValue = int.Parse(System.Math.Pow(10, length).ToString());
            long result = long.Parse(random.Next(minValue, maxValue).ToString());
            return result;
        }

    }

}
