using Quartz;
using Quartz.Impl;
using QuartzApplicationStudy.Jobs;
using System;
using System.Threading;

namespace QuartzApplicationStudy {
    class Program {
        static void Main(string[] args) {
            Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter { Level = Common.Logging.LogLevel.Info };
            try {
                #region Starting Scheduler
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

                scheduler.Start();
                #endregion

                #region Creating a Job
                IJobDetail job = JobBuilder.Create<HelloJob>()
                    .WithIdentity("job1", "group1")
                    .Build();
                #endregion

                #region Triggering Job
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("job1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(1)
                        .RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using the trigger
                scheduler.ScheduleJob(job, trigger);
                #endregion

                Thread.Sleep(TimeSpan.FromSeconds(5));

                #region Terminating Scheduler
                scheduler.Shutdown();
                #endregion
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }
    }
}
