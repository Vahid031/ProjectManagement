using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionViewModel
{
    public class ListProjectSectionViewModel
	{
        public ProjectSection ProjectSection { get; set; }
        public AssignmentType AssignmentType { get; set; }
        public ProjectSectionOperationState ProjectSectionOperationState { get; set; }
        public ProjectSectionAssignmentState ProjectSectionAssignmentState { get; set; }
	}
   
}