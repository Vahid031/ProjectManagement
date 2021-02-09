using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionStatementViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;
using ViewModels.ProjectDefine.ProjectStatementViewModel;
//public static class ConfirmedChecking
//{
//    public static string ConfirmedChecking(int num)
//    {
//        DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
//        //int value = 0;
//        //var q=
//        return "";
//    }
//}

namespace Services.EfServices.ProjectDevision
{
    
    public class ProjectSectionStatementService : GenericService<ProjectSectionStatement>, IProjectSectionStatementService
    {
        public ProjectSectionStatementService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionStatementViewModel> GetAll(ListProjectSectionStatementViewModel lcvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectSectionStatementViewModel>(lcvm, ref pg);

            var a =
            _uow.Set<ProjectSectionStatement>()
                .Join(_uow.Set<Opinion>(), ProjectSectionStatement => ProjectSectionStatement.TechnicalOpinionId, Opinion => Opinion.Id, (ProjectSectionStatement, Opinion) => new { ProjectSectionStatement, Opinion })
                .Join(_uow.Set<StatementType>(), ProjectSectionStatement => ProjectSectionStatement.ProjectSectionStatement.StatementTypeId, StatementType => StatementType.Id, (ProjectSectionStatement, StatementType) => new { ProjectSectionStatement.ProjectSectionStatement, ProjectSectionStatement.Opinion, StatementType })
                .Join(_uow.Set<StatementNumber>(), ProjectSectionStatement => ProjectSectionStatement.ProjectSectionStatement.StatementNumberId, StatementNumber => StatementNumber.Id, (ProjectSectionStatement, StatementNumber) => new { ProjectSectionStatement.ProjectSectionStatement, ProjectSectionStatement.Opinion, ProjectSectionStatement.StatementType, StatementNumber })
                .Join(_uow.Set<TempFixed>(), ProjectSectionStatement => ProjectSectionStatement.StatementNumber.TempFixedId, TempFixed => TempFixed.Id, (ProjectSectionStatement, TempFixed) => new { ProjectSectionStatement.ProjectSectionStatement, ProjectSectionStatement.Opinion, ProjectSectionStatement.StatementType, ProjectSectionStatement.StatementNumber, TempFixed });
            //.Join(_uow.Set<ProjectSectionStatementConfirm>, ProjectSectionStatement => ProjectSectionStatement.ProjectSectionStatement.Id,ProjectSectionStatementConfirm => ProjectSectionStatementConfirm.ProjectSectionStatementId,(ProjectSectionStatement,ProjectSectionStatementConfirm)=> new{projectstatementCon})


            if (pg._filter != "")
                //var x = new { StatementStatus="0"};  
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionStatementViewModel()
            {
                ProjectSectionStatement = new ProjectSectionStatement() { Id = Result.ProjectSectionStatement.Id, AdvisorPrice = Result.ProjectSectionStatement.AdvisorPrice, ConfirmPrice = Result.ProjectSectionStatement.ConfirmPrice, ContractPrice = Result.ProjectSectionStatement.ContractPrice, ProjectSectionId = Result.ProjectSectionStatement.ProjectSectionId, ProjectSectionStatementStateId = Result.ProjectSectionStatement.ProjectSectionStatementStateId, StatementNumberId = Result.ProjectSectionStatement.StatementNumberId, StatementTypeId = Result.ProjectSectionStatement.StatementTypeId, TechnicalOpinionId = Result.ProjectSectionStatement.TechnicalOpinionId },
                Opinion = new Opinion() { Id = Result.Opinion.Id, Title = Result.Opinion.Title },
                StatementType = new StatementType() { Id = Result.StatementType.Id, Title = Result.StatementType.Title },
                TempFixed = new TempFixed() { Id = Result.TempFixed.Id, Title = Result.TempFixed.Title },
                StatementNumber = new StatementNumber() { Id = Result.StatementNumber.Id, TempFixedId = Result.StatementNumber.TempFixedId, Title = Result.StatementNumber.Title },
                //StatementStatus=StatusTxt(Result.ProjectSectionStatement.Id)
            });
        }
        
        
        public KeyValuePair<Int64, string> Save(CreateProjectSectionStatementViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionStatementService _ProjectSectionStatementService = new ProjectSectionStatementService(_uow);
            ProjectSectionStatementFileService _ProjectSectionStatementFileService = new ProjectSectionStatementFileService(_uow);

            FileService _fileService = new FileService(_uow);

            if (cpsivm.TempFixedId == 2)
            {
                ProjectSectionComplementarityDateService projectSectionComplementarityDateService = new ProjectSectionComplementarityDateService(_uow);

                if (projectSectionComplementarityDateService.Get(i => i.ProjectSectionId == cpsivm.ProjectSectionStatement.ProjectSectionId).Any())
                {
                    if (String.IsNullOrEmpty(cpsivm.Files))
                    {
                        return new KeyValuePair<long, string>(-1, "صورت وضعیت دارای ماده 30 می باشد و پیوست الزامی است");
                    }
                }

                ProjectSectionComplementarityPriceService projectSectionComplementarityPriceService = new ProjectSectionComplementarityPriceService(_uow);

                if (projectSectionComplementarityPriceService.Get(i => i.ProjectSectionId == cpsivm.ProjectSectionStatement.ProjectSectionId).Any())
                {
                    if (String.IsNullOrEmpty(cpsivm.Files))
                    {
                        return new KeyValuePair<long, string>(-1, "صورت وضعیت دارای ماده 29 می باشد و پیوست الزامی است");
                    }
                }


            }

            long? isInsert = cpsivm.ProjectSectionStatement.Id;

            if (isInsert == -1)
            {
                cpsivm.ProjectSectionStatement.ProjectSectionStatementStateId = Convert.ToInt64(GeneralEnums.ProjectSectionStatementStates.Statement);

                _ProjectSectionStatementService.Insert(cpsivm.ProjectSectionStatement);
            }
            else
            {
                _ProjectSectionStatementService.Update(cpsivm.ProjectSectionStatement);

                foreach (var item in _ProjectSectionStatementFileService.Get(i => i.ProjectSectionStatementId == cpsivm.ProjectSectionStatement.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionStatementFileService.Delete(item);
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
                            ProjectSectionStatementFile ProjectSectionStatementFile = new ProjectSectionStatementFile();
                            ProjectSectionStatementFile.ProjectSectionStatementId = cpsivm.ProjectSectionStatement.Id;
                            ProjectSectionStatementFile.FileId = fileId;

                            _ProjectSectionStatementFileService.Insert(ProjectSectionStatementFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionStatement.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }
        public IEnumerable<ReportProjectStatementViewModel> GetAllReport(ReportParameterProjectStatementViewModel parameters)
        {
            string tempFilter = " ";
            string and = "";
            if ((parameters.FileCode != null) && (parameters.ProjectTitle != null) && (parameters.ProjectSectionTitle != null) && (parameters.StatementNumberId != null) && (parameters.ContractorId != null) && (parameters.StartDate != null) && (parameters.EndDate != null))
            {

                IEnumerable<ReportProjectStatementViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectStatementViewModel>(@"SELECT ProjectDefine.Projects.ProjectTitle, ProjectDevision.ProjectSections.Title AS projectSectionTitle, ProjectDefine.Projects.FileCode, 
                         ProjectDevision.ProjectSectionStatements.ContractPrice AS ExpensesOfContractor, ProjectDevision.ProjectSectionStatements.AdvisorPrice AS AdviserOfferPrice, 
                         ProjectDevision.ProjectSectionStatements.ConfirmPrice AS SupervisorOfferPrice, ProjectDevision.ProjectSectionStatements.ContractDate AS StatementPresentationDate, BaseInformation.StatementTypes.Title AS StatementType, 
                         BaseInformation.StatementNumbers.Title AS StatementNumber, ProjectDevision.ProjectSectionStatementConfirms.Price AS AcceptedPrice, ProjectDevision.ProjectSectionDrafts.DraftPrice AS AcceptedDraftPrice, 
                         ProjectDevision.ProjectSectionDrafts.DraftDate AS AcceptedDraftDate, ProjectDevision.ProjectSectionPaidPrices.DraftNumber AS DraftNumber, ProjectDevision.ProjectSectionPaidPrices.PaidPrice, 
                         ProjectDevision.ProjectSectionPaidPrices.PaidDate, BaseInformation.Contractors.CompanyName
FROM            ProjectDefine.Projects LEFT JOIN
                         ProjectDevision.ProjectSections ON ProjectDefine.Projects.Id = ProjectDevision.ProjectSections.ProjectId LEFT JOIN
                         ProjectDevision.ProjectSectionStatements ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionStatements.ProjectSectionId LEFT JOIN
                         BaseInformation.StatementTypes ON ProjectDevision.ProjectSectionStatements.StatementTypeId = BaseInformation.StatementTypes.Id LEFT JOIN
                         BaseInformation.StatementNumbers ON ProjectDevision.ProjectSectionStatements.StatementNumberId = BaseInformation.StatementNumbers.Id LEFT JOIN
                         ProjectDevision.ProjectSectionStatementConfirms ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionStatementConfirms.ProjectSectionId AND 
                         ProjectDevision.ProjectSectionStatements.Id = ProjectDevision.ProjectSectionStatementConfirms.ProjectSectionStatementId LEFT JOIN
                         ProjectDevision.ProjectSectionDrafts ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionDrafts.ProjectSectionId AND 
                         ProjectDevision.ProjectSectionStatementConfirms.Id = ProjectDevision.ProjectSectionDrafts.ProjectSectionStatementConfirmId AND 
                         ProjectDevision.ProjectSectionStatementConfirms.Id = ProjectDevision.ProjectSectionDrafts.ProjectSectionStatementConfirmId AND 
                         ProjectDevision.ProjectSectionStatementConfirms.Id = ProjectDevision.ProjectSectionDrafts.ProjectSectionStatementConfirmId LEFT JOIN
                         ProjectDevision.ProjectSectionPaidPrices ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionPaidPrices.ProjectSectionId AND 
                         ProjectDevision.ProjectSectionDrafts.Id = ProjectDevision.ProjectSectionPaidPrices.ProjectSectionDraftId AND 
                         ProjectDevision.ProjectSectionDrafts.Id = ProjectDevision.ProjectSectionPaidPrices.ProjectSectionDraftId AND 
                         ProjectDevision.ProjectSectionDrafts.Id = ProjectDevision.ProjectSectionPaidPrices.ProjectSectionDraftId LEFT JOIN
                         ProjectDevision.ProjectSectionWinners ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionWinners.ProjectSectionId LEFT JOIN
                         BaseInformation.Contractors ON ProjectDevision.ProjectSectionWinners.ContractorId = BaseInformation.Contractors.Id AND 
                         ProjectDevision.ProjectSectionWinners.ContractorId = BaseInformation.Contractors.Id AND 
                         ProjectDevision.ProjectSectionWinners.ContractorId = BaseInformation.Contractors.Id
                       ")
                    .Select(Result => new ReportProjectStatementViewModel()
                    {
                        FileCode = Result.FileCode,
                        ProjectTitle = Result.ProjectTitle,
                        ProjectSectionTitle = Result.ProjectSectionTitle,
                        CompanyName = Result.CompanyName,
                        StatementPresentationDate = Result.StatementPresentationDate,
                        StatementType = Result.StatementType,
                        StatementNumber = Result.StatementNumber,
                        ExpensesOfContractor = Result.ExpensesOfContractor,
                        AdviserOfferPrice = Result.AdviserOfferPrice,
                        SupervisorOfferPrice = Result.SupervisorOfferPrice,
                        AcceptedPrice = Result.AcceptedPrice,
                        DraftSendPrice = Result.DraftSendPrice,
                        DraftNumber = Result.DraftNumber,
                        AcceptedDraftPrice = Result.AcceptedDraftPrice,
                        AcceptedDraftDate = Result.AcceptedDraftDate,
                        PaidPrice = Result.PaidPrice,
                        PaidDate = Result.PaidDate,
                    });
                return result;
            }
            else
            {


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
                if (parameters.StatementNumberId != null)
                {
                    tempFilter += and;
                    tempFilter += "BaseInformation.StatementNumbers.Id = " + parameters.StatementNumberId;
                    and = " AND ";
                }
                if (parameters.ContractorId != null)
                {
                    tempFilter += and;
                    tempFilter += "BaseInformation.Contractors.Id = " + parameters.ContractorId;
                    and = " AND ";
                }

                if (parameters.StartDate != null)
                {
                    tempFilter += and;
                    tempFilter += "ProjectDevision.ProjectSectionStatements.ContractDate >= N'" + parameters.StartDate + "'";
                    and = " AND ";
                }
                if (parameters.StartDate != null)
                {
                    tempFilter += and;
                    tempFilter += "ProjectDevision.ProjectSectionStatements.ContractDate >= N'" + parameters.StartDate + "'";
                    and = " AND ";
                }
                //BaseInformation.StatementNumbers.Title

                if (parameters.EndDate != null)
                {
                    tempFilter += and;
                    tempFilter += "ProjectDevision.ProjectSectionStatements.ContractDate < N'" + parameters.EndDate + "'";
                    and = " AND ";
                }


                string filter = "";
                if (!String.IsNullOrEmpty(tempFilter.Trim()))
                    filter = " where " + tempFilter;

                IEnumerable<ReportProjectStatementViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectStatementViewModel>(@"SELECT ProjectDefine.Projects.ProjectTitle, ProjectDevision.ProjectSections.Title AS projectSectionTitle, ProjectDefine.Projects.FileCode, 
                         ProjectDevision.ProjectSectionStatements.ContractPrice AS ExpensesOfContractor, ProjectDevision.ProjectSectionStatements.AdvisorPrice AS AdviserOfferPrice, 
                         ProjectDevision.ProjectSectionStatements.ConfirmPrice AS SupervisorOfferPrice, ProjectDevision.ProjectSectionStatements.ContractDate AS StatementPresentationDate, BaseInformation.StatementTypes.Title AS StatementType, 
                         BaseInformation.StatementNumbers.Title AS StatementNumber, ProjectDevision.ProjectSectionStatementConfirms.Price AS AcceptedPrice, ProjectDevision.ProjectSectionDrafts.DraftPrice AS AcceptedDraftPrice, 
                         ProjectDevision.ProjectSectionDrafts.DraftDate AS AcceptedDraftDate, ProjectDevision.ProjectSectionPaidPrices.DraftNumber AS DraftNumber, ProjectDevision.ProjectSectionPaidPrices.PaidPrice, 
                         ProjectDevision.ProjectSectionPaidPrices.PaidDate, BaseInformation.Contractors.CompanyName
FROM            ProjectDefine.Projects LEFT JOIN
                         ProjectDevision.ProjectSections ON ProjectDefine.Projects.Id = ProjectDevision.ProjectSections.ProjectId LEFT JOIN
                         ProjectDevision.ProjectSectionStatements ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionStatements.ProjectSectionId LEFT JOIN
                         BaseInformation.StatementTypes ON ProjectDevision.ProjectSectionStatements.StatementTypeId = BaseInformation.StatementTypes.Id LEFT JOIN
                         BaseInformation.StatementNumbers ON ProjectDevision.ProjectSectionStatements.StatementNumberId = BaseInformation.StatementNumbers.Id LEFT JOIN
                         ProjectDevision.ProjectSectionStatementConfirms ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionStatementConfirms.ProjectSectionId AND 
                         ProjectDevision.ProjectSectionStatements.Id = ProjectDevision.ProjectSectionStatementConfirms.ProjectSectionStatementId LEFT JOIN
                         ProjectDevision.ProjectSectionDrafts ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionDrafts.ProjectSectionId AND 
                         ProjectDevision.ProjectSectionStatementConfirms.Id = ProjectDevision.ProjectSectionDrafts.ProjectSectionStatementConfirmId AND 
                         ProjectDevision.ProjectSectionStatementConfirms.Id = ProjectDevision.ProjectSectionDrafts.ProjectSectionStatementConfirmId AND 
                         ProjectDevision.ProjectSectionStatementConfirms.Id = ProjectDevision.ProjectSectionDrafts.ProjectSectionStatementConfirmId LEFT JOIN
                         ProjectDevision.ProjectSectionPaidPrices ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionPaidPrices.ProjectSectionId AND 
                         ProjectDevision.ProjectSectionDrafts.Id = ProjectDevision.ProjectSectionPaidPrices.ProjectSectionDraftId AND 
                         ProjectDevision.ProjectSectionDrafts.Id = ProjectDevision.ProjectSectionPaidPrices.ProjectSectionDraftId AND 
                         ProjectDevision.ProjectSectionDrafts.Id = ProjectDevision.ProjectSectionPaidPrices.ProjectSectionDraftId LEFT JOIN
                         ProjectDevision.ProjectSectionWinners ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionWinners.ProjectSectionId LEFT JOIN
                         BaseInformation.Contractors ON ProjectDevision.ProjectSectionWinners.ContractorId = BaseInformation.Contractors.Id AND 
                         ProjectDevision.ProjectSectionWinners.ContractorId = BaseInformation.Contractors.Id AND 
                         ProjectDevision.ProjectSectionWinners.ContractorId = BaseInformation.Contractors.Id
                       " + filter)
                    .Select(Result => new ReportProjectStatementViewModel()
                    {
                        FileCode = Result.FileCode,
                        ProjectTitle = Result.ProjectTitle,
                        ProjectSectionTitle = Result.ProjectSectionTitle,
                        CompanyName = Result.CompanyName,
                        StatementPresentationDate = Result.StatementPresentationDate,
                        StatementType = Result.StatementType,
                        StatementNumber = Result.StatementNumber,
                        ExpensesOfContractor = Result.ExpensesOfContractor,
                        AdviserOfferPrice = Result.AdviserOfferPrice,
                        SupervisorOfferPrice = Result.SupervisorOfferPrice,
                        AcceptedPrice = Result.AcceptedPrice,
                        DraftSendPrice = Result.DraftSendPrice,
                        DraftNumber = Result.DraftNumber,
                        AcceptedDraftPrice = Result.AcceptedDraftPrice,
                        AcceptedDraftDate = Result.AcceptedDraftDate,
                        PaidPrice = Result.PaidPrice,
                        PaidDate = Result.PaidDate,
                    });
                return result;
            }


        }
        //public static int StatusTxt(Int64 b)
        //{
        //    int sum = 0;
        //    DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
        //     sum = db.ProjectSectionStatementConfirms.Where(a => a.ProjectSectionStatementId == b).ToList().Count;
        //     if (sum > 0) return 1;
        //     else return 0;
        //}

    }
   
}
