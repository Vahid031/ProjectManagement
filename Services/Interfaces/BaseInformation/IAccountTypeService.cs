using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.AccountTypeViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IAccountTypeService : IGenericService<AccountType>
    {
        IEnumerable<ListAccountTypeViewModel> GetAll(ListAccountTypeViewModel latvm, ref Paging pg);
    }
}