using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProjectSectionContractAmendmentReport
{
    public class ReportProjectSectionContractAmendmentViewModel
    {
        public string FileCode { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectSectionTitle { get; set; }
        public string CompanyName { get; set; }
        public Int64? NewPrice { get; set; }
        public string Date { get; set; }
        public string Number { get; set; }
        public string AmendmentType { get; set; }
        public int? IncreasePrice { get; set; }
        public int? DecreasePrice { get; set; }

    }
}
