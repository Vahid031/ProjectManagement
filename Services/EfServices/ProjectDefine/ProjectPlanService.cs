using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using Services.Interfaces.BaseInformation;
using ViewModels.ProjectDefine.ProjectPlanViewModel;

namespace Services.EfServices.BaseInformation
{
    public class ProjectPlanService : GenericService<ProjectPlan>, IProjectPlanService
    {
        public ProjectPlanService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ReportProjectPlanViewModel> GetAllReport(ReportParameterProjectPlanViewModel parameters)
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

            if (parameters.ProgramId != null)
            {
                tempFilter += and;
                tempFilter += "Result.ProgramId = " + parameters.ProgramId.Value.ToString();
                and = " AND ";
            }

            if (parameters.CreditProvidePlaceId != null)
            {
                tempFilter += and;
                tempFilter += "Result.CreditProvidePlaceId = " + parameters.CreditProvidePlaceId.Value.ToString();
                and = " AND ";
            }


            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string allField = @"SELECT * FROM (";
            string query = "";

            query += @"Select PDP.FileCode, PDP.ProjectTitle, PDPP.ProgramId, BIP.Title ProgramTitle, PDPP.ProgramPlanId, 
                       	   PDPRP.PlanTitle ProgramPlanTitle, PDPP.CreditProvidePlaceId, BICPP.Title CreditProvidePlaceTitle,
                       	   PDPP.FromFinantialYearId, BIFFY.Title FromFinantialYearTitle, PDPP.ToFinantialYearId, BITFY.Title ToFinantialYearTitle
                       From ProjectDefine.ProjectPlans PDPP
                       	   inner join ProjectDefine.Projects PDP on PDPP.ProjectId = PDP.Id 
                       	   inner join BaseInformation.Programs BIP on BIP.Id = PDPP.ProgramId
                       	   inner join ProjectDefine.ProgramPlans PDPRP on PDPRP.Id = PDPP.ProgramPlanId
                       	   left join BaseInformation.CreditProvidePlaces BICPP on BICPP.Id = PDPP.CreditProvidePlaceId
                       	   left join BaseInformation.FinantialYears BIFFY on BIFFY.Id = PDPP.FromFinantialYearId
                       	   left join BaseInformation.FinantialYears BITFY on BITFY.Id = PDPP.ToFinantialYearId
                       ) Result"
            + filter;

            IEnumerable<ReportProjectPlanViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectPlanViewModel>(allField + query)
                .Select(Result => new ReportProjectPlanViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    ProgramId = Result.ProgramId,
                    ProgramTitle = Result.ProgramTitle,
                    ProgramPlanId = Result.ProgramPlanId,
                    ProgramPlanTitle = Result.ProgramPlanTitle,
                    CreditProvidePlaceId = Result.CreditProvidePlaceId,
                    CreditProvidePlaceTitle = Result.CreditProvidePlaceTitle,
                    FromFinantialYearId = Result.FromFinantialYearId,
                    FromFinantialYearTitle = Result.FromFinantialYearTitle,
                    ToFinantialYearId = Result.ToFinantialYearId,
                    ToFinantialYearTitle = Result.ToFinantialYearTitle
                });

            return result;
        }
    }
}
