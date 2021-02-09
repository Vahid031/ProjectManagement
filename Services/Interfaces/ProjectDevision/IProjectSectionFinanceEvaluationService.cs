using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionFinanceEvaluationViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionFinanceEvaluationService : IGenericService<ProjectSectionFinanceEvaluation>
    {
        IEnumerable<ListProjectSectionFinanceEvaluationViewModel> GetAll(ListProjectSectionFinanceEvaluationViewModel lpscvm, ref Paging pg);
    }
}