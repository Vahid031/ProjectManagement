using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionRecoupmentViewModel
{
    public class ListProjectSectionRecoupmentViewModel
	{
        public ProjectSectionRecoupment ProjectSectionRecoupment { get; set; }
        public RecoupmentType RecoupmentType { get; set; }
	}
}