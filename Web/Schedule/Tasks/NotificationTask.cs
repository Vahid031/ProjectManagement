using DatabaseContext.Context;
using DomainModels.Entities.General;
using DomainModels.Enums;
using Infrastracture.AppCode;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Scheduler;
using Services.Interfaces.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModels.General.NotificationViewModel;
using Web.DependencyResolver;
using Web.Hubs;

namespace Web.Schedule.Tasks
{
    public class NotificationTask : ScheduledTaskTemplate
    {
        /// <summary>
        /// نام وظیفه‌ی جاری است که می‌تواند در گزارشات مفید باشد
        /// </summary>
        public override string Name
        {
            get { return "پیغام ها"; }
        }

        /// <summary>
        /// اگر چند جاب در یک زمان مشخص داشتید، این خاصیت ترتیب اجرای آن‌ها را مشخص خواهد کرد
        /// </summary>
        public override int Order
        {
            get { return 1; }
        }


        /// <summary>
        /// این متد ثانیه‌ای یکبار فراخوانی می‌شود
        /// </summary>
        /// <param name="utcNow">تاریخ جاری بر اساس ساعت جهانی گرینویچ</param>
        /// <returns>متد 'ران' اجرا بشه یا نشه</returns>
        public override bool RunAt(DateTime utcNow)
        {
            if (this.IsShuttingDown || this.Pause)
                return false;

            return utcNow.Second == 1; // هر دقیقه یکبار اجرا بشه
        }


        /// <summary>
        /// متد 'ران'
        ///اصل کدی که باید در زمان های مورد نظر ما اجرا بشه
        /// </summary>
        public override void Run()
        {
            if (this.IsShuttingDown || this.Pause)
                return;

            NotificationHub notifHup = new NotificationHub();
            notifHup.RefreshNotification();
        }

    }
}