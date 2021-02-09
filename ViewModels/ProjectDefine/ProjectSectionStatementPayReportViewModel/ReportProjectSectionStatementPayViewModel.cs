using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProjectSectionStatementPayReportViewModel
{
    public class ReportProjectSectionStatementPayViewModel
    {

        public string StatementType { get; set; }
        public string StatementDate { get; set; }
        public Int64? DraftPrice { get; set; }
        public float Deposit { get; set; }
        public Int64? PrePayment { get; set; }
        public Int64? Forfeit { get; set; }
        public float Tax { get; set; }
        public float MasterInsurance { get; set; }
        public Int64? PayablePrice { get; set; }
        public Int64? InsuranceType { get; set; }
        public string InsuranceTypeText
        {
            get;
            set;
        }
        public Int64? PaidPrice { get; set; }
        public string StatementNumber { get; set; }
        public string ContractDate { get; set; }
        public string CompanyName { get; set; }
        public string ProjectSectionTitle { get; set; }
        public string ProjectTitle { get; set; }
        public string FileCode { get; set; }

        //public void Insurance(int insuranceType)
        //{
        //    string insuranceText
        //    switch (InsuranceType)
        //    {
        //        case 1:
        //            insuranceText = "بیمه اعتبارات تملک و داراییها";
        //            break;
        //        case 2:
        //            insuranceText = "بیمه دستمزدی با مصالح";
        //            break;
        //        case 3:
        //            insuranceText = "بیمه دستمزدی بدون مصالح";
        //            break;
        //        default:
        //            insuranceText = "تعریف نشده";
        //            break;
        //    }
        //    InsuranceTypeText = insuranceText;
        //}
    }
}
