using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.ContractTypeViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IContractTypeService : IGenericService<ContractType>
    {
        IEnumerable<ListContractTypeViewModel> GetAll(ListContractTypeViewModel lctvm, ref Paging pg);
    }
}