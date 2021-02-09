using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.StatementTypeViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IStatementTypeService : IGenericService<StatementType>
    {
        IEnumerable<ListStatementTypeViewModel> GetAll(ListStatementTypeViewModel lstvm, ref Paging pg);
    }
}