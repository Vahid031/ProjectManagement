using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionCorrespondences", Schema = "ProjectDevision")]
    public class ProjectSectionCorrespondence
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? LetterTypeId { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("شماره نامه")]
        public string LetterNumber { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ نامه")]
        public string LetterDate { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("تاریخ صدور نامه")]
        public string LetterIssue { get; set; }

        [ForeignKey("LetterTypeId")]
        public virtual LetterType LetterType { get; set; }

        public virtual ICollection<ProjectSectionCorrespondenceFile> ProjectSectionCorrespondencFiles { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }


    }
}
