using DatabaseContext.Context;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProjectDefine.ProjectSectionContractAndCostsReportViewModel;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionContractAndCostsService : GenericService<ProjectSectionContract>, IProjectSectionContractAndCostsService
    {
        public ProjectSectionContractAndCostsService(IUnitOfWork uow)
            : base(uow)
        {

        }
        public IEnumerable<ReportProjectSectionContractAndCostsViewModel> GetAllReport(ReportParameterProjectSectionContractAndCostsViewModel parameters)
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

            if (parameters.CityId != null)
            {
                tempFilter += and;
                tempFilter += "BaseInformation.Cities.Id =" + parameters.CityId;
                and = " AND ";
            }
            if (parameters.ContractorTypeId != null)
            {
                tempFilter += and;
                tempFilter += "ProjectDevision.ProjectSectionContracts.ContractTypeId =" + parameters.ContractorTypeId;
                and = " AND ";
            }
            if (parameters.CreditProviderPlaceId != null)
            {
                tempFilter += and;
                tempFilter += "BaseInformation.CreditProvidePlaces.Id =" + parameters.CreditProviderPlaceId;
                and = " AND ";
            }
            if (parameters.FromDate != null)
            {
                tempFilter += and;
                tempFilter += "ProjectDevision.ProjectSectionContracts.StartDate >=N'" + parameters.FromDate + "'";
                and = " AND ";
            }
            if (parameters.TillDate != null)
            {
                tempFilter += and;
                tempFilter += "ProjectDevision.ProjectSectionContracts.EndDate <=N'" + parameters.TillDate + "'";
                and = " AND ";
            }
            if (parameters.ContractorId != null)
            {
                tempFilter += and;
                tempFilter += "BaseInformation.Contractors.Id = " + parameters.ContractorId + "AND ProjectDevision.ProjectSectionStatementConfirms.FinalOpinionId = 1";
                and = " AND ";
            }


            
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where "  + tempFilter;


            IEnumerable<ReportProjectSectionContractAndCostsViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionContractAndCostsViewModel>(@"SELECT        ISNULL(ProjectDevision.ProjectSectionContracts.ContractPrice, 0) - ISNULL(ProjectDevision.ProjectSectionPaidPrices.PaidPrice, 0) AS RemainingAmount, 
                         ProjectDefine.Projects.FileCode, ProjectDefine.Projects.ProjectTitle, ProjectDevision.ProjectSections.Title AS ProjectSectionTitle, 
                         BaseInformation.Contractors.CompanyName, BaseInformation.AssignmentTypes.Title AS AssignmentType, ProjectDevision.ProjectSectionContracts.ContractNumber, 
                         ProjectDevision.ProjectSectionContracts.EndDate, ProjectDevision.ProjectSectionContracts.StartDate, 
                         ProjectDevision.ProjectSectionContracts.GuaranteePeriod AS EndGuaranteeDuration, ProjectDevision.ProjectSectionContracts.ContractPrice, 
                         ProjectDevision.ProjectSectionComplementarityPrices.NewPrice AS ContractMaddeTwentyNinePrice, 
                         ProjectDevision.ProjectSectionComplementarityDates.NewDate AS MaddehThirtyNewDate, 
                         ProjectDevision.ProjectSectionMadeh46s.ForfeitPrice AS MaddeFourtySixNewPrice, ProjectDevision.ProjectSectionContracts.ContractDuration, 
                         BaseInformation.CreditProvidePlaces.Title, SUM(ProjectDevision.ProjectSectionStatementConfirms.Price) AS SumStatementConfirmedPrice, 
                         SUM(ProjectDevision.ProjectSectionDrafts.DraftPrice) AS SumDrafSendPrice, ProjectDevision.ProjectSectionPaidPrices.PaidPrice AS SumPaidPrice, 
                         ProjectDevision.ProjectSectionPaidPrices.PayablePrice
FROM            BaseInformation.CreditProvidePlaces LEFT JOIN
                         BaseInformation.Cities LEFT JOIN
                         ProjectDefine.Projects ON BaseInformation.Cities.Id = ProjectDefine.Projects.CityId LEFT JOIN
                         ProjectDefine.ProjectPlans ON ProjectDefine.Projects.Id = ProjectDefine.ProjectPlans.ProjectId ON 
                         BaseInformation.CreditProvidePlaces.Id = ProjectDefine.ProjectPlans.CreditProvidePlaceId LEFT JOIN
                         ProjectDevision.ProjectSectionPaidPrices LEFT JOIN
                         ProjectDevision.ProjectSections ON ProjectDevision.ProjectSectionPaidPrices.ProjectSectionId = ProjectDevision.ProjectSections.Id LEFT JOIN
                         ProjectDevision.ProjectSectionDrafts ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionDrafts.ProjectSectionId LEFT JOIN
                         ProjectDevision.ProjectSectionStatementConfirms ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionStatementConfirms.ProjectSectionId ON 
                         ProjectDefine.Projects.Id = ProjectDevision.ProjectSections.ProjectId LEFT JOIN
                         ProjectDevision.ProjectSectionMadeh46s ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionMadeh46s.ProjectSectionId LEFT JOIN
                         ProjectDevision.ProjectSectionComplementarityDates ON 
                         ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionComplementarityDates.ProjectSectionId LEFT JOIN
                         ProjectDevision.ProjectSectionContracts ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionContracts.ProjectSectionId LEFT JOIN
                         ProjectDevision.ProjectSectionWinners ON ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionWinners.ProjectSectionId LEFT JOIN
                         BaseInformation.Contractors ON ProjectDevision.ProjectSectionWinners.ContractorId = BaseInformation.Contractors.Id LEFT JOIN
                         BaseInformation.AssignmentTypes ON ProjectDevision.ProjectSections.AssignmentTypeId = BaseInformation.AssignmentTypes.Id LEFT JOIN
                         ProjectDevision.ProjectSectionComplementarityPrices ON 
                         ProjectDevision.ProjectSections.Id = ProjectDevision.ProjectSectionComplementarityPrices.ProjectSectionId
                       " + filter + @"GROUP BY ProjectDefine.Projects.FileCode, ProjectDefine.Projects.ProjectTitle, ProjectDevision.ProjectSections.Title, BaseInformation.Contractors.CompanyName, 
                         BaseInformation.AssignmentTypes.Title, ProjectDevision.ProjectSectionContracts.ContractNumber, ProjectDevision.ProjectSectionContracts.EndDate, 
                         ProjectDevision.ProjectSectionContracts.StartDate, ProjectDevision.ProjectSectionContracts.GuaranteePeriod, ProjectDevision.ProjectSectionContracts.ContractPrice, 
                         ProjectDevision.ProjectSectionComplementarityPrices.NewPrice, ProjectDevision.ProjectSectionComplementarityDates.NewDate, 
                         ProjectDevision.ProjectSectionMadeh46s.ForfeitPrice, ProjectDevision.ProjectSectionContracts.ContractDuration, BaseInformation.CreditProvidePlaces.Title, 
                         ProjectDevision.ProjectSectionPaidPrices.PaidPrice, ProjectDevision.ProjectSectionPaidPrices.PaidPrice, ProjectDevision.ProjectSectionPaidPrices.PayablePrice 
            ")
                .Select(Result => new ReportProjectSectionContractAndCostsViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    CompanyName = Result.CompanyName,
                    ContractNumber = Result.ContractNumber,
                    AssignmentType = Result.AssignmentType,
                    StartDate = Result.StartDate,
                    ContractDuration = Result.ContractDuration,
                    EndDate = Result.EndDate,
                    EndGuaranteeDuration = Result.EndGuaranteeDuration,
                    MaddehThirtyNewDate = Result.MaddehThirtyNewDate,
                    ContractPrice = Result.ContractPrice,
                    ContractMaddeTwentyNinePrice = Result.ContractMaddeTwentyNinePrice,
                    MaddeFourtySixNewPrice = Result.MaddeFourtySixNewPrice,
                    SumStatementConfirmedPrice = Result.SumStatementConfirmedPrice,
                    SumDrafSendPrice = Result.SumDrafSendPrice,
                    SumPaidPrice = Result.SumPaidPrice,
                    RemainingAmount = Result.RemainingAmount
                });
            var test = result.ToList();
            return result;
        }
    }
}
