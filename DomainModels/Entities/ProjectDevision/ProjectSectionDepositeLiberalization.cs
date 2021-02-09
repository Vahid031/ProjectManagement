using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionDepositeLiberalizations", Schema = "ProjectDevision")]
    public class ProjectSectionDepositeLiberalization
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ پرداخت")]
        public string PaidDate { get; set; }

        [DisplayName("مبلغ (تحویل موقت)")]
        public Int64? TemproryPrice { get; set; }

        [DisplayName("مبلغ (تحویل قطعی)")]
        public Int64? FixedPrice { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("شرح")]
        public string Description { get; set; }

        public virtual ICollection<ProjectSectionDepositeLiberalizationFile> ProjectSectionDepositeLiberalizationFiles { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }


    }
}
