using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionFinalParticipants", Schema = "ProjectDevision")]
    public class ProjectSectionFinalParticipant
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("پیمانکار")]
        public Int64? ContractorId { get; set; }

        [DisplayName("مبلغ پیشنهادی")]
        public Int64? SuggestPrice { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("شماره تضمین")]
        public string WarrantyNumber { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ اتمام ضمانت نامه")]
        public string WarrantyDate { get; set; }

        public virtual ICollection<ProjectSectionFinalParticipantFile> ProjectSectionFinalParticipantFiles { get; set; }

        [ForeignKey("ContractorId")]
        public virtual Contractor Contractor { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }
    }
}
