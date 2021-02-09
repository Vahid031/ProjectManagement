using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.ProjectDevision.ProjectSectionPaidPriceViewModel
{
    public class CreateProjectSectionPaidPriceViewModel
	{
        public ProjectSectionPaidPrice ProjectSectionPaidPrice { get; set; }
        public string Files { get; set; }
        
        public ProjectSectionStatement ProjectSectionStatement{ get; set; }
        public ProjectSectionDraft ProjectSectionDraft { get; set; }
	}
}