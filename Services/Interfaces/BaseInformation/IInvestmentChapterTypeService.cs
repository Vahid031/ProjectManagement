using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.InvestmentChapterTypeViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IInvestmentChapterTypeService : IGenericService<InvestmentChapterType>
    {
        IEnumerable<ListInvestmentChapterTypeViewModel> GetAll(ListInvestmentChapterTypeViewModel lictvm, ref Paging pg);
    }
}