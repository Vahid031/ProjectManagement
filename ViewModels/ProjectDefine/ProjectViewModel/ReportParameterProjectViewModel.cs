using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProjectViewModel
{
    public class ReportParameterProjectViewModel
    {
        [DisplayName("نوع پروژه")]
        public Int64? ProjectTypeId { get; set; }

        [DisplayName(" نحوه اجرا")]
        public Int64? ExecutionTypeId { get; set; }

        [DisplayName("سال شروع")]
        public Int64? StartFinantialYearId { get; set; }

        [DisplayName("شهرستان")]
        public Int64? CityId { get; set; }

        [DisplayName("پیش بینی سال خاتمه")]
        public Int64? ForecastEndFinantialYearId { get; set; }

        [DisplayName("وضعیت مالکیت")]
        public Int64? OwnershipTypeId { get; set; }


        public List<ProjectType> ProjectTypes { get; set; }
        public List<ExecutionType> ExecutionTypes { get; set; }
        public List<FinantialYear> FinantialYears { get; set; }
        public List<City> Citeis { get; set; }
        public List<OwnershipType> OwnershipTypes { get; set; }
    }
}
