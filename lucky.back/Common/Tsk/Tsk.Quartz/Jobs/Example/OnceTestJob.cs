using Quartz;
using System;
using System.Text;
using System.Collections.Generic;

namespace Tsk.Quartz.Jobs.Example
{
    public class OnceTestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            if (context.MergedJobDataMap == null || context.MergedJobDataMap.Count == 0)
                Console.WriteLine($"OnceTestJob:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            else
                Console.WriteLine($"OnceTestJob:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},{context.MergedJobDataMap.GetString("name")}");
        }
    }
}
