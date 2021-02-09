using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionComplementarityDateViewModel
{
    public class CreateProjectSectionComplementarityDateViewModel
	{
        public ProjectSectionComplementarityDate ProjectSectionComplementarityDate { get; set; }

        public string Files { get; set; }
	}
}