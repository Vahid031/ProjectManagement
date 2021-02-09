using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProjectSectionProjectProgressReportViewModel
{
    public class ReportProjectSectionProjectProgressViewModel
    {
        public string FileCode { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectSectionTitle { get; set; }
        public string City { get; set; }
        public int? DevelopmentPercent { get; set; }
        public string OperationTitle { get; set; }
        public string VisitDate { get; set; }
        public string MyProperty { get; set; }
        public string SupervisorName { get; set; }
    }
}
