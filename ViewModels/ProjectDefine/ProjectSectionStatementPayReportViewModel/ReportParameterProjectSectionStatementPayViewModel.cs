using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProjectSectionStatementPayReportViewModel
{
    public class ReportParameterProjectSectionStatementPayViewModel
    {
        [DisplayName("کد پروژه")]
        public string FileCode { get; set; }
        [DisplayName("عنوان پروژه")]
        public string ProjectTitle { get; set; }
        [DisplayName("عنوان فاز پروژه")]
        public string ProjectSectionTitle { get; set; }
        [DisplayName("شماره صورت وضعیت")]
        public Int64? StatementNumberId { get; set; }
        [DisplayName("شماره صورت وضعیت")]
        public List<StatementNumber> StatementNumbers { get; set; }
    }
}
