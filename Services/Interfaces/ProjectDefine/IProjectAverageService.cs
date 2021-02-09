using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using Services.Interfaces.ProjectDefine;
using DomainModels.Entities.ProjectDefine;
using ViewModels.ProjectDefine.ProjectAverageViewModel;

namespace Services.Interfaces.ProjectDefine
{
    public interface IProjectAverageService : IGenericService<ProjectAverage>
    {
        IEnumerable<ListProjectAverageViewModel> GetAll(ListProjectAverageViewModel lpavm, ref Paging pg);
    }
}