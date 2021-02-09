using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProjectPlanViewModel
{
    public class ReportParameterProjectPlanViewModel
    {
        [DisplayName("کد پروژه")]
        public string FileCode { get; set; }

        [DisplayName("عنوان پروژه")]
        public string ProjectTitle { get; set; }

        [DisplayName("برنامه")]
        public Int64? ProgramId { get; set; }

        [DisplayName("محل تامین اعتبار")]
        public Int64? CreditProvidePlaceId { get; set; }

        public List<Program> Programs { get; set; }
        public List<CreditProvidePlace> CreditProvidePlaces { get; set; }
    }
}
