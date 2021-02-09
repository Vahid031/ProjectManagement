using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProjectSectionContractAndCostsReportViewModel
{
    public class ReportProjectSectionContractAndCostsViewModel
    {
        public string FileCode { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectSectionTitle { get; set; }
        public string CompanyName { get; set; }

        public string ContractNumber { get; set; }
        public string AssignmentType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ContractDuration { get; set; }
        public Int64? EndGuaranteeDuration { get; set; }
        public string MaddehThirtyNewDate { get; set; }
        public Int64? ContractPrice { get; set; }
        public Int64? ContractMaddeTwentyNinePrice { get; set; }
        public Int64? MaddeFourtySixNewPrice { get; set; }
        public Int64? SumStatementConfirmedPrice { get; set; }
        public Int64? SumDrafSendPrice { get; set; }
        public Int64? SumPaidPrice { get; set; }
        public Int64? RemainingAmount { get; set; }
    }
}
