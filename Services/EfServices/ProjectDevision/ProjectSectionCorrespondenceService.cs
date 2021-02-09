using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionCorrespondenceViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionCorrespondenceService : GenericService<ProjectSectionCorrespondence>, IProjectSectionCorrespondenceService
    {
        public ProjectSectionCorrespondenceService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionCorrespondenceViewModel> GetAll(ListProjectSectionCorrespondenceViewModel lcvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectSectionCorrespondenceViewModel>(lcvm, ref pg);

            var a =
            _uow.Set<ProjectSectionCorrespondence>()
                .Join(_uow.Set<LetterType>(), ProjectSectionCorrespondence => ProjectSectionCorrespondence.LetterTypeId, LetterType => LetterType.Id, (ProjectSectionCorrespondence, LetterType) => new { ProjectSectionCorrespondence, LetterType });

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionCorrespondenceViewModel()
            {
                ProjectSectionCorrespondence = new ProjectSectionCorrespondence() { Id = Result.ProjectSectionCorrespondence.Id, LetterDate = Result.ProjectSectionCorrespondence.LetterDate, LetterIssue = Result.ProjectSectionCorrespondence.LetterIssue, LetterNumber = Result.ProjectSectionCorrespondence.LetterNumber, LetterTypeId = Result.ProjectSectionCorrespondence.LetterTypeId, ProjectSectionId = Result.ProjectSectionCorrespondence.ProjectSectionId },
                LetterType = new LetterType() { Id = Result.LetterType.Id, Title = Result.LetterType.Title }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionCorrespondenceViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionCorrespondenceService _ProjectSectionCorrespondenceService = new ProjectSectionCorrespondenceService(_uow);
            ProjectSectionCorrespondenceFileService _ProjectSectionCorrespondenceFileService = new ProjectSectionCorrespondenceFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionCorrespondence.Id;

            if (isInsert == -1)
            {
                _ProjectSectionCorrespondenceService.Insert(cpsivm.ProjectSectionCorrespondence);
            }
            else
            {
                _ProjectSectionCorrespondenceService.Update(cpsivm.ProjectSectionCorrespondence);

                foreach (var item in _ProjectSectionCorrespondenceFileService.Get(i => i.ProjectSectionCorrespondenceId == cpsivm.ProjectSectionCorrespondence.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionCorrespondenceFileService.Delete(item);
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
                            ProjectSectionCorrespondenceFile ProjectSectionCorrespondenceFile = new ProjectSectionCorrespondenceFile();
                            ProjectSectionCorrespondenceFile.ProjectSectionCorrespondenceId = cpsivm.ProjectSectionCorrespondence.Id;
                            ProjectSectionCorrespondenceFile.FileId = fileId;

                            _ProjectSectionCorrespondenceFileService.Insert(ProjectSectionCorrespondenceFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionCorrespondence.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

    }
}
