using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProjectAllocateViewModel
{
    public class ReportProjectAllocateViewModel
    {
        public string FileCode { get; set; }
        public string ProjectTitle { get; set; }
        public Int64? ProjectStateId { get; set; }
        public string Title { get; set; }
        public Int64? AllocateAmount { get; set; }
        public string AllocateDate { get; set; }
        public Int64? StartFinantialYearId { get; set; }
        public string StartFinantialYearTitle { get; set; }
    }
}
