using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Infrastracture.ReqularExpression;
using System;

namespace ViewModels.ProjectDefine.ProjectViewModel
{
    public class CreateProjectViewModel
    {
        public Project Project { get; set; }

        [DisplayName("طرح برنامه")]
        [RegularExpression(CustomRegex.Empty, ErrorMessage = "الزامی است")]
        [Remote("ProgramPlanTitleValidation", "Project", "ProjectDefine", AdditionalFields = "ProgramPlanId", ErrorMessage = "نا معتبر است")]
        public string ProgramPlanTitle { get; set; }
        public string Files { get; set; }
        public string ProjectPlans { get; set; }
        public Int64? ProgramPlanId { get; set; }

        public List<Program> Programs { get; set; }
        public List<ProjectType> ProjectTypes { get; set; }
        public List<ExecutionType> ExecutionTypes { get; set; }
        public List<State> States { get; set; }
        public List<City> Cities { get; set; }
        public List<Section> Sections { get; set; }
        public List<Village> Villages { get; set; }
        public List<RuralDistrict> RuralDistrictes { get; set; }
        public List<FinantialYear> FinantialYears { get; set; }
        public List<FinantialYear> ForecastEndFinantialYears { get; set; }
        public List<OwnershipType> OwnershipTypes { get; set; }
        public List<Unit> Units { get; set; }
        public List<ProjectState> ProjectStates { get; set; }
        public List<CreditProvidePlace> CreditProvidePlaces { get; set; }
        public List<AssignmentType> AssignmentType { get; set; }
        public List<ContractType> ContractType { get; set; }
    }
}