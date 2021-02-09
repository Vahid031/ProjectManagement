using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.InvestmentChapterViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IInvestmentChapterService : IGenericService<InvestmentChapter>
    {
        IEnumerable<ListInvestmentChapterViewModel> GetAll(ListInvestmentChapterViewModel licvm, ref Paging pg);
    }
}