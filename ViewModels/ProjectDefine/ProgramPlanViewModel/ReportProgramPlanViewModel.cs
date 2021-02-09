using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProgramPlanViewModel
{
    public class ReportProgramPlanViewModel
    {
        public Int64 ProgramId { get; set; }
        public string ProgramTitle { get; set; }
        public string PlanTitle { get; set; }
        public string PlanCode { get; set; }
        public Int64? PlanTypeId { get; set; }
        public string PlanTypeTitle { get; set; }
        public int? TotalProjects { get; set; }
        public Int64? FirstProjectFinantialYearId { get; set; }
        public string FirstProjectFinantialYearTitle { get; set; }
        public Int64? StartFinantialYearId { get; set; }
        public string StartFinantialYearTitle { get; set; }
        public Int64? ForecastLastProjectFinantialYearId { get; set; }
        public string ForecastLastProjectFinantialYearTitle { get; set; }
        public string ExecutionPlaceType { get; set; }
        public string CorrigendumNumber { get; set; }
        public string AgreementDate { get; set; }
        public int? ExchangeProjectCounts { get; set; }

    }
}
