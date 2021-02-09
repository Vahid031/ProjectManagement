using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.ProjectDevision.ProjectSectionStatementConfirmViewModel
{
    public class CreateProjectSectionStatementConfirmViewModel
	{
        public ProjectSectionStatementConfirm ProjectSectionStatementConfirm { get; set; }
        
        public List<Opinion> Opinions { get; set; }

        public string Files { get; set; }
	}
}