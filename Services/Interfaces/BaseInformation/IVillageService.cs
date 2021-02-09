using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.VillageViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IVillageService : IGenericService<Village>
    {
        IEnumerable<ListVillageViewModel> GetAll(ListVillageViewModel lvvm, ref Paging pg);
    }
}