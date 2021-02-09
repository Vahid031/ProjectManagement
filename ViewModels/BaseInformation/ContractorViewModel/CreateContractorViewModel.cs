using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;

namespace ViewModels.BaseInformation.ContractorViewModel
{
    public class CreateContractorViewModel
	{
        public Contractor Contractor { get; set; }

        public string ContractorLevels { get; set; }

        public List<MajorType> MajorTypes { get; set; }
        public List<LevelType> LevelTypes { get; set; }
	}
}