using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.CityViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface ICityService : IGenericService<City>
    {
        IEnumerable<ListCityViewModel> GetAll(ListCityViewModel lcvm, ref Paging pg);
    }
}