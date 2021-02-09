using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDevision.ProjectSectionSupervisorVisitViewModel
{
    public class ReportParameterProjectSectionSupervisorVisitsViewModel
    {
        [DisplayName("کد پروژه")]
        public string FileCode { get; set; }

        [DisplayName("عنوان پروژه")]
        public string ProjectTitle { get; set; }

        [DisplayName("عنوان فاز پروژه")]
        public string Title { get; set; }

        [DisplayName("شهرستان")]
        public Int64? CityId { get; set; }

        [DisplayName("نام ناظر")]
        public Int64? MemberId { get; set; }

        [DisplayName("از تاریخ")]
        public string FromDate { get; set; }

        [DisplayName("تا تاریخ")]
        public string ToDate { get; set; }

        public List<City> Cities { get; set; }
        public List<Member> Members { get; set; }
    }
}
