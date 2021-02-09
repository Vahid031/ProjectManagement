using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionScheduleViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionScheduleService : GenericService<ProjectSectionSchedule>, IProjectSectionScheduleService
    {
        public ProjectSectionScheduleService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionScheduleViewModel> GetAll(ListProjectSectionScheduleViewModel latvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectSectionScheduleViewModel>(latvm, ref pg);

            var a =
            _uow.Set<ProjectSectionSchedule>()
                .Join(_uow.Set<Opinion>(), ProjectSectionSchedule => ProjectSectionSchedule.OpinionId, Opinion => Opinion.Id, (ProjectSectionSchedule, Opinion) => new { ProjectSectionSchedule, Opinion });

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionScheduleViewModel()
            {
                ProjectSectionSchedule = new ProjectSectionSchedule() { Id = Result.ProjectSectionSchedule.Id, Description = Result.ProjectSectionSchedule.Description, OpinionId = Result.ProjectSectionSchedule.OpinionId, ProjectSectionId = Result.ProjectSectionSchedule.ProjectSectionId },
                Opinion = new Opinion() { Id = Result.Opinion.Id, Title = Result.Opinion.Title }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionScheduleViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionScheduleService _ProjectSectionScheduleService = new ProjectSectionScheduleService(_uow);
            ProjectSectionScheduleFileService _ProjectSectionScheduleFileService = new ProjectSectionScheduleFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionSchedule.Id;

            if (isInsert == -1)
            {
                if (!_ProjectSectionScheduleService.Get(i => i.ProjectSectionId == cpsivm.ProjectSectionSchedule.ProjectSectionId.Value).Any())
                {
                    var projectSection = _projectSectionService.Get(i => i.Id == cpsivm.ProjectSectionSchedule.ProjectSectionId.Value).FirstOrDefault();
                    projectSection.ProjectSectionOperationStateId = Convert.ToInt64(GeneralEnums.ProjectSectionOperationStates.ProjectSectionAdvisorSupervisorVisit);
                }

                _ProjectSectionScheduleService.Insert(cpsivm.ProjectSectionSchedule);
            }
            else
            {
                _ProjectSectionScheduleService.Update(cpsivm.ProjectSectionSchedule);

                foreach (var item in _ProjectSectionScheduleFileService.Get(i => i.ProjectSectionScheduleId == cpsivm.ProjectSectionSchedule.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionScheduleFileService.Delete(item);
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
                            ProjectSectionScheduleFile ProjectSectionScheduleFile = new ProjectSectionScheduleFile();
                            ProjectSectionScheduleFile.ProjectSectionScheduleId = cpsivm.ProjectSectionSchedule.Id;
                            ProjectSectionScheduleFile.FileId = fileId;

                            _ProjectSectionScheduleFileService.Insert(ProjectSectionScheduleFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionSchedule.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

    }
}
