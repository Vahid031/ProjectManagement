using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionViewModel;
using DomainModels.Entities.ProjectDevision;
using ViewModels.ProjectDevision.ProjectSectionAssignmentViewModel;
using ViewModels.ProjectDevision.ProjectSectionOperationViewModel;
using ViewModels.ProjectDevision.ProjectSectionLetterViewModel;
using ViewModels.ProjectDevision.ProjectSection48ViewModel;
using ViewModels.ProjectDevision.ProjectSection46ViewModel;
using ViewModels.ProjectDevision.ProjectSectionLiberalizationViewModel;
using ViewModels.ProjectDevision.ProjectSectionFixedViewModel;
using ViewModels.ProjectDevision.ProjectSectionTemproryViewModel;
using ViewModels.ProjectDevision.ProjectSectionRecoupmentViewModel;
using ViewModels.ProjectDevision.ProjectSectionRecoupmentListViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionService : IGenericService<ProjectSection>
    {
        IEnumerable<ListProjectSectionViewModel> GetAll(ListProjectSectionViewModel lpsvm, ref Paging pg);
        IEnumerable<ListProjectSection2ViewModel> GetAll2(ListProjectSection2ViewModel lpsvm, ref Paging pg);

        IEnumerable<ListProjectSectionAssignmentViewModel> GetProjectAssignment(ListProjectSectionAssignmentViewModel lpsavm, ref Paging pg);
        IEnumerable<ListProjectSectionOperationViewModel> GetProjectOperation(ListProjectSectionOperationViewModel lpsovm, ref Paging pg, Int64 MemberId);
        IEnumerable<ListProjectSectionTemproryViewModel> GetProjectTempDelivery(ListProjectSectionTemproryViewModel lpsovm, ref Paging pg, Int64 MemberId);
        IEnumerable<ListProjectSectionFixedViewModel> GetProjectFixedDelivery(ListProjectSectionFixedViewModel lpsovm, ref Paging pg, Int64 MemberId);
        IEnumerable<ListProjectSectionLiberalizationViewModel> GetProjectLiberalization(ListProjectSectionLiberalizationViewModel lpsovm, ref Paging pg);
        IEnumerable<ListProjectSection46ViewModel> GetProjectMadeh46(ListProjectSection46ViewModel lpsovm, ref Paging pg, Int64 MemberId);
        IEnumerable<ListProjectSection48ViewModel> GetProjectMadeh48(ListProjectSection48ViewModel lpsovm, ref Paging pg, Int64 MemberId);
        IEnumerable<ListProjectSectionLetterViewModel> GetProjectLetter(ListProjectSectionLetterViewModel lpsovm, ref Paging pg, Int64 MemberId);
        IEnumerable<ListProjectSectionRecoupmentListViewModel> GetProjectRecoupment(ListProjectSectionRecoupmentListViewModel lpsovm, ref Paging pg);
        IEnumerable<ReportProjectSectionViewModel> GetAllReport(ReportParameterProjectSectionViewModel parameters);
    }
}