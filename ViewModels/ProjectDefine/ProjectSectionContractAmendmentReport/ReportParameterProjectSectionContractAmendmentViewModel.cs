using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProjectSectionContractAmendmentReport
{
    public class ReportParameterProjectSectionContractAmendmentViewModel
    {
        [DisplayName("کد پروژه")]
        public string FileCode { get; set; }
        [DisplayName("عنوان پروژه")]
        public string ProjectTitle { get; set; }
        [DisplayName("عنوان فاز پروژه")]
        public string ProjectSectionTitle { get; set; }
    }
}
