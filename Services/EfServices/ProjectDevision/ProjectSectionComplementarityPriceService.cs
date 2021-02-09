using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionComplementarityPriceViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using ViewModels.ProjectDefine.ProjectSectionContractAmendmentReport;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionComplementarityPriceService : GenericService<ProjectSectionComplementarityPrice>, IProjectSectionComplementarityPriceService
    {
        public ProjectSectionComplementarityPriceService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionComplementarityPriceViewModel> GetAll(ListProjectSectionComplementarityPriceViewModel latvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ProjectSectionComplementarityPrice.", "");
            DynamicFiltering.GetFilter<ListProjectSectionComplementarityPriceViewModel>(latvm, ref pg);
            pg._filter = pg._filter.Replace("ProjectSectionComplementarityPrice.", "");

            var a = _uow.Set<ProjectSectionComplementarityPrice>().AsQueryable<ProjectSectionComplementarityPrice>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListProjectSectionComplementarityPriceViewModel()
            {
                ProjectSectionComplementarityPrice = new ProjectSectionComplementarityPrice() { Id = Result.Id, Date = Result.Date, DecreasePercent = Result.DecreasePercent, IncreasePercent = Result.IncreasePercent, NewPrice = Result.NewPrice, Number = Result.Number, ProjectSectionId = Result.ProjectSectionId }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionComplementarityPriceViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionComplementarityPriceService _ProjectSectionComplementarityPriceService = new ProjectSectionComplementarityPriceService(_uow);
            ProjectSectionComplementarityPriceFileService _ProjectSectionComplementarityPriceFileService = new ProjectSectionComplementarityPriceFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionComplementarityPrice.Id;

            if (isInsert == -1)
            {
                _ProjectSectionComplementarityPriceService.Insert(cpsivm.ProjectSectionComplementarityPrice);
            }
            else
            {
                _ProjectSectionComplementarityPriceService.Update(cpsivm.ProjectSectionComplementarityPrice);

                foreach (var item in _ProjectSectionComplementarityPriceFileService.Get(i => i.ProjectSectionComplementarityPriceId == cpsivm.ProjectSectionComplementarityPrice.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionComplementarityPriceFileService.Delete(item);
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
                            ProjectSectionComplementarityPriceFile ProjectSectionComplementarityPriceFile = new ProjectSectionComplementarityPriceFile();
                            ProjectSectionComplementarityPriceFile.ProjectSectionComplementarityPriceId = cpsivm.ProjectSectionComplementarityPrice.Id;
                            ProjectSectionComplementarityPriceFile.FileId = fileId;

                            _ProjectSectionComplementarityPriceFileService.Insert(ProjectSectionComplementarityPriceFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionComplementarityPrice.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }



        // کد محاسبه مجموع 
        public CreateProjectSectionComplementarityPriceViewModel GetContractPrice(long projectSectionId)
        {
            CreateProjectSectionComplementarityPriceViewModel cp = new CreateProjectSectionComplementarityPriceViewModel();

            try
            {
                long? contractPrice = 0;
                long? complementarityPrice = 0;
                long? TotalDPrice = 0;
                long? TotalIPrice = 0;

                ProjectSectionContractService _ProjectSectionContract = new ProjectSectionContractService(_uow);
                ProjectSectionComplementarityPriceService _ProjectSectionComplementarityPriceService = new ProjectSectionComplementarityPriceService(_uow);
                ProjectSectionPaidPriceService _ProjectSectionPaidPriceService = new ProjectSectionPaidPriceService(_uow);

                if (_ProjectSectionComplementarityPriceService._dbSet.Any(c => c.ProjectSectionId == projectSectionId))
                {
                    complementarityPrice = _ProjectSectionComplementarityPriceService._dbSet.OrderByDescending(c => c.Id).FirstOrDefault(c => c.ProjectSectionId == projectSectionId).NewPrice;
                    TotalIPrice = _ProjectSectionComplementarityPriceService._dbSet.Where(c => c.ProjectSectionId == projectSectionId).Sum(c => c.IncreasePercent);
                    TotalDPrice = _ProjectSectionComplementarityPriceService._dbSet.Where(c => c.ProjectSectionId == projectSectionId).Sum(c => c.DecreasePercent);
                }

                //if (contractPrice == 0 && _ProjectSectionContract._dbSet.Any(c => c.ProjectSectionId == projectSectionId))
                contractPrice = _ProjectSectionContract._dbSet.FirstOrDefault(c => c.ProjectSectionId == projectSectionId).ContractPrice.Value;

                cp.ContractPrice = (contractPrice == null) ? 0 : contractPrice.Value;
                cp.ComplementarityPrice = (complementarityPrice == null) ? 0 : complementarityPrice.Value;
                cp.TotalIPercent = (TotalIPrice == null) ? 0 : TotalIPrice.Value;
                cp.TotalDPercent = (TotalDPrice == null) ? 0 : TotalDPrice.Value;
            }
            catch (Exception)
            {}

            return cp;
        }



        public IEnumerable<ReportProjectSectionContractAmendmentViewModel> GetAllReport(ReportParameterProjectSectionContractAmendmentViewModel parameters)
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


            IEnumerable<ReportProjectSectionContractAmendmentViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionContractAmendmentViewModel>(@"SELECT        ProjectDefine.Projects.ProjectTitle, ProjectDefine.Projects.FileCode, ProjectDevision.ProjectSections.Title AS ProjectSectionTitle, 
                         ProjectDevision.ProjectSectionComplementarityPrices.Number, ProjectDevision.ProjectSectionComplementarityPrices.Date, 
                         ProjectDevision.ProjectSectionComplementarityPrices.IncreasePercent AS IncreasePrice, ProjectDevision.ProjectSectionComplementarityPrices.DecreasePercent AS DecreasePrice, 
                         ProjectDevision.ProjectSectionComplementarityPrices.NewPrice, BaseInformation.Contractors.CompanyName
FROM            ProjectDefine.Projects LEFT JOIN
                         ProjectDevision.ProjectSections ON ProjectDefine.Projects.Id = ProjectDevision.ProjectSections.ProjectId LEFT JOIN
                         ProjectDevision.ProjectSectionComplementarityPrices ON 
                         ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionComplementarityPrices.ProjectSectionId LEFT JOIN
                         ProjectDevision.ProjectSectionWinners ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionWinners.ProjectSectionId LEFT JOIN
                         BaseInformation.Contractors ON ProjectDevision.ProjectSectionWinners.ContractorId = BaseInformation.Contractors.Id
                       " + filter)
                .Select(Result => new ReportProjectSectionContractAmendmentViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    CompanyName = Result.CompanyName,
                    Date = Result.Date,
                    NewPrice = Result.NewPrice,
                    IncreasePrice = Result.IncreasePrice,
                    DecreasePrice = Result.DecreasePrice,
                    Number = Result.Number,
                    AmendmentType = "ماده 29",
                });
            //var test = result.ToList();
            return result;
        }
    }
}
