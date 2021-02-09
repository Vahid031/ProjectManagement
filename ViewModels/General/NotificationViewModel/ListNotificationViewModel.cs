using System.Collections.Generic;
using DomainModels.Entities.General;
using DomainModels.Entities.BaseInformation;

namespace ViewModels.General.NotificationViewModel
{
    public class ListNotificationViewModel
	{
        public Notification Notification { get; set; }
        public NotificationType NotificationType { get; set; }
	}
}