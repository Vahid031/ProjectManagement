using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.ExecutionTypeViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IExecutionTypeService : IGenericService<ExecutionType>
    {
        IEnumerable<ListExecutionTypeViewModel> GetAll(ListExecutionTypeViewModel letvm, ref Paging pg);
    }
}