using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.UnitViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IUnitService : IGenericService<Unit>
    {
        IEnumerable<ListUnitViewModel> GetAll(ListUnitViewModel luvm, ref Paging pg);
    }
}