using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDevision.ProjectSectionMadeh46ViewModel
{
    public class ReportProjectSectionMadeh46ViewModel
    {
        public string FileCode { get; set; }
        public string ProjectTitle { get; set; }
        public string Title { get; set; }
        public string LetterNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string LetterDate { get; set; }
        public Int64? ForfeitPrice { get; set; }
        public Int64? ConfirmPrice { get; set; }
    }
}
