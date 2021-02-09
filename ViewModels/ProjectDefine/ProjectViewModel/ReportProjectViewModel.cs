using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProjectViewModel
{
    public class ReportProjectViewModel
    {
        public string ProjectTitle { get; set; }
        public string FileCode { get; set; }
        public Int64? ProjectTypeId { get; set; }
        public string ProjectTypeTitle { get; set; }
        public Int64? ExecutionTypeId { get; set; }
        public string ExecutionTypeTitle { get; set; }
        public Int64? CityId { get; set; }
        public string CityTitle { get; set; }
        public Int64? StartFinantialYearId { get; set; }
        public string StartFinantialYearTitle { get; set; }
        public Int64? EstimateCost { get; set; }
        public Int64? ForecastEndFinantialYearId { get; set; }
        public string ForecastEndFinantialYearTitle { get; set; }
        public Int64? OwnershipTypeId { get; set; }
        public string OwnershipTypeTitle { get; set; }
        public Int64? Area { get; set; }
        public Int64? CloseArea { get; set; }
        public Int64? OpenArea { get; set; }
        public string Address { get; set; }
    }
}
