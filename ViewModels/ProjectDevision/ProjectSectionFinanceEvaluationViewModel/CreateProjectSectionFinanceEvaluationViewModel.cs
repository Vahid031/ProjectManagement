using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionFinanceEvaluationViewModel
{
    public class CreateProjectSectionFinanceEvaluationViewModel
	{
        public ProjectSectionFinanceEvaluation ProjectSectionFinanceEvaluation { get; set; }
        public List<Contractor> Contractors { get; set; }
	}
}