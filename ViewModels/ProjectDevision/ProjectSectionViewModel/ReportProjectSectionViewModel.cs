using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDevision.ProjectSectionViewModel
{
    public class ReportProjectSectionViewModel
    {
        public string FileCode { get; set; }
        public string ProjectTitle { get; set; }
        public string Title { get; set; }
        public Int64? AssignmentTypeId { get; set; }
        public string AssignmentTypeTitle { get; set; }
        public string ForecastStartDate { get; set; }
        public string ForecastEndDate { get; set; }
    }
}
