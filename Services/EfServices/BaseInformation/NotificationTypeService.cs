﻿using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using DomainModels.Entities.BaseInformation;
using Services.Interfaces.General;
using Services.Interfaces.BaseInformation;

namespace Services.EfServices.BaseInformation
{
    public class NotificationTypeService : GenericService<NotificationType>, INotificationTypeService
    {
        public NotificationTypeService(IUnitOfWork uow)
            : base(uow)
        {

        }

    }
}
