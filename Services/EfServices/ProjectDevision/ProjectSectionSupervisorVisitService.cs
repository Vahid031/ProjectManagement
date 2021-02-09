using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionSupervisorVisitViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.General;
using ViewModels.ProjectDefine.ProjectSectionProjectProgressReportViewModel;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionSupervisorVisitService : GenericService<ProjectSectionSupervisorVisit>, IProjectSectionSupervisorVisitService
    {
        public ProjectSectionSupervisorVisitService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionSupervisorVisitViewModel> GetAll(ListProjectSectionSupervisorVisitViewModel latvm, ref Paging pg, Int64 MemberId)
        {
            DynamicFiltering.GetFilter<ListProjectSectionSupervisorVisitViewModel>(latvm, ref pg);

            var a =
            _uow.Set<ProjectSectionSupervisorVisit>()
                .Join(_uow.Set<ProjectSectionMember>(), ProjectSectionSupervisorVisit => ProjectSectionSupervisorVisit.ProjectSectionMemberId, ProjectSectionMember => ProjectSectionMember.Id, (ProjectSectionSupervisorVisit, ProjectSectionMember) => new { ProjectSectionSupervisorVisit, ProjectSectionMember })
                .Join(_uow.Set<Member>(), ProjectSectionSupervisorVisit => ProjectSectionSupervisorVisit.ProjectSectionMember.MemberId, Member => Member.Id, (ProjectSectionSupervisorVisit, Member) => new { ProjectSectionSupervisorVisit.ProjectSectionSupervisorVisit, ProjectSectionSupervisorVisit.ProjectSectionMember, Member })
                .Join(_uow.Set<ProjectSectionOperationTitle>(), ProjectSectionSupervisorVisit => ProjectSectionSupervisorVisit.ProjectSectionSupervisorVisit.ProjectSectionOperationTitleId, PSOT => PSOT.Id, (ProjectSectionSupervisorVisit, ProjectSectionOperationTitle) => new { ProjectSectionSupervisorVisit.ProjectSectionSupervisorVisit, ProjectSectionSupervisorVisit.ProjectSectionMember, ProjectSectionSupervisorVisit.Member, ProjectSectionOperationTitle })
                ;
            var query = a.ToList();
            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            if (MemberId != 0)
                a = a.Where(i => i.Member.Id == MemberId);

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionSupervisorVisitViewModel()
            {
                ProjectSectionSupervisorVisit = new ProjectSectionSupervisorVisit() { Id = Result.ProjectSectionSupervisorVisit.Id, Description = Result.ProjectSectionSupervisorVisit.Description, ProjectSectionId = Result.ProjectSectionSupervisorVisit.ProjectSectionId, Agendum = Result.ProjectSectionSupervisorVisit.Agendum, DevelopmentPercent = Result.ProjectSectionSupervisorVisit.DevelopmentPercent, ProjectSectionMemberId = Result.ProjectSectionSupervisorVisit.ProjectSectionMemberId, VisitDate = Result.ProjectSectionSupervisorVisit.VisitDate },
                ProjectSectionMember = new ProjectSectionMember() { Id = Result.ProjectSectionMember.Id, Description = Result.ProjectSectionMember.Description, ProjectSectionId = Result.ProjectSectionMember.ProjectSectionId, EndDate = Result.ProjectSectionMember.EndDate, MemberId = Result.ProjectSectionMember.MemberId, MonitoringTypeId = Result.ProjectSectionMember.MonitoringTypeId, StartDate = Result.ProjectSectionMember.StartDate },
                Member = new Member() { Id = Result.Member.Id, FirstName = Result.Member.FirstName, LastName = Result.Member.LastName, NationalCode = Result.Member.NationalCode, UserName = Result.Member.UserName, Password = Result.Member.Password, Email = Result.Member.Email, Address = Result.Member.Address, IsActive = Result.Member.IsActive },
                ProjectSectionOperationTitle = new ProjectSectionOperationTitle() { Id = Result.ProjectSectionOperationTitle.Id, Title = Result.ProjectSectionOperationTitle.Title }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionSupervisorVisitViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionSupervisorVisitService _ProjectSectionSupervisorVisitService = new ProjectSectionSupervisorVisitService(_uow);
            ProjectSectionSupervisorVisitFileService _ProjectSectionSupervisorVisitFileService = new ProjectSectionSupervisorVisitFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionSupervisorVisit.Id;

            if (isInsert == -1)
            {
                _ProjectSectionSupervisorVisitService.Insert(cpsivm.ProjectSectionSupervisorVisit);
            }
            else
            {
                _ProjectSectionSupervisorVisitService.Update(cpsivm.ProjectSectionSupervisorVisit);

                foreach (var item in _ProjectSectionSupervisorVisitFileService.Get(i => i.ProjectSectionSupervisorVisitId == cpsivm.ProjectSectionSupervisorVisit.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionSupervisorVisitFileService.Delete(item);
                    }
                }
            }

            if (cpsivm.Files != null)
            {
                foreach (string item in cpsivm.Files.Split(','))
                {
                    if (item != "")
                    {
                        Int64 fileId = Convert.ToInt64(item);
                        if (_fileService.Get(i => i.Id == fileId).First().FileState.Value == 1)
                        {
                            ProjectSectionSupervisorVisitFile ProjectSectionSupervisorVisitFile = new ProjectSectionSupervisorVisitFile();
                            ProjectSectionSupervisorVisitFile.ProjectSectionSupervisorVisitId = cpsivm.ProjectSectionSupervisorVisit.Id;
                            ProjectSectionSupervisorVisitFile.FileId = fileId;

                            _ProjectSectionSupervisorVisitFileService.Insert(ProjectSectionSupervisorVisitFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionSupervisorVisit.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

        public IEnumerable<ReportProjectSectionSupervisorVisitsViewModel> GetAllReport(ReportParameterProjectSectionSupervisorVisitsViewModel parameters)
        {
            string tempFilter = " ";
            string and = "";

            if (parameters.FileCode != null)
            {
                tempFilter += and;
                tempFilter += "ProjectDefine.Projects.FileCode Like N'%" + parameters.FileCode + "%'";
                and = " AND ";
            }

            if (parameters.ProjectTitle != null)
            {
                tempFilter += and;
                tempFilter += "ProjectDefine.Projects.ProjectTitle Like N'%" + parameters.ProjectTitle + "%'";
                and = " AND ";
            }

            if (parameters.Title != null)
            {
                tempFilter += and;
                tempFilter += "ProjectDevision.ProjectSections.Title Like N'%" + parameters.Title + "%'";
                and = " AND ";
            }

            if (parameters.CityId != null)
            {
                tempFilter += and;
                tempFilter += "BaseInformation.Cities.Id = " + parameters.CityId;
                and = " AND ";
            }

            if (parameters.MemberId != null)
            {
                tempFilter += and;
                tempFilter += "General.Members.Id = " + parameters.MemberId;
                and = " AND ";
            }

            if (parameters.FromDate != null)
            {
                tempFilter += and;
                tempFilter += "ProjectDevision.ProjectSectionMembers.StartDate Like N'%" + parameters.FromDate + "%'";
                and = " AND ";
            }

            if (parameters.ToDate != null)
            {
                tempFilter += and;
                tempFilter += "ProjectDevision.ProjectSectionMembers.EndDate Like N'%" + parameters.ToDate + "%'";
                and = " AND ";
            }


            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            //string allField = @"SELECT * FROM (";
            //string query = "";

//            query += @"Select PDP.FileCode, PDP.ProjectTitle, PDPS.Title, PDP.CityId, BIC.Title CityTitle, GM.FirstName + ' ' + GM.LastName MemberName,
//                       	   PDPSSV.VisitDate, PDPSSV.ProjectSectionOperationTitleId, PDPSOT.Title ProjectSectionOperationTitle, PDPSSV.Agendum, PDPSSV.DevelopmentPercent,
//                       	   PDPSSV.[Description], PDPSM.MemberId
//                       From ProjectDevision.ProjectSectionSupervisorVisits PDPSSV
//                       	   LEFT join ProjectDevision.ProjectSectionMembers PDPSM on PDPSSV.ProjectSectionMemberId = PDPSM.Id
//                       	   LEFT join General.Members GM on GM.Id = PDPSM.MemberId
//                       	   LEFT join ProjectDevision.ProjectSections PDPS on PDPS.Id = PDPSM.ProjectSectionId 
//                       	   LEFT join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
//                       	   LEFT join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
//                       	   left join ProjectDevision.ProjectSectionOperationTitles PDPSOT on PDPSOT.Id = PDPSSV.ProjectSectionOperationTitleId
//                       ) Result"
//            + filter;
            IEnumerable<ReportProjectSectionSupervisorVisitsViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionSupervisorVisitsViewModel>(@"SELECT        ProjectDefine.Projects.ProjectTitle, ProjectDefine.Projects.FileCode, ProjectDevision.ProjectSections.Title, BaseInformation.Cities.Title AS CityTitle, 
                         (General.Members.FirstName+' '+General.Members.LastName) AS MemberName, ProjectDevision.ProjectSectionSupervisorVisits.VisitDate AS VisitDate, 
                         SUM(ProjectDevision.ProjectSectionSupervisorVisits.DevelopmentPercent) AS DevelopmentPercent, ProjectDevision.ProjectSectionSupervisorVisits.Agendum AS Agendum, 
                         ProjectDevision.ProjectSectionSupervisorVisits.[Description] AS [Description], ProjectDevision.ProjectSectionOperationTitles.Title AS ProjectSectionOperationTitle
FROM            ProjectDefine.Projects LEFT OUTER JOIN
                         ProjectDevision.ProjectSections ON ProjectDefine.Projects.Id = ProjectDevision.ProjectSections.ProjectId LEFT OUTER JOIN
                         BaseInformation.Cities ON ProjectDefine.Projects.CityId = BaseInformation.Cities.Id LEFT OUTER JOIN
                         ProjectDevision.ProjectSectionMembers ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionMembers.ProjectSectionId LEFT OUTER JOIN
                         General.Members ON ProjectDevision.ProjectSectionMembers.MemberId = General.Members.Id LEFT OUTER JOIN
                         ProjectDevision.ProjectSectionSupervisorVisits ON 
                         ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionSupervisorVisits.ProjectSectionId LEFT OUTER JOIN
                         ProjectDevision.ProjectSectionOperationTitles ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionOperationTitles.ProjectSectionId
                         " + filter + @"Group by ProjectDefine.Projects.ProjectTitle, ProjectDefine.Projects.FileCode, ProjectDevision.ProjectSections.Title, BaseInformation.Cities.Title,
						 General.Members.FirstName, General.Members.LastName, ProjectDevision.ProjectSectionSupervisorVisits.VisitDate,
						 ProjectDevision.ProjectSectionSupervisorVisits.Agendum,ProjectDevision.ProjectSectionSupervisorVisits.[Description], ProjectDevision.ProjectSectionOperationTitles.Title")
                .Select(Result => new ReportProjectSectionSupervisorVisitsViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    Title = Result.Title,
                    CityId = Result.CityId,
                    CityTitle = Result.CityTitle,
                    MemberName = Result.MemberName,
                    VisitDate = Result.VisitDate,
                    ProjectSectionOperationTitleId = Result.ProjectSectionOperationTitleId,
                    ProjectSectionOperationTitle = Result.ProjectSectionOperationTitle,
                    Agendum = Result.Agendum,
                    DevelopmentPercent = Result.DevelopmentPercent,
                    Description = Result.Description
                });
            var test = result.ToList();
            return result;
        }



        public IEnumerable<ReportProjectSectionProjectProgressViewModel> GetAllReportb(ReportParameterProjectSectionProjectProgressViewModel parameters)
        {
            string tempFilter = " ";
            string and = "";

            if (parameters.FileCode != null)
            {
                tempFilter += and;
                tempFilter += "ProjectDefine.Projects.FileCode = " + parameters.FileCode;
                and = " AND ";
            }

            if (parameters.ProjectTitle != null)
            {
                tempFilter += and;
                tempFilter += "ProjectDefine.Projects.ProjectTitle = N'" + parameters.ProjectTitle + "'";
                and = " AND ";
            }

            if (parameters.ProjectSectionTitle != null)
            {
                tempFilter += and;
                tempFilter += "ProjectDevision.ProjectSections.Title = N'" + parameters.ProjectSectionTitle + "'";
                and = " AND ";
            }
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;
            IEnumerable<ReportProjectSectionProjectProgressViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionProjectProgressViewModel>(@"SELECT        ProjectDefine.Projects.ProjectTitle, ProjectDefine.Projects.FileCode, ProjectDevision.ProjectSections.Title AS ProjectSectionTitle, 
                         SUM(ProjectDevision.ProjectSectionSupervisorVisits.DevelopmentPercent) AS DevelopmentPercent, BaseInformation.Cities.Title AS City, 
                         ProjectDevision.ProjectSectionOperationTitles.Title AS OperationTitle, ProjectDevision.ProjectSectionSupervisorVisits.VisitDate AS VisitDate, (General.Members.FirstName+ ' ' + 
                         General.Members.LastName) AS SupervisorName
FROM            ProjectDevision.ProjectSectionOperationTitles FULL OUTER JOIN
                         ProjectDevision.ProjectSectionMembers INNER JOIN
                         ProjectDevision.ProjectSectionSupervisorVisits ON 
                         ProjectDevision.ProjectSectionMembers.Id = ProjectDevision.ProjectSectionSupervisorVisits.ProjectSectionMemberId INNER JOIN
                         General.Members ON ProjectDevision.ProjectSectionMembers.MemberId = General.Members.Id FULL OUTER JOIN
                         ProjectDefine.Projects RIGHT OUTER JOIN
                         ProjectDevision.ProjectSections ON ProjectDefine.Projects.Id = ProjectDevision.ProjectSections.ProjectId LEFT OUTER JOIN
                         BaseInformation.Cities ON ProjectDefine.Projects.CityId = BaseInformation.Cities.Id ON 
                         ProjectDevision.ProjectSectionSupervisorVisits.ProjectSectionId = ProjectDevision.ProjectSections.Id ON 
                         ProjectDevision.ProjectSectionOperationTitles.ProjectSectionId = ProjectDevision.ProjectSections.Id
                       " + filter + @"group by 	ProjectDefine.Projects.ProjectTitle, ProjectDefine.Projects.FileCode, ProjectDevision.ProjectSections.Title, BaseInformation.Cities.Title, 
                         ProjectDevision.ProjectSectionOperationTitles.Title, ProjectDevision.ProjectSectionSupervisorVisits.VisitDate, General.Members.FirstName, 
                         General.Members.LastName")
                .Select(Result => new ReportProjectSectionProjectProgressViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    City = Result.City,
                    DevelopmentPercent = Result.DevelopmentPercent,
                    VisitDate = Result.VisitDate,
                    SupervisorName = Result.SupervisorName,
                    OperationTitle = Result.OperationTitle
                });
            var test = result.ToList();
            return result;

        }
    }
}
