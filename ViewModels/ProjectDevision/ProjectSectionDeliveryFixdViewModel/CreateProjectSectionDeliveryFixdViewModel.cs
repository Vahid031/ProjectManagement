using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionDeliveryFixdViewModel
{
    public class CreateProjectSectionDeliveryFixdViewModel
	{
        public ProjectSectionDeliveryFixd ProjectSectionDeliveryFixd { get; set; }

        public string Files { get; set; }

        public List<Opinion> Opinions { get; set; }
        public List<Defect> Defects { get; set; }
	}
}