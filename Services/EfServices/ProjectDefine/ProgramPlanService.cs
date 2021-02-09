using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using DomainModels.Entities.BaseInformation;
using ViewModels.ProjectDefine.ProgramPlanViewModel;
using DomainModels.Entities.ProjectDefine;
using Services.Interfaces.BaseInformation;

namespace Services.EfServices.BaseInformation
{
    public class ProgramPlanService : GenericService<ProgramPlan>, IProgramPlanService
    {
        public ProgramPlanService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProgramPlanViewModel> GetAll(ListProgramPlanViewModel lppvm, ref Paging pg)
        {
            string tempFilter = DynamicFiltering.GetSqlFilterDynamic(lppvm, "Result");
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string[] orderby = pg._orderColumn.Split(' ');
            orderby[1] = orderby[1] == "descending" ? "DESC" : "ASC";

            string allField = @"SELECT * FROM (";
            string query = "";
            query += @"Select PDPP.Id, PDPP.ProgramId, BIP.Title ProgramTitle, PDPP.PlanTitle, PDPP.PlanCode, PDPP.PlanTypeId, BIPT.Title PlanTypeTitle, 
                        	   ISNULL(PDPP.ExecutionSetId, -1) ExecutionSetId, ISNULL(BIES.Title, '') ExecutionSetTitle, ISNULL(PDPP.BeneficiarySetId, -1) BeneficiarySetId, 
                        	   ISNULL(BIBS.Title, '') BeneficiarySetTitle, ISNULL(CAST(PDPP.TotalProjects as nvarchar(20)), '') TotalProjects,
                        	   ISNULL(PDPP.FirstProjectFinantialYearId, -1) FirstProjectFinantialYearId, ISNULL(BIFYFP.Title, '') FirstProjectFinantialYearTitle, 
                        	   ISNULL(PDPP.ForecastLastProjectFinantialYearId, -1) ForecastLastProjectFinantialYearId, ISNULL(BIFYFLP.Title, '') ForecastLastProjectFinantialYearTitle
                        From ProjectDefine.ProgramPlans PDPP
                        		 inner join BaseInformation.Programs BIP on BIP.Id = PDPP.ProgramId
                        		 inner join BaseInformation.PlanTypes BIPT on BIPT.Id = PDPP.PlanTypeId
                        		 left join  BaseInformation.[Sets] BIES on BIES.Id = PDPP.ExecutionSetId
                        		 left join  BaseInformation.[Sets] BIBS on BIBS.Id = PDPP.BeneficiarySetId
                        		 left join  BaseInformation.FinantialYears BIFYFP on BIFYFP.Id = PDPP.FirstProjectFinantialYearId
                        		 left join  BaseInformation.FinantialYears BIFYFLP on BIFYFLP.Id = PDPP.ForecastLastProjectFinantialYearId
                      ) Result"
            + filter;


            string orderQuery = @" ORDER BY " + orderby[0] + " " + orderby[1] + @" OFFSET " + (pg._pageNumber - 1) * pg._pageSize + @" ROWS FETCH NEXT " + pg._pageSize + " ROWS ONLY;";

            IEnumerable<ListProgramPlanViewModel> result = _uow.GetInstance().Database.SqlQuery<ListProgramPlanViewModel>(allField + query + orderQuery)
                .Select(Result => new ListProgramPlanViewModel()
            {
                Id = Result.Id,
                ProgramId = Result.ProgramId,
                ProgramTitle = Result.ProgramTitle,
                PlanTitle = Result.PlanTitle,
                PlanCode = Result.PlanCode,
                PlanTypeId = Result.PlanTypeId,
                PlanTypeTitle = Result.PlanTypeTitle,
                ExecutionSetId = Result.ExecutionSetId,
                ExecutionSetTitle = Result.ExecutionSetTitle,
                BeneficiarySetId = Result.BeneficiarySetId,
                BeneficiarySetTitle = Result.BeneficiarySetTitle,
                TotalProjects = Result.TotalProjects,
                FirstProjectFinantialYearId = Result.FirstProjectFinantialYearId,
                FirstProjectFinantialYearTitle = Result.FirstProjectFinantialYearTitle,
                ForecastLastProjectFinantialYearId = Result.ForecastLastProjectFinantialYearId,
                ForecastLastProjectFinantialYearTitle = Result.ForecastLastProjectFinantialYearTitle
            });

            string queryCount = @"SELECT CAST(COUNT(Result.Id) AS BIGINT) AS [Count] FROM (";

            var count = _uow.GetInstance().Database.SqlQuery<long>(queryCount + query).FirstOrDefault();
            pg._rowCount = Convert.ToInt64(count);

            return result;
        }

        public IEnumerable<ReportProgramPlanViewModel> GetAllReport(ReportParameterProgramPlanViewModel parameters)
        {
            string tempFilter = " ";
            string and = "";

            if (parameters.ProgramId != null)
            {
                tempFilter += and;
                tempFilter += "Result.ProgramId = " + parameters.ProgramId;
                and = " AND ";
            }

            if (parameters.PlanTypeId != null)
            {
                tempFilter += and;
                tempFilter += "Result.PlanTypeId = " + parameters.PlanTypeId;
                and = " AND ";
            }

            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string allField = @"SELECT * FROM (";
            string query = "";

            query += @"Select PDPP.ProgramId, BIP.Title ProgramTitle, PDPP.PlanTitle, PDPP.PlanCode, PDPP.PlanTypeId, BIPT.Title PlanTypeTitle,
                       	   PDPP.TotalProjects, PDPP.FirstProjectFinantialYearId, BIFPFY.Title FirstProjectFinantialYearTitle,
                       	   PDPP.ForecastLastProjectFinantialYearId, BIFLPFY.Title ForecastLastProjectFinantialYearTitle, 
                       	   PDPP.ExecutionPlaceType, PDPP.CorrigendumNumber, PDPP.AgreementDate, PDPP.ExchangeProjectCounts
                       From ProjectDefine.ProgramPlans PDPP
                       		    inner join BaseInformation.Programs BIP on PDPP.ProgramId = BIP.Id
                       			inner join BaseInformation.PlanTypes BIPT on PDPP.PlanTypeId = BIPT.Id
                       			left join BaseInformation.FinantialYears BIFPFY on PDPP.FirstProjectFinantialYearId = BIFPFY.Id
                       			left join BaseInformation.FinantialYears BIFLPFY on PDPP.ForecastLastProjectFinantialYearId = BIFLPFY.Id
                       ) Result"
            + filter;

            IEnumerable<ReportProgramPlanViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProgramPlanViewModel>(allField + query)
                .Select(Result => new ReportProgramPlanViewModel()
                {
                    ProgramId = Result.ProgramId,
                    ProgramTitle = Result.ProgramTitle,
                    PlanTitle = Result.PlanTitle,
                    PlanCode = Result.PlanCode,
                    PlanTypeId = Result.PlanTypeId,
                    PlanTypeTitle = Result.PlanTypeTitle,
                    TotalProjects = Result.TotalProjects,
                    FirstProjectFinantialYearId = Result.FirstProjectFinantialYearId,
                    FirstProjectFinantialYearTitle = Result.FirstProjectFinantialYearTitle,
                    StartFinantialYearId = Result.StartFinantialYearId,
                    StartFinantialYearTitle = Result.StartFinantialYearTitle,
                    ForecastLastProjectFinantialYearId = Result.ForecastLastProjectFinantialYearId,
                    ForecastLastProjectFinantialYearTitle = Result.ForecastLastProjectFinantialYearTitle,
                    ExecutionPlaceType = Result.ExecutionPlaceType,
                    CorrigendumNumber = Result.CorrigendumNumber,
                    AgreementDate = Result.AgreementDate,
                    ExchangeProjectCounts = Result.ExchangeProjectCounts
                });
            var query1 = result.ToList();

            return result;
        }

        
    }
}
