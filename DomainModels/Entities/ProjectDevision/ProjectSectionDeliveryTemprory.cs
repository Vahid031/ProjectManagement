using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionDeliveryTemprories", Schema = "ProjectDevision")]
    public class ProjectSectionDeliveryTemprory
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("شماره درخواست")]
        public string RequestNumber { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ درخواست")]
        public string RequestDate { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("شماره تشکیل جلسه")]
        public string ConveneNumber { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ تشکیل جلسه")]
        public string ConveneDate { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ برگزاری جلسه")]
        public string MeetingDate { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("نواقص")]
        public Int64? DefectId { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("ریز نواقص")]
        public string DefectDetails { get; set; }

        [DisplayName("مبلغ پرداختی")]
        public Int64? PaidPrice { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("نظر کمیسیون")]
        public Int64? CommisionOpinionId { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("شرح")]
        public string Description { get; set; }

        public virtual ICollection<ProjectSectionDeliveryTemproryFile> ProjectSectionDeliveryTemproryFiles { get; set; }

        [ForeignKey("CommisionOpinionId")]
        public virtual Opinion Opinion { get; set; }

        [ForeignKey("DefectId")]
        public virtual Defect Defect { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }


    }
}
