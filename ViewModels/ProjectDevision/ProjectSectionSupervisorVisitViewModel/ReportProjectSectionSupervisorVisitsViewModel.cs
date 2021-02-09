using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDevision.ProjectSectionSupervisorVisitViewModel
{
    public class ReportProjectSectionSupervisorVisitsViewModel
    {
        public string FileCode { get; set; }
        public string ProjectTitle { get; set; }
        public string Title { get; set; }
        public Int64? CityId { get; set; }
        public string CityTitle { get; set; }
        public string MemberName { get; set; }
        public string VisitDate { get; set; }
        public Int64? ProjectSectionOperationTitleId { get; set; }
        public string ProjectSectionOperationTitle { get; set; }
        public string Agendum { get; set; }
        public int? DevelopmentPercent { get; set; }
        public string Description { get; set; }
    }
}
