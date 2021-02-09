using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.ProjectDevision.ProjectSectionPaidPriceViewModel
{
    public class CalculateProjectSectionPaidPriceViewModel
	{
        public long? ContractPrice { get; set; }
        public long? TotalDraftPrice { get; set; }
        public long? TotalPadePrice { get; set; }
        public long? Penalty46Price { get; set; }
        public long? RemainingDraftPrice { get; set; }
        public long? RemainingTotalContractPrice { get; set; }
	}
}