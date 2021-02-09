using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.SetViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface ISetService : IGenericService<Set>
    {
        IEnumerable<ListSetViewModel> GetAll(ListSetViewModel lsvm, ref Paging pg);
    }
}