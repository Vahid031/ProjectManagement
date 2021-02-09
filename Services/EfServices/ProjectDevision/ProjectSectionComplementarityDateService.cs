using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionComplementarityDateViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using ViewModels.ProjectDefine.ProjectSectionContractAmendmentDateReport;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionComplementarityDateService : GenericService<ProjectSectionComplementarityDate>, IProjectSectionComplementarityDateService
    {
        public ProjectSectionComplementarityDateService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionComplementarityDateViewModel> GetAll(ListProjectSectionComplementarityDateViewModel latvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ProjectSectionComplementarityDate.", "");
            DynamicFiltering.GetFilter<ListProjectSectionComplementarityDateViewModel>(latvm, ref pg);
            pg._filter = pg._filter.Replace("ProjectSectionComplementarityDate.", "");

            var a = _uow.Set<ProjectSectionComplementarityDate>().AsQueryable<ProjectSectionComplementarityDate>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListProjectSectionComplementarityDateViewModel()
            {
                ProjectSectionComplementarityDate = new ProjectSectionComplementarityDate() { Id = Result.Id, Date = Result.Date, Number = Result.Number, ProjectSectionId = Result.ProjectSectionId, NewDate = Result.NewDate }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionComplementarityDateViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionComplementarityDateService _ProjectSectionComplementarityDateService = new ProjectSectionComplementarityDateService(_uow);
            ProjectSectionComplementarityDateFileService _ProjectSectionComplementarityDateFileService = new ProjectSectionComplementarityDateFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionComplementarityDate.Id;

            if (isInsert == -1)
            {
                _ProjectSectionComplementarityDateService.Insert(cpsivm.ProjectSectionComplementarityDate);
            }
            else
            {
                _ProjectSectionComplementarityDateService.Update(cpsivm.ProjectSectionComplementarityDate);

                foreach (var item in _ProjectSectionComplementarityDateFileService.Get(i => i.ProjectSectionComplementarityDateId == cpsivm.ProjectSectionComplementarityDate.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionComplementarityDateFileService.Delete(item);
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
                            ProjectSectionComplementarityDateFile ProjectSectionComplementarityDateFile = new ProjectSectionComplementarityDateFile();
                            ProjectSectionComplementarityDateFile.ProjectSectionComplementarityDateId = cpsivm.ProjectSectionComplementarityDate.Id;
                            ProjectSectionComplementarityDateFile.FileId = fileId;

                            _ProjectSectionComplementarityDateFileService.Insert(ProjectSectionComplementarityDateFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionComplementarityDate.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }



        public IEnumerable<ReportProjectSectionContractAmendmentDateViewModel> GetAllReport(ReportParameterProjectSectionContractAmendmentDateViewModel parameters)
        {
            string tempFilter = " ";
            string and = "";

            if (parameters.FileCode != null)
            {
                tempFilter += and;
                tempFilter += "ProjectDefine.Projects.FileCode = N'" + parameters.FileCode + "'";
                and = " AND ";
            }

            if (parameters.ProjectTitle != null)
            {
                tempFilter += and;
                tempFilter += "ProjectDefine.Projects.ProjectTitle = N'" + parameters.ProjectTitle + "'";
                and = " AND ";
            }

            if (parameters.ProjectSectionTitle != null)
            {
                tempFilter += and;
                tempFilter += "ProjectDevision.ProjectSections.Title = N'" + parameters.ProjectSectionTitle + "'";
                and = " AND ";
            }



            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;


            IEnumerable<ReportProjectSectionContractAmendmentDateViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionContractAmendmentDateViewModel>(@"SELECT        ProjectDevision.ProjectSectionComplementarityDates.Number AS LetterNumber, ProjectDevision.ProjectSectionComplementarityDates.Date AS LetterDate, 
                         ProjectDevision.ProjectSectionComplementarityDates.NewDate, ProjectDefine.Projects.ProjectTitle, ProjectDefine.Projects.FileCode, 
                         ProjectDevision.ProjectSections.Title AS ProjectSectionTitle, BaseInformation.Contractors.CompanyName
FROM            ProjectDevision.ProjectSectionComplementarityDates LEFT JOIN
                         ProjectDevision.ProjectSections ON ProjectDevision.ProjectSectionComplementarityDates.ProjectSectionId = ProjectDevision.ProjectSections.Id LEFT JOIN
                         ProjectDefine.Projects ON ProjectDevision.ProjectSections.ProjectId = ProjectDefine.Projects.Id LEFT JOIN
                         ProjectDevision.ProjectSectionWinners ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionWinners.ProjectSectionId LEFT JOIN
                         BaseInformation.Contractors ON ProjectDevision.ProjectSectionWinners.ContractorId = BaseInformation.Contractors.Id
                       " + filter)
                .Select(Result => new ReportProjectSectionContractAmendmentDateViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    CompanyName = Result.CompanyName,
                    NewDate = Result.NewDate,
                    LetterDate = Result.LetterDate,
                    LetterNumber = Result.LetterNumber,
                });
            //var test = result.ToList();
            return result;
        }
    }
}
