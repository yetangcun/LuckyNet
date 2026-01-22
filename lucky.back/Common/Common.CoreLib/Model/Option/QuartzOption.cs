using System;
using System.Text;
using System.Collections.Generic;

namespace Common.CoreLib.Model.Option
{
    /// <summary>
    /// Quartz定时任务配置
    /// </summary>
    [Serializable]
    public class QuartzOption
    {
        /// <summary>
        /// 调度器Id
        /// </summary>
        public string SchedulerId { get; set; } = "QuartzSchedulerId";
    }
}
