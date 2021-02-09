using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using Services.Interfaces.ProjectDefine;
using DomainModels.Entities.ProjectDefine;
using ViewModels.ProjectDefine.ProjectQuantityGoalViewModel;

namespace Services.Interfaces.ProjectDefine
{
    public interface IProjectQuantityGoalService : IGenericService<ProjectQuantityGoal>
    {
        IEnumerable<ListProjectQuantityGoalViewModel> GetAll(ListProjectQuantityGoalViewModel lpqgvm, ref Paging pg);
    }
}