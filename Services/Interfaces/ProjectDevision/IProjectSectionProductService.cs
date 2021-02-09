using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionProductViewModel;
using DomainModels.Entities.ProjectDevision;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionProductService : IGenericService<ProjectSectionProduct>
    {
        KeyValuePair<Int64, string> Save(CreateProjectSectionProductViewModel cpscvm, Int64 MemberId, List<string> paths);
        IEnumerable<ListProjectSectionProductViewModel> GetAll(ListProjectSectionProductViewModel lpsvm, ref Paging pg);

        // این کد چک می کند که آیا مجموع مبلغ تخصیص به پروژه بیشتر از مجموع مبلغ پیش بینی فاز های پروژه می باشد؟
        bool CheckAllocatePriceAndProductPrice(long ProjectSectionId);
    }
}