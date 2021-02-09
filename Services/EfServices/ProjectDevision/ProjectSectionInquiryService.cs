using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionInquiryViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionInquiryService : GenericService<ProjectSectionInquiry>, IProjectSectionInquiryService
    {
        public ProjectSectionInquiryService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionInquiryViewModel> GetAll(ListProjectSectionInquiryViewModel latvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ProjectSectionInquiry.", "");
            DynamicFiltering.GetFilter<ListProjectSectionInquiryViewModel>(latvm, ref pg);
            pg._filter = pg._filter.Replace("ProjectSectionInquiry.", "");

            var a = _uow.Set<ProjectSectionInquiry>().AsQueryable<ProjectSectionInquiry>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListProjectSectionInquiryViewModel()
            {
                ProjectSectionInquiry = new ProjectSectionInquiry() { Id = Result.Id, InquiryDate = Result.InquiryDate, InquiryNumber = Result.InquiryNumber, ProjectSectionId = Result.ProjectSectionId }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionInquiryViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionInquiryService _projectSectionInquiryService = new ProjectSectionInquiryService(_uow);
            ProjectSectionInquiryFileService _projectSectionInquiryFileService = new ProjectSectionInquiryFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionInquiry.Id;

            if (isInsert == -1)
            {
                if (!_projectSectionInquiryService.Get(i => i.ProjectSectionId == cpsivm.ProjectSectionInquiry.ProjectSectionId.Value).Any())
                {
                    var projectSection = _projectSectionService.Get(i => i.Id == cpsivm.ProjectSectionInquiry.ProjectSectionId.Value).FirstOrDefault();
                    projectSection.ProjectSectionAssignmentStateId = Convert.ToInt64(GeneralEnums.ProjectSectionAssignmentStates.ProjectSectionWinner);
                }

                _projectSectionInquiryService.Insert(cpsivm.ProjectSectionInquiry);
            }
            else
            {
                _projectSectionInquiryService.Update(cpsivm.ProjectSectionInquiry);

                foreach (var item in _projectSectionInquiryFileService.Get(i => i.ProjectSectionInquiryId == cpsivm.ProjectSectionInquiry.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _projectSectionInquiryFileService.Delete(item);
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
                            ProjectSectionInquiryFile projectSectionInquiryFile = new ProjectSectionInquiryFile();
                            projectSectionInquiryFile.ProjectSectionInquiryId = cpsivm.ProjectSectionInquiry.Id;
                            projectSectionInquiryFile.FileId = fileId;

                            _projectSectionInquiryFileService.Insert(projectSectionInquiryFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionInquiry.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

    }
}
