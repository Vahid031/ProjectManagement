using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;


namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionGovermentWorthyDocumentsPaidPrices", Schema = "ProjectDevision")]
    public class ProjectSectionGovermentWorthyDocumentsPaidPrice
    {
        [Key]
        [DisplayName("شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage="الزامی است")]
        public Int64 Id { get; set; }


        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionPaidPriceId { get; set; }


        [StringLength(20,MinimumLength=0,ErrorMessage="حداکثر 20 کاراکتر")]
        [DisplayName("نوع ملی/استانی")]
        public string Type_MeliOstani { get; set; }


        [StringLength(30, MinimumLength = 0, ErrorMessage = "حداکثر 30 کاراکتر")]
        [DisplayName("نوع اسناد")]
        public string DocumentsType { get; set; }


        [StringLength(11, MinimumLength = 0, ErrorMessage = "حداکثر 11 کاراکتر")]
        [DisplayName("تاریخ گواهی تخصیص")]
        public string CertificateAssignmentDate { get; set; }


        [StringLength(20, MinimumLength = 0, ErrorMessage = "حداکثر 20 کاراکتر")]
        [DisplayName("شماره گواهی تخصیص")]
        public string CertificateAssignmentNumber { get; set; }


        [StringLength(11, MinimumLength = 0, ErrorMessage = "حداکثر 11 کاراکتر")]
        [DisplayName("تاریخ تخصیص اوراق")]
        public string DocumentsAssignmentDate { get; set; }


        [DisplayName("تعداد اوراق یک میلیون ریالی")]
        public int? NumberOfMilionRialsPapers { get; set; }


        [StringLength(30, MinimumLength = 0, ErrorMessage = "حداکثر 30 کاراکتر")]
        [DisplayName("بانک")]
        public string Bank { get; set; }


        [StringLength(20, MinimumLength = 0, ErrorMessage = "حداکثر 20 کاراکتر")]
        [DisplayName("شماره شبا")]
        public string SHEBA { get; set; }


        [StringLength(20, MinimumLength = 0, ErrorMessage = "حداکثر 20 کاراکتر")]
        [DisplayName("شماره معاملاتی مالک اوراق بهادار")]
        public string WorthyDocumentsOwnerTradingCode { get; set; }


        [StringLength(11, MinimumLength = 0, ErrorMessage = "حداکثر 11 کاراکتر")]
        [DisplayName("تاریخ انتشار اوراق")]
        public string DocumentsPublishedDate { get; set; }


        [StringLength(11, MinimumLength = 0, ErrorMessage = "حداکثر 11 کاراکتر")]
        [DisplayName("تاریخ سررسید اوراق")]
        public string DocumentsDueDate { get; set; }


        [ForeignKey("ProjectSectionPaidPriceId")]
        public virtual ProjectSectionPaidPrice ProjectSectionPaidPrice { get; set; }

    }
}