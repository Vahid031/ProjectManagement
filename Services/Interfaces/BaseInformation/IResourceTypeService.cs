using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.ResourceTypeViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IResourceTypeService : IGenericService<ResourceType>
    {
        IEnumerable<ListResourceTypeViewModel> GetAll(ListResourceTypeViewModel lrtvm, ref Paging pg);
    }
}