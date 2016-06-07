using Quartz;
using Quartz.Impl;
using QuartzApplicationStudy.Jobs;
using System;
using System.Collections.Generic;
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
                IJobDetail helloJob = JobBuilder.Create<HelloJob>()
                    .WithIdentity("job1", "group1")
                    .Build();
                #endregion

                #region Creating a Job with Data Mapping
                  IJobDetail dumbJob = JobBuilder.Create<DumbJob>()
                    .WithIdentity("job2", "group1")
                    .UsingJobData("jobSays", "Hello World!")
                    .UsingJobData("myFloatValue", 3.14f)
                    .Build();
                #endregion

                #region Triggering Job
                ITrigger helloTrigger = TriggerBuilder.Create()
                    .WithIdentity("job1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(1)
                        .RepeatForever())
                    .Build();

                ITrigger dumbTrigger = TriggerBuilder.Create()
                    .WithIdentity("job2", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(3)
                        .RepeatForever())
                    .Build();
                
                // Tell quartz to schedule the job using the trigger
                scheduler.ScheduleJob(helloJob, helloTrigger);
                scheduler.ScheduleJob(dumbJob, dumbTrigger);
                #endregion

                //Thread.Sleep(TimeSpan.FromSeconds(5));

                #region Terminating Scheduler
                Shutdown(scheduler);
                #endregion
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }

            Console.ReadLine();
        }

        static void Shutdown(IScheduler scheduler) {
            if (Console.ReadKey().KeyChar == 'f') {
                scheduler.Shutdown();
            }
            else {
                Shutdown(scheduler);
            }
        }
    }
}
