using DomainModels.Entities;
using DomainModels.Entities.ProjectDefine;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDefine.ProjectAgreementViewModel;
using ViewModels.ProjectDefine.ProjectViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IProjectService : IGenericService<Project>
    {
        IEnumerable<ListProjectViewModel> GetAll(ListProjectViewModel lpvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectViewModel catvm, Int64 MemberId, List<string> paths);

        IEnumerable<ListProjectAgreementViewModel> GetProjectAggrement(ListProjectAgreementViewModel lpvm, ref Paging pg);

        IEnumerable<ReportProjectViewModel> GetAllReport(ReportParameterProjectViewModel parameters);
    }
}