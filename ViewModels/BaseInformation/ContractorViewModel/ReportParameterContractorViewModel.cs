using DomainModels.Entities.BaseInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.BaseInformation.ContractorViewModel
{
    public class ReportParameterContractorViewModel
    {
        [DisplayName("نوع پیمانکار")]
        public string ContractorType { get; set; }

        [DisplayName("فعال")]
        public bool? IsActive { get; set; }

        [DisplayName("رشته پیمانکار")]
        public Int64? MajorTypeId { get; set; }
        
        [DisplayName("رتبه پیمانکار")]
        public Int64? LevelTypeId { get; set; }

        public List<MajorType> MajorTypes { get; set; }
        public List<LevelType> LevelTypes { get; set; }
    }
}
