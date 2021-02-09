using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.CreditProvidePlaceViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface ICreditProvidePlaceService : IGenericService<CreditProvidePlace>
    {
        IEnumerable<ListCreditProvidePlaceViewModel> GetAll(ListCreditProvidePlaceViewModel lcptvm, ref Paging pg);
    }
}