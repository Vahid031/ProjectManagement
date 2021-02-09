using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.ReqularExpression;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionStatementConfirms", Schema = "ProjectDevision")]
    public class ProjectSectionStatementConfirm
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
        public Int64? ProjectSectionStatementId { get; set; }

        [DisplayName("نظر واحد فنی")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? FinalOpinionId { get; set; }

        [DisplayName("مبلغ")]
        [Required(ErrorMessage = "الزامی است")]
        //[RegularExpression("([0-9]+)", ErrorMessage="فقط عدد وارد کنید")]

        // اعدادی که سه رقم سه رقم از هم جدا می شوند
        [RegularExpression("[0-9]+(,[0-9]+)*", ErrorMessage = "فقط عدد وارد کنید")]
        public Int64? Price { get; set; }

        [StringLength(10, MinimumLength = 0, ErrorMessage = "حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ تایید")]
        [RegularExpression(CustomRegex.PERSIAN_DATE, ErrorMessage = "تاریخ را درست وارد کنید")]
        public string Date { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("شرح")]
        public string Description { get; set; }

        [ForeignKey("FinalOpinionId")]
        public virtual Opinion Opinion { get; set; }

        [ForeignKey("ProjectSectionStatementId")]
        public virtual ProjectSectionStatement ProjectSectionStatement { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }

        public virtual ICollection<ProjectSectionDraft> ProjectSectionDrafts { get; set; }

        public virtual ICollection<ProjectSectionStatementConfirmFile> ProjectSectionStatementConfirmFiles { get; set; }
    }
}
