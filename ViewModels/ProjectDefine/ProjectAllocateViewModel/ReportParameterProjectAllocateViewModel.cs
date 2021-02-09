using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDefine.ProjectAllocateViewModel
{
    public class ReportParameterProjectAllocateViewModel
    {
        [DisplayName("کد پروژه")]
        public string FileCode { get; set; }

        [DisplayName("عنوان پروژه")]
        public string ProjectTitle { get; set; }

        [DisplayName("شهرستان")]
        public Int64? CityId { get; set; }

        [DisplayName("نوع پروژه")]
        public Int64? ProjectTypeId { get; set; }

        public List<City> Cities { get; set; }
        public List<ProjectType> ProjectTypes { get; set; }
    }
}
