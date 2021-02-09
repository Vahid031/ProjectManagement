using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProjectPlanViewModel
{
    public class ReportProjectPlanViewModel
    {
        public string FileCode { get; set; }
        public string ProjectTitle { get; set; }
        public Int64? ProgramId { get; set; }
        public string ProgramTitle { get; set; }
        public Int64? ProgramPlanId { get; set; }
        public string ProgramPlanTitle { get; set; }
        public Int64? CreditProvidePlaceId { get; set; }
        public string CreditProvidePlaceTitle { get; set; }
        public Int64? FromFinantialYearId { get; set; }
        public string FromFinantialYearTitle { get; set; }
        public Int64? ToFinantialYearId { get; set; }
        public string ToFinantialYearTitle { get; set; }
    }
}
