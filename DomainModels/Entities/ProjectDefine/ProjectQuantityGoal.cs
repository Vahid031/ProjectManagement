using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;

namespace DomainModels.Entities.ProjectDefine
{
    [Table("ProjectQuantityGoals", Schema = "ProjectDefine")]
    public class ProjectQuantityGoal
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectId { get; set; }

        [StringLength(200, MinimumLength=0, ErrorMessage="حد اکثر 200 کاراکتر")]
        [DisplayName("نوع اهداف کمی")]
        public string QuantityGoalType { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("واحد")]
        public Int64? UnitId { get; set; }

        [DisplayName("درصد از کل پروژه")]
        public float? Amount { get; set; }

        [DisplayName("قیمت هر واحد")]
        public Int64? UnitPrice { get; set; }

        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }


    }
}
