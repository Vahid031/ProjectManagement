using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProjectStatementViewModel
{
    public class ReportProjectStatementViewModel
    {
        public string FileCode { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectSectionTitle { get; set; }
        public string CompanyName { get; set; }
        public string StatementPresentationDate { get; set; }
        public string StatementType { get; set; }
        public string StatementNumber { get; set; }
        public Int64? ExpensesOfContractor { get; set; }
        public Int64? AdviserOfferPrice { get; set; }
        public Int64? SupervisorOfferPrice { get; set; }
        public Int64? AcceptedPrice { get; set; }
        public string DraftSendPrice { get; set; }
        public string DraftNumber { get; set; }
        public Int64? AcceptedDraftPrice { get; set; }
        public DateTime? AcceptedDraftDate { get; set; }
        public string PaidPrice { get; set; }
        public string PaidDate { get; set; }
    }
}
