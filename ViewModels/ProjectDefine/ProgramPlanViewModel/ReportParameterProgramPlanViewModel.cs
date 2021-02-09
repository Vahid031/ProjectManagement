using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProgramPlanViewModel
{
    public class ReportParameterProgramPlanViewModel
    {
        [DisplayName("برنامه")]
        public Int64? ProgramId { get; set; }

        [DisplayName("نوع برنامه")]
        public Int64? PlanTypeId { get; set; }

        public List<Program> Programs { get; set; }
        public List<PlanType> PlanTypes { get; set; }
    }
}
