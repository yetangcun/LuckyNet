using System;
using Quartz;
using System.Text;
using System.Collections.Generic;

namespace Tsk.Quartz.Jobs.Example
{
    public class IntervalTestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("IntervalTestJob: " + DateTime.Now.ToString());
        }
    }
}
