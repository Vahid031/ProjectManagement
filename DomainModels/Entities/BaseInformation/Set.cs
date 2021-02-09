using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDefine;
using System.Web.Mvc;

namespace DomainModels.Entities.BaseInformation
{
    [Table("Sets", Schema = "BaseInformation")]
    public class Set
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("کد")]
        [Required(ErrorMessage = "الزامی است")]
        [Remote("CodeValidation", "Set", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        public string Code { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان ")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("TitleValidation", "Set", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        public virtual ICollection<ProgramPlan> ProgramPlansExecutionSetId { get; set; }

        public virtual ICollection<ProgramPlan> ProgramPlansBeneficiarySetId { get; set; }


    }
}
