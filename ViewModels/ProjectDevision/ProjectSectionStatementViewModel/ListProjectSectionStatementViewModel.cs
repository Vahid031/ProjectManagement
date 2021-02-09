using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionStatementViewModel
{
    public class ListProjectSectionStatementViewModel
	{
        public ProjectSectionStatement ProjectSectionStatement { get; set; }
        public Opinion Opinion { get; set; }
        public StatementType StatementType { get; set; }
        public TempFixed TempFixed { get; set; }
        public StatementNumber StatementNumber { get; set; }
        public ProjectSectionStatementConfirm ProjectSectionStatementConfirm { get; set; }
        //public int StatementStatus { get; set; }
	}
}