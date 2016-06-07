using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzApplicationStudy.Jobs {
    public class DumbJob : IJob {
        public void Execute(IJobExecutionContext context) {
            JobKey key = context.JobDetail.Key;

            JobDataMap dataMap = context.JobDetail.JobDataMap;

            string jobSays = dataMap.GetString("jobSays");

            float myFloatValue = dataMap.GetFloat("myFloatValue");

            Console.WriteLine($"Instance {key} of DumbJob says: {jobSays}, and value is: {myFloatValue}");
        }
    }
}
