using Owin;
using Quartz;
using Quartz.Impl;
using QuartzApplicationStudy.Jobs;
using System;
using CrystalQuartz.Owin;
using Microsoft.Owin.Hosting;
using System.Collections.Generic;
using System.Threading;

namespace QuartzApplicationStudy {
    class Program {
        static void Main(string[] args) {
            Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter { Level = Common.Logging.LogLevel.Info };
            try {
                IScheduler scheduler = SetupScheduler();
                Action<IAppBuilder> startup = app => {
                    app.UseCrystalQuartz(scheduler);
                };

                Console.WriteLine("Starting self-hosted server");
                using (WebApp.Start("http://localhost:9000/", startup)) {
                    Console.WriteLine("Server is started");
                    Console.WriteLine();
                    Console.WriteLine("Check http://localhost:9000/CrystalQuartzPanel.axd to see jobs information");

                    Console.WriteLine("Starting Scheduler");
                    scheduler.Start();

                    Console.WriteLine("Scheduler is running");
                    Console.WriteLine();
                    Console.WriteLine("Press [ENTER] to close");
                    Console.ReadLine();
                }
                Console.WriteLine("Shutting Down");
                scheduler.Shutdown();
                Console.WriteLine("Scheduler has been stopped");
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }

        private static IScheduler SetupScheduler() {
            #region Starting Scheduler
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

            //scheduler.Start();
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
            //Shutdown(scheduler);
            #endregion

            return scheduler;
        }

        static void Shutdown(IScheduler scheduler) {
            Console.WriteLine("\nPress 'f' to shutdown Quartz and 'enter' to finish application.");
            if (Console.ReadKey().KeyChar == 'f') {
                scheduler.Shutdown();
            }
            else {
                Shutdown(scheduler);
            }
        }
    }
}
