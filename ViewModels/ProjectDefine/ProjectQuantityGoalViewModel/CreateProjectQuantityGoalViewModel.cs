using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;

namespace ViewModels.ProjectDefine.ProjectQuantityGoalViewModel
{
    public class CreateProjectQuantityGoalViewModel
	{
        public ProjectQuantityGoal ProjectQuantityGoal { get; set; }

        public List<Unit> Units { get; set; }
	}
}