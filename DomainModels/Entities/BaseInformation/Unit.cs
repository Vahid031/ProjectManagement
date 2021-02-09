using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;
using System.Web.Mvc;

namespace DomainModels.Entities.BaseInformation
{
    [Table("Units", Schema = "BaseInformation")]
    public class Unit
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("واحد")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("TitleValidation", "Unit", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        public virtual ICollection<ProjectQuantityGoal> ProjectQuantityGoals { get; set; }

        public virtual ICollection<ProjectSectionContractDetail> ProjectSectionContractDetails { get; set; }

        public virtual ICollection<Project> Projects { get; set; }


    }
}
