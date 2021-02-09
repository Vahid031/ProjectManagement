using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionStatementConfirmViewModel
{
    public class ListProjectSectionStatementConfirmViewModel
	{
        public ProjectSectionStatementConfirm ProjectSectionStatementConfirm { get; set; }
        public ProjectSectionStatement ProjectSectionStatement { get; set; }
        public Opinion Opinion { get; set; }
	}
}