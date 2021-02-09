using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.BaseInformation.ContractorViewModel
{
    public class ReportContractorViewModel
    {
        public string ContractorType { get; set; }
        public string CompanyName { get; set; }
        public string ManagerName { get; set; }
        public string ManagerMobileNumber { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
    }
}
