using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.DeductionPercentViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IDeductionPercentService : IGenericService<DeductionPercent>
    {
        IEnumerable<ListDeductionPercentViewModel> GetAll(ListDeductionPercentViewModel ldpvm, ref Paging pg);
    }
}