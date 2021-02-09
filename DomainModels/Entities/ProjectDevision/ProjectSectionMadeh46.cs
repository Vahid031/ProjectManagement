using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionMadeh46s", Schema = "ProjectDevision")]
    public class ProjectSectionMadeh46
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        [DisplayName("مبلغ تایید شده")]
        public Int64? ConfirmPrice { get; set; }

        [DisplayName("مبلغ جریمه")]
        public Int64? ForfeitPrice { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("شماره نامه")]
        public string LetterNumber { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ نامه")]
        public string LetterDate { get; set; }


        [DisplayName("شماره مجوز ")]
        [Required(ErrorMessage = "الزامی است")]
        [StringLength(10, MinimumLength = 0, ErrorMessage = "حد اکثر 10 کاراکتر")]
        public string LicenseNumber { get; set; }

        public virtual ICollection<ProjectSectionMadeh46File> ProjectSectionMadeh46Files { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }


    }
}
