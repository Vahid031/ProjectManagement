using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionMemberViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.General;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionMemberService : GenericService<ProjectSectionMember>, IProjectSectionMemberService
    {
        public ProjectSectionMemberService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionMemberViewModel> GetAllAdvisor(ListProjectSectionMemberViewModel latvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectSectionMemberViewModel>(latvm, ref pg);

            var a =
            _uow.Set<ProjectSectionMember>()
                .Join(_uow.Set<Member>(), ProjectSectionMember => ProjectSectionMember.MemberId, Member => Member.Id, (ProjectSectionMember, Member) => new { ProjectSectionMember, Member });
            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionMemberViewModel()
            {
                ProjectSectionMember = new ProjectSectionMember() { Id = Result.ProjectSectionMember.Id, Description = Result.ProjectSectionMember.Description, ProjectSectionId = Result.ProjectSectionMember.ProjectSectionId, EndDate = Result.ProjectSectionMember.EndDate, MemberId = Result.ProjectSectionMember.MemberId, MonitoringTypeId = Result.ProjectSectionMember.MonitoringTypeId, StartDate = Result.ProjectSectionMember.StartDate },
                Member = new Member() { Id = Result.Member.Id, FirstName = Result.Member.FirstName, LastName = Result.Member.LastName, NationalCode = Result.Member.NationalCode, UserName = Result.Member.UserName, Password = Result.Member.Password, Email = Result.Member.Email, Address = Result.Member.Address, IsActive = Result.Member.IsActive },
            });
        }

        public IEnumerable<ListProjectSectionMemberViewModel> GetAllSuppervisor(ListProjectSectionMemberViewModel latvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectSectionMemberViewModel>(latvm, ref pg);

            var a =
            _uow.Set<ProjectSectionMember>()
                .Join(_uow.Set<Member>(), ProjectSectionMember => ProjectSectionMember.MemberId, Member => Member.Id, (ProjectSectionMember, Member) => new { ProjectSectionMember, Member })
                .Join(_uow.Set<MonitoringType>(), ProjectSectionMember => ProjectSectionMember.ProjectSectionMember.MonitoringTypeId, MonitoringType => MonitoringType.Id, (ProjectSectionMember, MonitoringType) => new { ProjectSectionMember.ProjectSectionMember, ProjectSectionMember.Member, MonitoringType });

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            a = a.Where(i => i.Member.MemberGroupId == 2 || i.Member.MemberGroupId == 4);

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionMemberViewModel()
            {
                ProjectSectionMember = new ProjectSectionMember() { Id = Result.ProjectSectionMember.Id, Description = Result.ProjectSectionMember.Description, ProjectSectionId = Result.ProjectSectionMember.ProjectSectionId, EndDate = Result.ProjectSectionMember.EndDate, MemberId = Result.ProjectSectionMember.MemberId, MonitoringTypeId = Result.ProjectSectionMember.MonitoringTypeId, StartDate = Result.ProjectSectionMember.StartDate, IsMainSupervisor = Result.ProjectSectionMember.IsMainSupervisor },
                Member = new Member() { Id = Result.Member.Id, FirstName = Result.Member.FirstName, LastName = Result.Member.LastName, NationalCode = Result.Member.NationalCode, UserName = Result.Member.UserName, Password = Result.Member.Password, Email = Result.Member.Email, Address = Result.Member.Address, IsActive = Result.Member.IsActive },
                MonitoringType = new MonitoringType() { Id = Result.MonitoringType.Id, Title = Result.MonitoringType.Title }
            });
        }

    }
}
