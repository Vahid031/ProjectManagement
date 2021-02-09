using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionDrafts", Schema = "ProjectDevision")]
    public class ProjectSectionDraft
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
        public Int64? ProjectSectionStatementConfirmId { get; set; }

        [DisplayName("مبلغ حواله")]
        [Required(ErrorMessage = "الزامی است")]
        [RegularExpression("[0-9]+(,[0-9]+)*", ErrorMessage = "فقط عدد وارد کنید")] // اعدادی که سه رقم سه رقم از هم جدا می شوند 
        public Int64? TempDraftPrice { get; set; }


        [DisplayName("مبلغ تایید شده حواله")]
        [Required(ErrorMessage = "الزامی است")]
        //[RegularExpression("([0-9]+)", ErrorMessage = "فقط عدد وارد کنید")]

        // اعدادی که سه رقم سه رقم از هم جدا می شوند
        [RegularExpression("[0-9]+(,[0-9]+)*", ErrorMessage = "فقط عدد وارد کنید")]
        public Int64? DraftPrice { get; set; }


        [StringLength(10, MinimumLength = 0, ErrorMessage = "حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ حواله")]
        [NotMapped]
        public string DraftDate_ { get; set; }


        [DisplayName("تاریخ حواله")]
        public DateTime? DraftDate { get; set; }



        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("شماره حواله")]
        [Required(ErrorMessage = "الزامی است")]
        [RegularExpression("([0-9]+)", ErrorMessage = "فقط عدد وارد کنید")]
        public string DraftNumber { get; set; }


        [ForeignKey("ProjectSectionStatementConfirmId")]
        public virtual ProjectSectionStatementConfirm ProjectSectionStatementConfirm { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }

        public virtual ICollection<ProjectSectionDraftFile> ProjectSectionDraftFiles { get; set; }

        public virtual ICollection<ProjectSectionPaidPrice> ProjectSectionPaidPrices { get; set; }


    }
}
