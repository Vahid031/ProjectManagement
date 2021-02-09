using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.ResourceViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IResourceService : IGenericService<Resource>
    {
        IEnumerable<ListResourceViewModel> GetAll(ListResourceViewModel lrvm, ref Paging pg);
    }
}