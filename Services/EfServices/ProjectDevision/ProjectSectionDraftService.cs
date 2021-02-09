using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionDraftViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionDraftService : GenericService<ProjectSectionDraft>, IProjectSectionDraftService
    {
        public ProjectSectionDraftService(IUnitOfWork uow)
            : base(uow)
        {

        }


        public IEnumerable<ListProjectSectionDraftViewModel> GetAll(ListProjectSectionDraftViewModel ldtvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ProjectSectionDraft.", "");
            DynamicFiltering.GetFilter<ListProjectSectionDraftViewModel>(ldtvm, ref pg);
            pg._filter = pg._filter.Replace("ProjectSectionDraft.", "");

            var a = _uow.Set<ProjectSectionDraft>().AsQueryable<ProjectSectionDraft>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            var test = a.ToList();

            return a.AsEnumerable().Select(Result => new ListProjectSectionDraftViewModel()
            {
                ProjectSectionDraft = new ProjectSectionDraft() { Id = Result.Id, DraftPrice = Result.DraftPrice, TempDraftPrice = Result.TempDraftPrice, DraftDate_ = ConvertDate.GetShamsi(Result.DraftDate), DraftNumber = Result.DraftNumber }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionDraftViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            if (cpsivm.ProjectSectionDraft.DraftDate_ != null)
            {
                cpsivm.ProjectSectionDraft.DraftDate = ConvertDate.GetMiladi(cpsivm.ProjectSectionDraft.DraftDate_).Value;
                cpsivm.ProjectSectionDraft.DraftDate_ = null;
            }

            ProjectSectionDraftService _ProjectSectionDraftService = new ProjectSectionDraftService(_uow);
            ProjectSectionDraftFileService _ProjectSectionDraftFileService = new ProjectSectionDraftFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            ProjectSectionStatementConfirmService _projectSectionStatementConfirmService = new ProjectSectionStatementConfirmService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionDraft.Id;

            //چک کردن اینکه مبلغ صدور حواله بیشتر از تایید صورت وضعیت نشود
            Int64 projectSectionConfirmPrice = _projectSectionStatementConfirmService.Find(cpsivm.ProjectSectionDraft.ProjectSectionStatementConfirmId).Price.Value;
            var projectSectionDrafts = _ProjectSectionDraftService.Get(i => i.Id != cpsivm.ProjectSectionDraft.Id && i.ProjectSectionStatementConfirmId == cpsivm.ProjectSectionDraft.ProjectSectionStatementConfirmId);
            Int64 draftPrice = cpsivm.ProjectSectionDraft.TempDraftPrice.Value;

            foreach (var projectSectionDraft in projectSectionDrafts)
            {
                draftPrice += projectSectionDraft.TempDraftPrice.Value;    
            }

            if (draftPrice > projectSectionConfirmPrice)
            {
                return new KeyValuePair<long, string>(-1, "مبلغ حواله نمیتواند بیشتر از مبلغ تایید شده صورت وضعیت باشد.");
            }

            if (isInsert == -1)
            {
                _ProjectSectionDraftService.Insert(cpsivm.ProjectSectionDraft);
            }
            else
            {
                _ProjectSectionDraftService.Update(cpsivm.ProjectSectionDraft);

                foreach (var item in _ProjectSectionDraftFileService.Get(i => i.ProjectSectionDraftId == cpsivm.ProjectSectionDraft.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionDraftFileService.Delete(item);
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
                            ProjectSectionDraftFile ProjectSectionDraftFile = new ProjectSectionDraftFile();
                            ProjectSectionDraftFile.ProjectSectionDraftId = cpsivm.ProjectSectionDraft.Id;
                            ProjectSectionDraftFile.FileId = fileId;

                            _ProjectSectionDraftFileService.Insert(ProjectSectionDraftFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionDraft.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

    }
}
