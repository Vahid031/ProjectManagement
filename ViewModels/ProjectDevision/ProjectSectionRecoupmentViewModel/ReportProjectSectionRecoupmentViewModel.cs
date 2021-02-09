using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDevision.ProjectSectionRecoupmentViewModel
{
    public class ReportProjectSectionRecoupmentViewModel
    {
        public string FileCode { get; set; }
        public string ProjectTitle { get; set; }
        public string Title { get; set; }
        public Int64? RecoupmentTypeId { get; set; }
        public string RecoupmentTypeTitle { get; set; }
        public string Number { get; set; }
        public DateTime? Date { get; set; }
        public string BranchExporter { get; set; }
    }
}
