using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProjectStatementViewModel
{
    public class ReportParameterProjectStatementViewModel
    {
        [DisplayName("کد پروژه")]
        public string FileCode { get; set; }
        public Int64? StatementNumberId { get; set; }
        [DisplayName("شماره صورت وضعیت")]
        public List<StatementNumber> StatementNumbers { get; set; }
        [DisplayName("عنوان پروژه")]
        public string ProjectTitle { get; set; }
        [DisplayName("عنوان فاز پروژه")]
        public string ProjectSectionTitle { get; set; }
        [DisplayName("پیمانکار")]
        public Int64? ContractorId { get; set; }
        [DisplayName("ازتاریخ")]
        public string StartDate { get; set; }
        [DisplayName("تاتاریخ")]
        public string EndDate { get; set; }
        [DisplayName("پیمانکار")]
        public List<Contractor> Contractors { get; set; }
    }
}
