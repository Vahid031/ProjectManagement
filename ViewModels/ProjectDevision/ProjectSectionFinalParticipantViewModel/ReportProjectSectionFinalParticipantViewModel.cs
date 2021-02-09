using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDevision.ProjectSectionFinalParticipantViewModel
{
    public class ReportProjectSectionFinalParticipantViewModel
    {
        public string FileCode { get; set; }
        public string ProjectTitle { get; set; }
        public string Title { get; set; }
        public string WarrantyDate { get; set; }
        public string CompanyName { get; set; }
        public Int64? SuggestPrice { get; set; }
    }
}
