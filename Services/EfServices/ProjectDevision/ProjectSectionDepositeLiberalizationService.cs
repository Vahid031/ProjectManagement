using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionDepositeLiberalizationViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionDepositeLiberalizationService : GenericService<ProjectSectionDepositeLiberalization>, IProjectSectionDepositeLiberalizationService
    {
        public ProjectSectionDepositeLiberalizationService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionDepositeLiberalizationViewModel> GetAll(ListProjectSectionDepositeLiberalizationViewModel lcvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ProjectSectionDepositeLiberalization.", "");
            DynamicFiltering.GetFilter<ListProjectSectionDepositeLiberalizationViewModel>(lcvm, ref pg);
            pg._filter = pg._filter.Replace("ProjectSectionDepositeLiberalization.", "");

            var a = _uow.Set<ProjectSectionDepositeLiberalization>().AsQueryable<ProjectSectionDepositeLiberalization>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListProjectSectionDepositeLiberalizationViewModel()
            {
                ProjectSectionDepositeLiberalization = new ProjectSectionDepositeLiberalization() { Id = Result.Id, ProjectSectionId = Result.ProjectSectionId, Description = Result.Description, FixedPrice = Result.FixedPrice, PaidDate = Result.PaidDate, TemproryPrice = Result.TemproryPrice }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionDepositeLiberalizationViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionDepositeLiberalizationService _ProjectSectionDepositeLiberalizationService = new ProjectSectionDepositeLiberalizationService(_uow);
            ProjectSectionDepositeLiberalizationFileService _ProjectSectionDepositeLiberalizationFileService = new ProjectSectionDepositeLiberalizationFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionDepositeLiberalization.Id;

            if (isInsert == -1)
            {
                _ProjectSectionDepositeLiberalizationService.Insert(cpsivm.ProjectSectionDepositeLiberalization);
            }
            else
            {
                _ProjectSectionDepositeLiberalizationService.Update(cpsivm.ProjectSectionDepositeLiberalization);

                foreach (var item in _ProjectSectionDepositeLiberalizationFileService.Get(i => i.ProjectSectionDepositeLiberalizationId == cpsivm.ProjectSectionDepositeLiberalization.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionDepositeLiberalizationFileService.Delete(item);
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
                            ProjectSectionDepositeLiberalizationFile ProjectSectionDepositeLiberalizationFile = new ProjectSectionDepositeLiberalizationFile();
                            ProjectSectionDepositeLiberalizationFile.ProjectSectionDepositeLiberalizationId = cpsivm.ProjectSectionDepositeLiberalization.Id;
                            ProjectSectionDepositeLiberalizationFile.FileId = fileId;

                            _ProjectSectionDepositeLiberalizationFileService.Insert(ProjectSectionDepositeLiberalizationFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionDepositeLiberalization.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }



        public IEnumerable<ReportProjectSectionDepositeLiberalizationsViewModel> GetAllReport(ReportParameterProjectSectionDepositeLiberalizationsViewModel parameters)
        {
            string tempFilter = " ";
            string and = "";

            if (parameters.FileCode != null)
            {
                tempFilter += and;
                tempFilter += "Result.FileCode Like N'%" + parameters.FileCode + "%'";
                and = " AND ";
            }

            if (parameters.ProjectTitle != null)
            {
                tempFilter += and;
                tempFilter += "Result.ProjectTitle Like N'%" + parameters.ProjectTitle + "%'";
                and = " AND ";
            }

            if (parameters.Title != null)
            {
                tempFilter += and;
                tempFilter += "Result.Title Like N'%" + parameters.Title + "%'";
                and = " AND ";
            }


            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string allField = @"SELECT * FROM (";
            string query = "";

            query += @"Select PDP.FileCode, PDP.ProjectTitle, PDPS.Title, PDPSDL.PaidDate, PDPSDL.TemproryPrice, PDPSDL.FixedPrice
                       From ProjectDevision.ProjectSectionDepositeLiberalizations PDPSDL
                       	   inner join ProjectDevision.ProjectSections PDPS on PDPS.Id = PDPSDL.ProjectSectionId
                       	   inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                       ) Result"
            + filter;

            IEnumerable<ReportProjectSectionDepositeLiberalizationsViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionDepositeLiberalizationsViewModel>(allField + query)
                .Select(Result => new ReportProjectSectionDepositeLiberalizationsViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    Title = Result.Title,
                    PaidDate = Result.PaidDate,
                    TemproryPrice = Result.TemproryPrice,
                    FixedPrice = Result.FixedPrice
                });

            return result;
        }

    }
}
