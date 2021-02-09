using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.FinantialYearViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IFinantialYearService : IGenericService<FinantialYear>
    {
        IEnumerable<ListFinantialYearViewModel> GetAll(ListFinantialYearViewModel lfyvm, ref Paging pg);
    }
}