using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionSupervisorVisits", Schema = "ProjectDevision")]
    public class ProjectSectionSupervisorVisit
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        //[ScaffoldColumn(false)]
        [DisplayName("عنوان عملیات")]
        public Int64? ProjectSectionOperationTitleId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("ناظر")]
        public Int64? ProjectSectionMemberId { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ بازدید")]
        public string VisitDate { get; set; }

        [DisplayName("درصد پیشرفت")]
        public int? DevelopmentPercent { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("دستورکار")]
        public string Agendum { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("شرح")]
        public string Description { get; set; }

        [ForeignKey("ProjectSectionMemberId")]
        public virtual ProjectSectionMember ProjectSectionMember { get; set; }

        public virtual ICollection<ProjectSectionSupervisorVisitFile> ProjectSectionSupervisorVisitFiles { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }

        [ForeignKey("ProjectSectionOperationTitleId")]
        public virtual ProjectSectionOperationTitle ProjectSectionOperationTitle { get; set; }
    }
}
