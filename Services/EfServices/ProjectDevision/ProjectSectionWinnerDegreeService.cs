using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionWinnerDegreeViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionWinnerDegreeService : GenericService<ProjectSectionWinnerDegree>, IProjectSectionWinnerDegreeService
    {
        public ProjectSectionWinnerDegreeService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionWinnerDegreeViewModel> GetAll(ListProjectSectionWinnerDegreeViewModel latvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ProjectSectionWinnerDegree.", "");
            DynamicFiltering.GetFilter<ListProjectSectionWinnerDegreeViewModel>(latvm, ref pg);
            pg._filter = pg._filter.Replace("ProjectSectionWinnerDegree.", "");

            var a = _uow.Set<ProjectSectionWinnerDegree>().AsQueryable<ProjectSectionWinnerDegree>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListProjectSectionWinnerDegreeViewModel()
            {
                ProjectSectionWinnerDegree = new ProjectSectionWinnerDegree() { Id = Result.Id, ProjectSectionId = Result.ProjectSectionId, ReciveDate = Result.ReciveDate }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionWinnerDegreeViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionWinnerDegreeService _ProjectSectionWinnerDegreeService = new ProjectSectionWinnerDegreeService(_uow);
            ProjectSectionWinnerDegreeFileService _ProjectSectionWinnerDegreeFileService = new ProjectSectionWinnerDegreeFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionWinnerDegree.Id;

            if (isInsert == -1)
            {
                if (!_ProjectSectionWinnerDegreeService.Get(i => i.ProjectSectionId == cpsivm.ProjectSectionWinnerDegree.ProjectSectionId.Value).Any())
                {
                    var projectSection = _projectSectionService.Get(i => i.Id == cpsivm.ProjectSectionWinnerDegree.ProjectSectionId.Value).FirstOrDefault();
                    projectSection.ProjectSectionAssignmentStateId = Convert.ToInt64(GeneralEnums.ProjectSectionAssignmentStates.ProjectSectionContract);
                }

                _ProjectSectionWinnerDegreeService.Insert(cpsivm.ProjectSectionWinnerDegree);
            }
            else
            {
                _ProjectSectionWinnerDegreeService.Update(cpsivm.ProjectSectionWinnerDegree);

                foreach (var item in _ProjectSectionWinnerDegreeFileService.Get(i => i.ProjectSectionWinnerDegreeId == cpsivm.ProjectSectionWinnerDegree.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionWinnerDegreeFileService.Delete(item);
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
                            ProjectSectionWinnerDegreeFile ProjectSectionWinnerDegreeFile = new ProjectSectionWinnerDegreeFile();
                            ProjectSectionWinnerDegreeFile.ProjectSectionWinnerDegreeId = cpsivm.ProjectSectionWinnerDegree.Id;
                            ProjectSectionWinnerDegreeFile.FileId = fileId;

                            _ProjectSectionWinnerDegreeFileService.Insert(ProjectSectionWinnerDegreeFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionWinnerDegree.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

    }
}
