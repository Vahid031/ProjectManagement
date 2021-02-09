using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.General;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionMembers", Schema = "ProjectDevision")]
    public class ProjectSectionMember
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("انتخاب ناظر / مشاور")]
        public Int64? MemberId { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ شروع به کار")]
        public string StartDate { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ پایان")]
        public string EndDate { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("نوع نظارت ناظر")]
        public Int64? MonitoringTypeId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("ناظر / سرناظر")]
        public bool? IsMainSupervisor { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("شرح")]
        public string Description { get; set; }

        [ForeignKey("MonitoringTypeId")]
        public virtual MonitoringType MonitoringType { get; set; }

        public virtual ICollection<ProjectSectionAdvisorVisit> ProjectSectionAdvisorVisits { get; set; }

        public virtual ICollection<ProjectSectionSupervisorVisit> ProjectSectionSupervisorVisits { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }

    }
}
