using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDevision.ProjectSectionDeliveryTemproryViewModel
{
    public class ReportProjectSectionDeliveryTemproryViewModel
    {
        public string FileCode { get; set; }
        public string ProjectTitle { get; set; }
        public string Title { get; set; }
        public string RequestNumber { get; set; }
        public string RequestDate { get; set; }
        public string ConveneNumber { get; set; }
        public string ConveneDate { get; set; }
        public string MeetingDate { get; set; }
        public string DefectDetails { get; set; }
        public Int64? PaidPrice { get; set; }
        public Int64? CommisionOpinionId { get; set; }
        public string CommisionOpinionTitle { get; set; }
        public string Description { get; set; }
    }
}
