using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionMemberViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionMemberService : IGenericService<ProjectSectionMember>
    {
        IEnumerable<ListProjectSectionMemberViewModel> GetAllAdvisor(ListProjectSectionMemberViewModel lpscvm, ref Paging pg);
        IEnumerable<ListProjectSectionMemberViewModel> GetAllSuppervisor(ListProjectSectionMemberViewModel lpscvm, ref Paging pg);
    }
}