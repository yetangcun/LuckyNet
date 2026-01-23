using Quartz;
using System;
using System.Text;
using System.Collections.Generic;

namespace Tsk.Quartz.Jobs.Example
{
    public class CornTestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("CornTestJob: " + DateTime.Now.ToString());
        }
    }
}
