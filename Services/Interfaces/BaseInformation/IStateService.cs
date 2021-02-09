using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.StateViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IStateService : IGenericService<State>
    {
        IEnumerable<ListStateViewModel> GetAll(ListStateViewModel lsvm, ref Paging pg);
    }
}