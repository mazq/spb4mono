//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using Tunynet.Tasks;
using System.Linq;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用于表单呈现的TaskDetail实体
    /// </summary>
    public class TaskDetailEditModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        [Display(Name = "是否重复执行")]
        [Required(ErrorMessage = "必须选择一个选项")]
        public bool IsRepeat { get; set; }

        [Display(Name = "任务名")]
        [Required(ErrorMessage = "必须填写任务名称")]
        [StringLength(64, ErrorMessage = "最多输入64个字符")]
        public string TaskName { get; set; }

        [Display(Name = "执行一次任务的日期")]
        public DateTime OnceDate { get; set; }

        [Display(Name = "秒")]
        public string Seconds { get; set; }

        [Display(Name = "分钟")]
        public string Minutes { get; set; }

        [Display(Name = "小时")]
        public string Hours { get; set; }

        [Display(Name = "执行一次的时间")]
        public DateTime OnceTime { get; set; }

        [Display(Name = "每天执行一次")]
        public bool DayRepeat { get; set; }

        [Display(Name = "天")]
        public string Day { get; set; }

        [Display(Name = "每月中的某一天")]
        public string DayOfMouth { get; set; }

        [Display(Name = "月")]
        public string Mouth { get; set; }

        [Display(Name = "星期")]
        public string[] DayOfWeek { get; set; }

        [Display(Name = "是否启用")]
        public bool Enabled { get; set; }

        [Display(Name = "恢复后马上执行")]
        public bool RunAtRestart { get; set; }

        [Display(Name = "频率")]
        public TaskFrequency Frequency { get; set; }

        [Display(Name = "开始时间")]
        public DateTime StartDate { get; set; }

        [Display(Name = "结束时间")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        ///每个月中的天还是星期 
        /// </summary>
        /// <remarks>ture-天/false星期</remarks>
        public bool DayOrWeekly { get; set; }

        /// <summary>
        /// 第几个星期
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 每个月的星期几
        /// </summary>
        public string WeeklyOfMouth { get; set; }

        /// <summary>
        /// 转换为TaskDetailEditModel用于数据库存储
        /// </summary>
        public TaskDetail AsTaskDetail()
        {
            TaskDetail taskDetail = new TaskService().Get(Id);

            if (taskDetail == null) taskDetail = TaskDetail.New();

            taskDetail.Name = TaskName;
            taskDetail.TaskRule = GenerateRule();
            taskDetail.StartDate = StartDate;
            taskDetail.EndDate = EndDate;
            taskDetail.Enabled = Enabled;

            return taskDetail;
        }

        #region Helper Method

        /// <summary>
        /// 生成规则
        /// </summary>
        private string GenerateRule()
        {
            string ruleFormat = "{0} {1} {2} {3} {4} {5}";
            if (!IsRepeat)
            {
                return string.Format(ruleFormat, OnceDate.Second
                                               , OnceDate.Minute
                                               , OnceDate.Hour
                                               , OnceDate.Day
                                               , OnceDate.Month
                                               , "?");
            }

            string dayOfWeek = DayOfWeek != null ? String.Join(",", DayOfWeek) : "?";

            switch (Frequency)
            {
                case TaskFrequency.EveryDay:
                    dayOfWeek = "?";
                    Mouth = "*";
                    break;
                case TaskFrequency.PerMonth:
                    Day = DayOfMouth;
                    if (!DayOrWeekly)
                    {
                        dayOfWeek = WeeklyOfMouth + "#" + Number;
                        Day = "?";
                    }
                    break;
                case TaskFrequency.Weekly:
                    Day = dayOfWeek != "?" ? "?" : Day;
                    break;
            }

            return string.Format(ruleFormat, GenerateRulePart(Seconds, RulePart.seconds)
                                           , GenerateRulePart(Minutes, RulePart.minutes)
                                           , GenerateRulePart(Hours, RulePart.hours)
                                           , GenerateRulePart(Day, RulePart.day)
                                           , GenerateRulePart(Mouth, RulePart.mouth)
                                           , string.IsNullOrEmpty(dayOfWeek) ? "?" : dayOfWeek);
        }

        /// <summary>
        /// 生成规则的每一部分
        /// </summary>
        /// <param name="value">规则的某一部分值</param>
        /// <param name="rulePart">任务规则组成部分</param>
        /// <returns></returns>
        private string GenerateRulePart(string value, RulePart? rulePart = null)
        {
            if (rulePart.HasValue)
            {
                if (!IsRepeat || (IsRepeat && !DayRepeat))
                {
                    switch (rulePart)
                    {
                        case RulePart.hours:
                            return OnceTime.Hour.ToString();
                        case RulePart.minutes:
                            return OnceTime.Minute.ToString();
                        case RulePart.seconds:
                            return OnceTime.Second.ToString();
                    }
                }

                if (IsRepeat && rulePart != RulePart.dayofweek)
                {
                    if (((Frequency == TaskFrequency.PerMonth || Frequency == TaskFrequency.Weekly) && rulePart == RulePart.day)
                        || (Frequency == TaskFrequency.EveryDay && rulePart == RulePart.mouth))
                    {
                        return value ?? "*";
                    }

                    if (value == "0")
                    {
                        return value;
                    }

                    string[] terms = new string[] { "*", "1" };
                    if (terms.Contains(value))
                        return "*";

                    if (rulePart == RulePart.day || rulePart == RulePart.mouth)
                    {
                        return "*/" + value;
                    }

                    return "0/" + value;
                }
            }

            return value ?? "*";
        }

        #endregion
    }

    /// <summary>
    /// TaskDetail扩展
    /// </summary>
    public static class TaskDetailExtensions
    {
        /// <summary>
        /// 转换成TaskDetailEditModel
        /// </summary>
        /// <returns></returns>
        public static TaskDetailEditModel AsEditModel(this TaskDetail taskDetail)
        {
            string seconds = taskDetail.GetRulePart(RulePart.seconds);
            string minutes = taskDetail.GetRulePart(RulePart.minutes);
            string hours = taskDetail.GetRulePart(RulePart.hours);
            string day = taskDetail.GetRulePart(RulePart.day);
            string mouth = taskDetail.GetRulePart(RulePart.mouth);
            string dayOfWeek = taskDetail.GetRulePart(RulePart.dayofweek) ?? string.Empty;

            string[] rulePartArray = taskDetail.TaskRule.Split(' ');
            TaskFrequency frequency = TaskFrequency.EveryDay;
            if ((rulePartArray[3].Contains("/") || rulePartArray[3] == "*") && rulePartArray[4] == "*")
            {
                frequency = TaskFrequency.EveryDay;
            }
            else if (rulePartArray[5] != "?" && !rulePartArray[5].Contains("#"))
            {
                frequency = TaskFrequency.Weekly;
            }
            else
            {
                frequency = TaskFrequency.PerMonth;
            }

            bool dayRepeat = false;
            for (int i = 0; i < 3; i++)
            {
                dayRepeat = rulePartArray[i].Contains("*") || rulePartArray[i].Contains("/");

                if (dayRepeat) break;
            }

            return new TaskDetailEditModel()
            {
                TaskName = taskDetail.Name,
                IsRepeat = taskDetail.TaskRule.Contains("/") || taskDetail.TaskRule.Contains("*"),
                Seconds = seconds,
                Minutes = minutes,
                Hours = hours,
                Day = day,
                Mouth = mouth,
                Frequency = frequency,
                DayOrWeekly = !dayOfWeek.Contains("#"),
                DayOfMouth = day,
                WeeklyOfMouth = dayOfWeek.Contains("#") ? dayOfWeek.Substring(0, dayOfWeek.IndexOf("#")) : "2",
                Number = dayOfWeek.Contains("#") ? dayOfWeek.Substring(dayOfWeek.IndexOf("#") + 1) : "1",
                DayOfWeek = dayOfWeek.Contains("#") ? null : dayOfWeek.Split(','),
                DayRepeat = dayRepeat,
                OnceTime = !dayRepeat ? new DateTime(1, 1, 1, Convert.ToInt32(hours), Convert.ToInt32(minutes), Convert.ToInt32(seconds)) : new DateTime(1, 1, 1, 1, 0, 0),
                StartDate = string.IsNullOrEmpty(taskDetail.StartDate.ToString()) ? DateTime.Now : taskDetail.StartDate,
                EndDate = taskDetail.EndDate ?? new DateTime(),
                Enabled = taskDetail.Enabled
            };
        }
    }
}