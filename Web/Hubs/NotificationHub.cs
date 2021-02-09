using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using ViewModels.Other;
using Web.DependencyResolver;
using Services.Interfaces.General;
using System.Configuration;
using DomainModels.Entities.General;
using DatabaseContext.Context;
using System.Web.Script.Serialization;
using Infrastracture.AppCode;

namespace Web.Hubs
{
    public class NotificationHub : Hub
    {
        public void RefreshNotification()
        {
            var _uow = StructureMapObjectFactory.Container.GetInstance<IUnitOfWork>();
            var _notificationService = StructureMapObjectFactory.Container.GetInstance<INotificationService>();
            var con = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            string date = ConvertDate.GetShamsi(DateTime.Now);
            con.Clients.All.RefreshNotification(new JavaScriptSerializer().Serialize(_notificationService.Get(i => i.IsActive == true && i.DateTime.Contains(date)).Select(i => new { i.Id, i.Title, i.Priority, i.NotificationTypeId, i.Url, i.Content, i.DateTime, i.GotoPage }).ToList()));
        }
    }
}