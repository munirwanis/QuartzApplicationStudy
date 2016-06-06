using Quartz;
using System;

namespace QuartzApplicationStudy.Jobs {
    public class HelloJob : IJob {
        public void Execute(IJobExecutionContext context) {
            Console.WriteLine("Hello World!");
        }
    }
}
