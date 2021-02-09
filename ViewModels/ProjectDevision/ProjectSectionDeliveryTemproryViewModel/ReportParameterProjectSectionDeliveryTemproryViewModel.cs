using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDevision.ProjectSectionDeliveryTemproryViewModel
{
    public class ReportParameterProjectSectionDeliveryTemproryViewModel
    {
        [DisplayName("کد پروژه")]
        public string FileCode { get; set; }

        [DisplayName("عنوان پروژه")]
        public string ProjectTitle { get; set; }

        [DisplayName("عنوان فاز پروژه")]
        public string Title { get; set; }
    }
}
