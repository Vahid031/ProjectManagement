using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Infrastracture.ReqularExpression;
using System.Web.Mvc;

namespace ViewModels.ProjectDefine.ProgramPlanViewModel
{
    public class CreateProgramPlanViewModel
	{
        public ProgramPlan ProgramPlan { get; set; }

        [DisplayName("دستگاه اجرایی")]
        [Remote("ExecutionSetTitleValidation", "ProgramPlan", "ProjectDefine", AdditionalFields = "ProgramPlan.ExecutionSetId", ErrorMessage = "نا معتبر است")]
        public string ExecutionSetTitle { get; set; }

        [DisplayName("دستگاه بهره برداری")]
        [Remote("BeneficiarySetTitleValidation", "ProgramPlan", "ProjectDefine", AdditionalFields = "ProgramPlan.BeneficiarySetId", ErrorMessage = "نا معتبر است")]
        public string BeneficiarySetTitle { get; set; }


        public List<Program> Programs { get; set; }
        public List<PlanType> PlanTypes { get; set; }
        public List<FinantialYear> FinantialYears { get; set; }
	}
}