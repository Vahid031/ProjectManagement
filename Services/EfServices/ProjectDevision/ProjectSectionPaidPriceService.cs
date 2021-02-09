using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionPaidPriceViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;
using ViewModels.ProjectDefine.ProjectSectionStatementPayReportViewModel;



namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionPaidPriceService : GenericService<ProjectSectionPaidPrice>, IProjectSectionPaidPriceService
    {
        public ProjectSectionPaidPriceService(IUnitOfWork uow)
            : base(uow)
        {
        }


        public IEnumerable<ListProjectSectionPaidPriceViewModel> GetAll(ListProjectSectionPaidPriceViewModel ldtvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ProjectSectionPaidPrice.", "");
            DynamicFiltering.GetFilter<ListProjectSectionPaidPriceViewModel>(ldtvm, ref pg);
            pg._filter = pg._filter.Replace("ProjectSectionPaidPrice.", "");

            var a = _uow.Set<ProjectSectionPaidPrice>().AsQueryable<ProjectSectionPaidPrice>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            var test = a.ToList();

            

            return a.AsEnumerable().Select(Result => new ListProjectSectionPaidPriceViewModel()
            {
                ProjectSectionPaidPrice = new ProjectSectionPaidPrice() { Id = Result.Id, DraftPrice = Result.DraftPrice, DraftDate_ = ConvertDate.GetShamsi(Result.DraftDate), PaidDate_ = ConvertDate.GetShamsi(Result.PaidDate), PayablePrice = Result.PayablePrice, PaidPrice = Result.PaidPrice }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionPaidPriceViewModel cpsivm, Int64 MemberId, List<string> paths)
        {

            if (cpsivm.ProjectSectionPaidPrice.DraftDate_ != null)
            {
                cpsivm.ProjectSectionPaidPrice.DraftDate = ConvertDate.GetMiladi(cpsivm.ProjectSectionPaidPrice.DraftDate_).Value;
                cpsivm.ProjectSectionPaidPrice.DraftDate_ = null;
            }

            if (cpsivm.ProjectSectionPaidPrice.PaidDate_ != null)
            {
                cpsivm.ProjectSectionPaidPrice.PaidDate = ConvertDate.GetMiladi(cpsivm.ProjectSectionPaidPrice.PaidDate_).Value;
                cpsivm.ProjectSectionPaidPrice.PaidDate_ = null;
            }

            ProjectSectionPaidPriceService _ProjectSectionPaidPriceService = new ProjectSectionPaidPriceService(_uow);
            ProjectSectionPaidPriceFileService _ProjectSectionPaidPriceFileService = new ProjectSectionPaidPriceFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionPaidPrice.Id;

            if (isInsert == -1)
            {
                _ProjectSectionPaidPriceService.Insert(cpsivm.ProjectSectionPaidPrice);
            }
            else
            {
                _ProjectSectionPaidPriceService.Update(cpsivm.ProjectSectionPaidPrice);

                foreach (var item in _ProjectSectionPaidPriceFileService.Get(i => i.ProjectSectionPaidPriceId == cpsivm.ProjectSectionPaidPrice.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionPaidPriceFileService.Delete(item);
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
                            ProjectSectionPaidPriceFile ProjectSectionPaidPriceFile = new ProjectSectionPaidPriceFile();
                            ProjectSectionPaidPriceFile.ProjectSectionPaidPriceId = cpsivm.ProjectSectionPaidPrice.Id;
                            ProjectSectionPaidPriceFile.FileId = fileId;

                            _ProjectSectionPaidPriceFileService.Insert(ProjectSectionPaidPriceFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionPaidPrice.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }





        public IEnumerable<CalculateProjectSectionPaidPriceViewModel> CalculatePadePriceList(long ProjectSectionId)
        {
            IEnumerable<CalculateProjectSectionPaidPriceViewModel> GetList = _uow.GetInstance().Database.SqlQuery<CalculateProjectSectionPaidPriceViewModel>("exec CalculatePadePriceList " + ProjectSectionId)
            .Select(Result => new CalculateProjectSectionPaidPriceViewModel()
            {
                ContractPrice = Result.ContractPrice,
                TotalDraftPrice = Result.TotalDraftPrice,
                TotalPadePrice = Result.TotalPadePrice,
                Penalty46Price = Result.Penalty46Price,
                RemainingDraftPrice = Result.RemainingDraftPrice,
                RemainingTotalContractPrice = Result.RemainingTotalContractPrice
            });

            return GetList.ToList();
        }








        public bool CalculateCanInsert(long? projectSectionId, long price)
        {
            long contractPrice = 0;
            long padePrice = 0;

            ProjectSectionContractService _ProjectSectionContract = new ProjectSectionContractService(_uow);
            ProjectSectionComplementarityPriceService _ProjectSectionComplementarityPriceService = new ProjectSectionComplementarityPriceService(_uow);
            ProjectSectionPaidPriceService _ProjectSectionPaidPriceService = new ProjectSectionPaidPriceService(_uow);

            var tempContractPrice = _ProjectSectionComplementarityPriceService._dbSet.OrderByDescending(c => c.Id).FirstOrDefault(c => c.ProjectSectionId == projectSectionId); ;

            if (tempContractPrice != null)
                contractPrice = tempContractPrice.NewPrice.Value;
            else
                contractPrice = _ProjectSectionContract._dbSet.FirstOrDefault(c => c.ProjectSectionId == projectSectionId).ContractPrice.Value;

            padePrice = _ProjectSectionPaidPriceService._dbSet.Where(c => c.ProjectSectionId == projectSectionId).ToList().Sum(c => c.PaidPrice).Value;

            if ((padePrice + price) > contractPrice)
                return false;
            else
                return true;
        }

        

        public IEnumerable<ReportProjectSectionStatementPayViewModel> GetAllReport(ReportParameterProjectSectionStatementPayViewModel parameters)
        {
            if ((parameters.FileCode == null)&&(parameters.ProjectTitle != null)&&(parameters.ProjectSectionTitle != null)&&(parameters.StatementNumberId != null))
            {
                 IEnumerable<ReportProjectSectionStatementPayViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionStatementPayViewModel>(@"SELECT        ProjectDevision.ProjectSectionPaidPrices.DraftPrice, ProjectDevision.ProjectSectionPaidPrices.Deposit, ProjectDevision.ProjectSectionPaidPrices.PrePayment, 
                         ProjectDevision.ProjectSectionPaidPrices.Forfeit, ProjectDevision.ProjectSectionPaidPrices.Tax, ProjectDevision.ProjectSectionPaidPrices.InsuranceTypeText, 
                         ProjectDevision.ProjectSectionPaidPrices.MasterInsurance, ProjectDevision.ProjectSectionPaidPrices.PayablePrice, 
                         ProjectDevision.ProjectSectionPaidPrices.InsuranceType, ProjectDevision.ProjectSectionPaidPrices.InsuranceTypeText AS Expr1, 
                         ProjectDevision.ProjectSectionPaidPrices.PaidPrice, ProjectDevision.ProjectSectionStatements.ContractDate AS StatementDate, 
                         BaseInformation.StatementTypes.Title AS StatementType, BaseInformation.StatementNumbers.Title AS StatementNumber, ProjectDefine.Projects.FileCode, 
                         ProjectDefine.Projects.ProjectTitle, ProjectDevision.ProjectSections.Title AS ProjectSectionTitle, BaseInformation.Contractors.CompanyName 
FROM            ProjectDevision.ProjectSectionWinners INNER JOIN
                         ProjectDevision.ProjectSections ON ProjectDevision.ProjectSectionWinners.ProjectSectionId = ProjectDevision.ProjectSections.Id INNER JOIN
                         BaseInformation.Contractors ON ProjectDevision.ProjectSectionWinners.ContractorId = BaseInformation.Contractors.Id RIGHT OUTER JOIN
                         ProjectDevision.ProjectSectionPaidPrices ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionPaidPrices.ProjectSectionId LEFT OUTER JOIN
                         ProjectDevision.ProjectSectionStatements ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionStatements.ProjectSectionId LEFT OUTER JOIN
                         BaseInformation.StatementNumbers ON ProjectDevision.ProjectSectionStatements.StatementNumberId = BaseInformation.StatementNumbers.Id LEFT OUTER JOIN
                         BaseInformation.StatementTypes ON ProjectDevision.ProjectSectionStatements.StatementTypeId = BaseInformation.StatementTypes.Id LEFT OUTER JOIN
                         ProjectDefine.Projects ON ProjectDevision.ProjectSections.ProjectId = ProjectDefine.Projects.Id
                       ").Select(Result => new ReportProjectSectionStatementPayViewModel()
                {
                    StatementNumber = Result.StatementNumber,
                    StatementType = Result.StatementType,
                    StatementDate = Result.StatementDate,
                    InsuranceType = Result.InsuranceType,
                    InsuranceTypeText = ChangeValue.ChangeValues((int)Result.InsuranceType),
                    DraftPrice = Result.DraftPrice,
                    Deposit = Result.Deposit,
                    PrePayment = Result.PrePayment,
                    Forfeit = Result.Forfeit,
                    MasterInsurance = Result.MasterInsurance,
                    Tax = Result.Tax,
                    PayablePrice = Result.PayablePrice,
                    PaidPrice = Result.PaidPrice,
                    FileCode = Result.FileCode,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    ProjectTitle = Result.ProjectTitle,
                });
                 return result;
            }
            else
            {
                string tempFilter = " ";
                string and = "";

                if (parameters.FileCode != null)
                {
                    tempFilter += and;
                    tempFilter += "ProjectDefine.Projects.FileCode = " + parameters.FileCode;
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

                if (parameters.StatementNumberId != null)
                {
                    tempFilter += and;
                    tempFilter += "BaseInformation.StatementNumbers.Id = " + parameters.StatementNumberId;
                    and = " AND ";
                }
                string filter = "";
                if (!String.IsNullOrEmpty(tempFilter.Trim()))
                    filter = " where " + tempFilter;

                IEnumerable<ReportProjectSectionStatementPayViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionStatementPayViewModel>(@"SELECT        ProjectDevision.ProjectSectionPaidPrices.DraftPrice, ProjectDevision.ProjectSectionPaidPrices.Deposit, ProjectDevision.ProjectSectionPaidPrices.PrePayment, 
                         ProjectDevision.ProjectSectionPaidPrices.Forfeit, ProjectDevision.ProjectSectionPaidPrices.Tax, ProjectDevision.ProjectSectionPaidPrices.InsuranceTypeText, 
                         ProjectDevision.ProjectSectionPaidPrices.MasterInsurance, ProjectDevision.ProjectSectionPaidPrices.PayablePrice, 
                         ProjectDevision.ProjectSectionPaidPrices.InsuranceType, ProjectDevision.ProjectSectionPaidPrices.InsuranceTypeText AS Expr1, 
                         ProjectDevision.ProjectSectionPaidPrices.PaidPrice, ProjectDevision.ProjectSectionStatements.ContractDate AS StatementDate, 
                         BaseInformation.StatementTypes.Title AS StatementType, BaseInformation.StatementNumbers.Title AS StatementNumber, ProjectDefine.Projects.FileCode, 
                         ProjectDefine.Projects.ProjectTitle, ProjectDevision.ProjectSections.Title AS ProjectSectionTitle, BaseInformation.Contractors.CompanyName 
FROM            ProjectDevision.ProjectSectionWinners INNER JOIN
                         ProjectDevision.ProjectSections ON ProjectDevision.ProjectSectionWinners.ProjectSectionId = ProjectDevision.ProjectSections.Id INNER JOIN
                         BaseInformation.Contractors ON ProjectDevision.ProjectSectionWinners.ContractorId = BaseInformation.Contractors.Id RIGHT OUTER JOIN
                         ProjectDevision.ProjectSectionPaidPrices ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionPaidPrices.ProjectSectionId LEFT OUTER JOIN
                         ProjectDevision.ProjectSectionStatements ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionStatements.ProjectSectionId LEFT OUTER JOIN
                         BaseInformation.StatementNumbers ON ProjectDevision.ProjectSectionStatements.StatementNumberId = BaseInformation.StatementNumbers.Id LEFT OUTER JOIN
                         BaseInformation.StatementTypes ON ProjectDevision.ProjectSectionStatements.StatementTypeId = BaseInformation.StatementTypes.Id LEFT OUTER JOIN
                         ProjectDefine.Projects ON ProjectDevision.ProjectSections.ProjectId = ProjectDefine.Projects.Id
                       " + filter).Select(Result => new ReportProjectSectionStatementPayViewModel()
                         {
                             StatementNumber = Result.StatementNumber,
                             StatementType = Result.StatementType,
                             StatementDate = Result.StatementDate,
                             InsuranceType = Result.InsuranceType,
                             InsuranceTypeText = ChangeValue.ChangeValues((int)Result.InsuranceType),
                             DraftPrice = Result.DraftPrice,
                             Deposit = Result.Deposit,
                             PrePayment = Result.PrePayment,
                             Forfeit = Result.Forfeit,
                             MasterInsurance = Result.MasterInsurance,
                             Tax = Result.Tax,
                             PayablePrice = Result.PayablePrice,
                             PaidPrice = Result.PaidPrice,
                             FileCode = Result.FileCode,
                             ProjectSectionTitle = Result.ProjectSectionTitle,
                             ProjectTitle = Result.ProjectTitle,
                         });
                var test = result.ToList();

                return result;
            }                                 
        }
    }
}
