using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionCeremonialViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionCeremonialService : GenericService<ProjectSectionCeremonial>, IProjectSectionCeremonialService
    {
        public ProjectSectionCeremonialService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionCeremonialViewModel> GetAll(ListProjectSectionCeremonialViewModel latvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ProjectSectionCeremonial.", "");
            DynamicFiltering.GetFilter<ListProjectSectionCeremonialViewModel>(latvm, ref pg);
            pg._filter = pg._filter.Replace("ProjectSectionCeremonial.", "");

            var a = _uow.Set<ProjectSectionCeremonial>().AsQueryable<ProjectSectionCeremonial>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListProjectSectionCeremonialViewModel()
            {
                ProjectSectionCeremonial = new ProjectSectionCeremonial() { Id = Result.Id, JustificationDate = Result.JustificationDate, JustificationNumber = Result.JustificationNumber, ProjectSectionId = Result.ProjectSectionId }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionCeremonialViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionCeremonialService _ProjectSectionCeremonialService = new ProjectSectionCeremonialService(_uow);
            ProjectSectionCeremonialFileService _ProjectSectionCeremonialFileService = new ProjectSectionCeremonialFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionCeremonial.Id;

            if (isInsert == -1)
            {
                if (!_ProjectSectionCeremonialService.Get(i => i.ProjectSectionId == cpsivm.ProjectSectionCeremonial.ProjectSectionId.Value).Any())
                {
                    var projectSection = _projectSectionService.Get(i => i.Id == cpsivm.ProjectSectionCeremonial.ProjectSectionId.Value).FirstOrDefault();
                    projectSection.ProjectSectionAssignmentStateId = Convert.ToInt64(GeneralEnums.ProjectSectionAssignmentStates.ProjectSectionWinner);
                }

                _ProjectSectionCeremonialService.Insert(cpsivm.ProjectSectionCeremonial);
            }
            else
            {
                _ProjectSectionCeremonialService.Update(cpsivm.ProjectSectionCeremonial);

                foreach (var item in _ProjectSectionCeremonialFileService.Get(i => i.ProjectSectionCeremonialId == cpsivm.ProjectSectionCeremonial.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionCeremonialFileService.Delete(item);
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
                            ProjectSectionCeremonialFile ProjectSectionCeremonialFile = new ProjectSectionCeremonialFile();
                            ProjectSectionCeremonialFile.ProjectSectionCeremonialId = cpsivm.ProjectSectionCeremonial.Id;
                            ProjectSectionCeremonialFile.FileId = fileId;

                            _ProjectSectionCeremonialFileService.Insert(ProjectSectionCeremonialFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionCeremonial.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

    }
}
