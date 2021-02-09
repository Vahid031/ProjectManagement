using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionRecoupments", Schema = "ProjectDevision")]
    public class ProjectSectionRecoupment
    {
        [Key]
        [DisplayName("مفاصا حساب")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("نوع مفاصا حساب")]
        public Int64? RecoupmentTypeId { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("شماره")]
        [Required(ErrorMessage = "الزامی است")]
        public string Number { get; set; }

        [DisplayName("تاریخ")]
        public DateTime? Date { get; set; }

        [StringLength(10, MinimumLength = 0, ErrorMessage = "حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ")]
        [NotMapped]
        public string Date_ { get; set; }


        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("شعبه صادرکننده")]
        [Required(ErrorMessage = "الزامی است")]
        public string BranchExporter { get; set; }

        [ForeignKey("RecoupmentTypeId")]
        public virtual RecoupmentType RecoupmentType { get; set; }

        public virtual ICollection<ProjectSectionRecoupmentFile> ProjectSectionRecoupmentFiles { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }


    }
}
