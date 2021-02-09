using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using DomainModels.Entities.ProjectDefine;
using Services.Interfaces.ProjectDefine;
using ViewModels.ProjectDefine.ProjectFundingViewModel;
using DomainModels.Entities.BaseInformation;
using Services.EfServices.General;
using DomainModels.Enums;
using Services.EfServices.BaseInformation;

namespace Services.EfServices.ProjectDefine
{
    public class ProjectFundingService : GenericService<ProjectFunding>, IProjectFundingService
    {
        public ProjectFundingService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectFundingViewModel> GetAll(ListProjectFundingViewModel lcvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectFundingViewModel>(lcvm, ref pg);

            var a =
            _uow.Set<ProjectFunding>()
                .Join(_uow.Set<ResourceType>(), ProjectFunding => ProjectFunding.ResourceTypeId, ResourceType => ResourceType.Id, (ProjectFunding, ResourceType) => new { ProjectFunding, ResourceType })
                .Join(_uow.Set<Resource>(), ProjectFunding => ProjectFunding.ResourceType.ResourceId, Resource => Resource.Id, (ProjectFunding, Resource) => new { ProjectFunding.ProjectFunding, ProjectFunding.ResourceType, Resource })
                .Join(_uow.Set<FinantialYear>(), ProjectFunding => ProjectFunding.ProjectFunding.FinantialYearId, FinantialYear => FinantialYear.Id, (ProjectFunding, FinantialYear) => new { ProjectFunding.ProjectFunding, ProjectFunding.ResourceType, ProjectFunding.Resource, FinantialYear })
                ;

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectFundingViewModel()
            {
                ProjectFunding = new ProjectFunding() { Id = Result.ProjectFunding.Id, ProjectId = Result.ProjectFunding.ProjectId, ResourceTypeId = Result.ProjectFunding.ResourceTypeId, Price = Result.ProjectFunding.Price, FinantialYearId = Result.ProjectFunding.FinantialYearId },
                Resource = new Resource() { Id = Result.Resource.Id, Title = Result.Resource.Title },
                ResourceType = new ResourceType() { Id = Result.ResourceType.Id, Title = Result.ResourceType.Title, ResourceId = Result.ResourceType.ResourceId },
                FinantialYear = new FinantialYear() { Id = Result.FinantialYear.Id, Title = Result.FinantialYear.Title, IsActive = Result.FinantialYear.IsActive }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectFundingViewModel cpavm, Int64 memberId, List<string> paths)
        {
            ProjectFundingService _ProjectFundingService = new ProjectFundingService(_uow);
            ProjectFundingFileService _ProjectFundingFileService = new ProjectFundingFileService(_uow);

            FileService _fileService = new FileService(_uow);

            long? isInsert = cpavm.ProjectFunding.Id;

            if (isInsert == -1)
            {
                _ProjectFundingService.Insert(cpavm.ProjectFunding);
            }
            else
            {
                _ProjectFundingService.Update(cpavm.ProjectFunding);

                foreach (var item in _ProjectFundingFileService.Get(i => i.ProjectFundingId == cpavm.ProjectFunding.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectFundingFileService.Delete(item);
                    }
                }
            }


            if (cpavm.Files != null)
            {
                foreach (string item in cpavm.Files.Split(','))
                {
                    if (item != "")
                    {
                        Int64 fileId = Convert.ToInt64(item);
                        if (_fileService.Get(i => i.Id == fileId).First().FileState.Value == 1)
                        {
                            ProjectFundingFile ProjectFundingFile = new ProjectFundingFile();
                            ProjectFundingFile.ProjectFundingId = cpavm.ProjectFunding.Id;
                            ProjectFundingFile.FileId = fileId;

                            _ProjectFundingFileService.Insert(ProjectFundingFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(memberId) > 0)
            {
                ProjectService _projectService = new ProjectService(_uow);

                var project = _projectService.Get(c => c.Id == cpavm.ProjectFunding.ProjectId).FirstOrDefault();
                project.ProjectStateId = Convert.ToInt64(GeneralEnums.ProjectState.WaitingAlocation);
                _projectService.Update(project);
                _uow.SaveChanges(memberId);

                return new KeyValuePair<long, string>(cpavm.ProjectFunding.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }
    }
}
