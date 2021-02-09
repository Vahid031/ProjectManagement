using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionDeliveryTemproryViewModel
{
    public class CreateProjectSectionDeliveryTemproryViewModel
	{
        public ProjectSectionDeliveryTemprory ProjectSectionDeliveryTemprory { get; set; }

        public string Files { get; set; }

        public List<Opinion> Opinions { get; set; }
        public List<Defect> Defects { get; set; }
	}
}