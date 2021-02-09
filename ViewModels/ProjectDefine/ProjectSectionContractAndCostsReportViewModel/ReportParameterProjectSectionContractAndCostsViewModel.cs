using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModels.Entities.BaseInformation;

namespace ViewModels.ProjectDefine.ProjectSectionContractAndCostsReportViewModel
{
    public class ReportParameterProjectSectionContractAndCostsViewModel
    {
        [DisplayName("شهرستان")]
        public Int64? CityId { get; set; }
        [DisplayName("کد پروژه")]
        public string FileCode { get; set; }
        [DisplayName("عنوان پروژه")]
        public string ProjectTitle { get; set; }
        [DisplayName("عنوان فاز پروژه")]
        public string ProjectSectionTitle { get; set; }
        [DisplayName("پیمانکار")]
        public Int64? ContractorId { get; set; }
        [DisplayName("نوع قرارداد")]
        public Int64? ContractorTypeId { get; set; }
        [DisplayName("محل تأمین اعتبار")]
        public Int64? CreditProviderPlaceId { get; set; }
        [DisplayName("از تاریخ")]
        public string FromDate { get; set; }
        [DisplayName("تا تاریخ")]
        public string TillDate { get; set; }
        public List<City> Citeies { get; set; }
        [DisplayName("نوع قرارداد")]
        public List<ContractType> ContractTypes { get; set; }
        [DisplayName("محل تأمین اعتبار")]
        public List<CreditProvidePlace> CreditProvidePlaces { get; set; }
        [DisplayName("پیمانکار")]
        public List<Contractor> Contractors { get; set; }

    }
}
