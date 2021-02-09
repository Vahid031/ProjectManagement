using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.RuralDistrictViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IRuralDistrictService : IGenericService<RuralDistrict>
    {
        IEnumerable<ListRuralDistrictViewModel> GetAll(ListRuralDistrictViewModel lrdvm, ref Paging pg);
    }
}