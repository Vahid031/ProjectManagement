using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionTenderViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionTenderService : GenericService<ProjectSectionTender>, IProjectSectionTenderService
    {
        public ProjectSectionTenderService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionTenderViewModel> GetAll(ListProjectSectionTenderViewModel latvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ProjectSectionTender.", "");
            DynamicFiltering.GetFilter<ListProjectSectionTenderViewModel>(latvm, ref pg);
            pg._filter = pg._filter.Replace("ProjectSectionTender.", "");

            var a = _uow.Set<ProjectSectionTender>().AsQueryable<ProjectSectionTender>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListProjectSectionTenderViewModel()
            {
                ProjectSectionTender = new ProjectSectionTender() { Id = Result.Id, DiffusionDate = Result.DiffusionDate, NewspaperNumber = Result.NewspaperNumber, NewspaperTitle = Result.NewspaperTitle, ProjectSectionId = Result.ProjectSectionId }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionTenderViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionTenderService _ProjectSectionTenderService = new ProjectSectionTenderService(_uow);
            ProjectSectionTenderFileService _ProjectSectionTenderFileService = new ProjectSectionTenderFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionTender.Id;

            if (isInsert == -1)
            {
                if (!_ProjectSectionTenderService.Get(i => i.ProjectSectionId == cpsivm.ProjectSectionTender.ProjectSectionId.Value).Any())
                {
                    var projectSection = _projectSectionService.Get(i => i.Id == cpsivm.ProjectSectionTender.ProjectSectionId.Value).FirstOrDefault();
                    projectSection.ProjectSectionAssignmentStateId = Convert.ToInt64(GeneralEnums.ProjectSectionAssignmentStates.ProjectSectionParticipant);
                }

                _ProjectSectionTenderService.Insert(cpsivm.ProjectSectionTender);
            }
            else
            {
                _ProjectSectionTenderService.Update(cpsivm.ProjectSectionTender);

                foreach (var item in _ProjectSectionTenderFileService.Get(i => i.ProjectSectionTenderId == cpsivm.ProjectSectionTender.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionTenderFileService.Delete(item);
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
                            ProjectSectionTenderFile ProjectSectionTenderFile = new ProjectSectionTenderFile();
                            ProjectSectionTenderFile.ProjectSectionTenderId = cpsivm.ProjectSectionTender.Id;
                            ProjectSectionTenderFile.FileId = fileId;

                            _ProjectSectionTenderFileService.Insert(ProjectSectionTenderFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionTender.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

    }
}
