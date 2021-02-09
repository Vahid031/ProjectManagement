using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDevision.ProjectSectionDepositeLiberalizationViewModel
{
    public class ReportProjectSectionDepositeLiberalizationsViewModel
    {
        public string FileCode { get; set; }
        public string ProjectTitle { get; set; }
        public string Title { get; set; }
        public string PaidDate { get; set; }
        public Int64? TemproryPrice { get; set; }
        public Int64? FixedPrice { get; set; }
    }
}
