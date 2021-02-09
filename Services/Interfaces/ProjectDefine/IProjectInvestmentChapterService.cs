using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using Services.Interfaces.ProjectDefine;
using DomainModels.Entities.ProjectDefine;
using ViewModels.ProjectDefine.ProjectInvestmentChapterViewModel;

namespace Services.Interfaces.ProjectDefine
{
    public interface IProjectInvestmentChapterService : IGenericService<ProjectInvestmentChapter>
    {
        IEnumerable<ListProjectInvestmentChapterViewModel> GetAll(ListProjectInvestmentChapterViewModel lpicvm, ref Paging pg);
    }
}