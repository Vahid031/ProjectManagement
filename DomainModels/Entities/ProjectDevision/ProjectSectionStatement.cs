using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;
using System.Web.Mvc;
using Infrastracture.ReqularExpression;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionStatements", Schema = "ProjectDevision")]
    public class ProjectSectionStatement
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("نظر واحد فنی")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? TechnicalOpinionId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("وضعیت صورت وضعیت")]
        public Int64? ProjectSectionStatementStateId { get; set; }
        
        [ScaffoldColumn(false)]
        [DisplayName("نوع صورت وضعیت")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? StatementTypeId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شماره صورت وضعیت")]
        [Required(ErrorMessage = "الزامی است")]
        [Remote("CodeValidation", "ProjectSectionStatement", AdditionalFields = "Id,ProjectSectionId,StatementTypeId", ErrorMessage = "تکراری است ")]
        public Int64? StatementNumberId { get; set; }

        [DisplayName("مبلغ اعلامی پیمانکار")]
        public Int64? ContractPrice { get; set; }

        [StringLength(10, MinimumLength = 0, ErrorMessage = "حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ ارائه صورت وضعیت توسط پیمانکار")]
        [RegularExpression(CustomRegex.PERSIAN_DATE, ErrorMessage = "تاریخ را درست وارد کنید")]
        public string ContractDate { get; set; }

        [DisplayName("مبلغ پیشنهادی مشاور")]
        public Int64? AdvisorPrice { get; set; }

        [DisplayName("مبلغ پیشنهادی ناظر")]
        public Int64? ConfirmPrice { get; set; }

        [ForeignKey("StatementTypeId")]
        public virtual StatementType StatementType { get; set; }

        [ForeignKey("StatementNumberId")]
        public virtual StatementNumber StatementNumber { get; set; }

        [ForeignKey("TechnicalOpinionId")]
        public virtual Opinion Opinion { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }

        [ForeignKey("ProjectSectionStatementStateId")]
        public virtual ProjectSectionStatementState ProjectSectionStatementState { get; set; }

        public virtual ICollection<ProjectSectionStatementFile> ProjectSectionStatementFiles { get; set; }

        public virtual ICollection<ProjectSectionStatementConfirm> ProjectSectionStatementConfirms { get; set; }

    }
}
