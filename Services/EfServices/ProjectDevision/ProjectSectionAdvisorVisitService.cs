using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionAdvisorVisitViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.General;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionAdvisorVisitService : GenericService<ProjectSectionAdvisorVisit>, IProjectSectionAdvisorVisitService
    {
        public ProjectSectionAdvisorVisitService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionAdvisorVisitViewModel> GetAll(ListProjectSectionAdvisorVisitViewModel latvm, ref Paging pg, Int64 MemberId)
        {
            DynamicFiltering.GetFilter<ListProjectSectionAdvisorVisitViewModel>(latvm, ref pg);

            var a =
            _uow.Set<ProjectSectionAdvisorVisit>()
                .Join(_uow.Set<ProjectSectionMember>(), ProjectSectionAdvisorVisit => ProjectSectionAdvisorVisit.ProjectSectionMemberId, ProjectSectionMember => ProjectSectionMember.Id, (ProjectSectionAdvisorVisit, ProjectSectionMember) => new { ProjectSectionAdvisorVisit, ProjectSectionMember })
                .Join(_uow.Set<Member>(), ProjectSectionAdvisorVisit => ProjectSectionAdvisorVisit.ProjectSectionMember.MemberId, Member => Member.Id, (ProjectSectionAdvisorVisit, Member) => new { ProjectSectionAdvisorVisit.ProjectSectionAdvisorVisit, ProjectSectionAdvisorVisit.ProjectSectionMember, Member });

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            if (MemberId != 0)
                a = a.Where(i => i.Member.Id == MemberId);

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionAdvisorVisitViewModel()
            {
                ProjectSectionAdvisorVisit = new ProjectSectionAdvisorVisit() { Id = Result.ProjectSectionAdvisorVisit.Id, Description = Result.ProjectSectionAdvisorVisit.Description, ProjectSectionId = Result.ProjectSectionAdvisorVisit.ProjectSectionId, Agendum = Result.ProjectSectionAdvisorVisit.Agendum, ProjectSectionMemberId = Result.ProjectSectionAdvisorVisit.ProjectSectionMemberId, VisitDate = Result.ProjectSectionAdvisorVisit.VisitDate },
                ProjectSectionMember = new ProjectSectionMember() { Id = Result.ProjectSectionMember.Id, Description = Result.ProjectSectionMember.Description, ProjectSectionId = Result.ProjectSectionMember.ProjectSectionId, EndDate = Result.ProjectSectionMember.EndDate, MemberId = Result.ProjectSectionMember.MemberId, MonitoringTypeId = Result.ProjectSectionMember.MonitoringTypeId, StartDate = Result.ProjectSectionMember.StartDate },
                Member = new Member() { Id = Result.Member.Id, FirstName = Result.Member.FirstName, LastName = Result.Member.LastName, NationalCode = Result.Member.NationalCode, UserName = Result.Member.UserName, Password = Result.Member.Password, Email = Result.Member.Email, Address = Result.Member.Address, IsActive = Result.Member.IsActive },
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionAdvisorVisitViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionAdvisorVisitService _ProjectSectionAdvisorVisitService = new ProjectSectionAdvisorVisitService(_uow);
            ProjectSectionAdvisorVisitFileService _ProjectSectionAdvisorVisitFileService = new ProjectSectionAdvisorVisitFileService(_uow);
            
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionAdvisorVisit.Id;

            if (isInsert == -1)
            {
                _ProjectSectionAdvisorVisitService.Insert(cpsivm.ProjectSectionAdvisorVisit);
            }
            else
            {
                _ProjectSectionAdvisorVisitService.Update(cpsivm.ProjectSectionAdvisorVisit);

                foreach (var item in _ProjectSectionAdvisorVisitFileService.Get(i => i.ProjectSectionAdvisorVisitId == cpsivm.ProjectSectionAdvisorVisit.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionAdvisorVisitFileService.Delete(item);
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
                            ProjectSectionAdvisorVisitFile ProjectSectionAdvisorVisitFile = new ProjectSectionAdvisorVisitFile();
                            ProjectSectionAdvisorVisitFile.ProjectSectionAdvisorVisitId = cpsivm.ProjectSectionAdvisorVisit.Id;
                            ProjectSectionAdvisorVisitFile.FileId = fileId;

                            _ProjectSectionAdvisorVisitFileService.Insert(ProjectSectionAdvisorVisitFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionAdvisorVisit.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

    }
}
