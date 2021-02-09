using Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Web.Schedule.Tasks;

namespace Web.Schedule
{
    public static class ScheduledTasksRegistry
    {
        public static void Init()
        {
            ScheduledTasksCoordinator.Current.AddScheduledTasks(
                new NotificationTask());

            ScheduledTasksCoordinator.Current.OnUnexpectedException = (exception, scheduledTask) =>
            {
                //todo: log the exception.
                System.Diagnostics.Trace.WriteLine(scheduledTask.Name + ":" + exception.Message);
            };

            // start Thread timer
            ScheduledTasksCoordinator.Current.Start();
        }

        public static void End()
        {
            ScheduledTasksCoordinator.Current.Dispose();
        }

        public static void WakeUp(string pageUrl)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Credentials = CredentialCache.DefaultNetworkCredentials;
                    client.Headers.Add("User-Agent", "ScheduledTasks 1.0");
                    client.DownloadData(pageUrl);
                }
            }
            catch (Exception ex)
            {
                //todo: log ex
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
        }

    }
}