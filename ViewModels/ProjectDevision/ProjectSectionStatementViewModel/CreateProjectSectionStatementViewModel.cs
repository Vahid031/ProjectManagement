using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.ProjectDevision.ProjectSectionStatementViewModel
{
    public class CreateProjectSectionStatementViewModel
	{
        public ProjectSectionStatement ProjectSectionStatement { get; set; }
        public string Files { get; set; }

        [DisplayName("موقت / قطعی")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? TempFixedId { get; set; }

        public List<Opinion> Opinions { get; set; }
        public List<StatementType> StatementTypes { get; set; }
        public List<TempFixed> TempFixeds { get; set; }
        public List<StatementNumber> StatementNumbers { get; set; }
	}
}