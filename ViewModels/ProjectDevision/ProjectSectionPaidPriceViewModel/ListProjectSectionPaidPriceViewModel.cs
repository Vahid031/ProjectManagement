using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionPaidPriceViewModel
{
    public class ListProjectSectionPaidPriceViewModel
	{
        public ProjectSectionPaidPrice ProjectSectionPaidPrice { get; set; }
        public ProjectSection ProjectSection{ get; set; }
        public ProjectSectionDraft ProjectSectionDraft{ get; set; }
	}
}