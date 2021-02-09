using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace ViewModels.ProjectDefine.ProjectQuantityGoalViewModel
{
    public class ListProjectQuantityGoalViewModel
	{
        public ProjectQuantityGoal ProjectQuantityGoal { get; set; }
        public Unit Unit { get; set; }
	}
}