using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProjectDevision.ProjectSectionContractViewModel
{
    public class ReportProjectSectionContractViewModel
    {
        public string FileCode { get; set; }
        public string ProjectTitle { get; set; }
        public string Title { get; set; }
        public string EndDate { get; set; }
        public string CompanyName { get; set; }
        public string ContractNumber { get; set; }
        public Int64? ContractTypeId { get; set; }
        public string ContractTypeTitle { get; set; }
    }
}
