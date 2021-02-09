using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.MonitoringTypeViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IMonitoringTypeService : IGenericService<MonitoringType>
    {
        IEnumerable<ListMonitoringTypeViewModel> GetAll(ListMonitoringTypeViewModel lmtvm, ref Paging pg);
    }
}