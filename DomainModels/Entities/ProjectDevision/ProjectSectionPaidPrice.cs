using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionPaidPrices", Schema = "ProjectDevision")]
    public class ProjectSectionPaidPrice
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("نوع بیمه")]
        public Int64? InsuranceType { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionDraftId { get; set; }

        [DisplayName("مبلغ حواله")]
        public Int64? DraftPrice { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("شماره حواله")]
        public string DraftNumber { get; set; }

        [DisplayName("تاریخ حواله")]
        public DateTime? DraftDate { get; set; }


        [DisplayName("تاریخ حواله")]
        [NotMapped]
        public string DraftDate_ { get; set; }


        //change float to Int64
        [DisplayName("سپرده")]
        public Int64? Deposit { get; set; }

        [DisplayName("پیش پرداخت")]
        public Int64? PrePayment { get; set; }

        [DisplayName("جریمه")]
        public Int64? Forfeit { get; set; }


        //change float to Int64
        [DisplayName("بیمه سهم پیمانکار")]
        public Int64? ContractorInsurance { get; set; }


        //change float to Int64
        [DisplayName("مالیات")]
        public Int64? Tax { get; set; }

        [DisplayName("سایر بدهی ها")]
        public Int64? OtherDept { get; set; }

        [DisplayName("جمع کسورات پیمانکار")]
        public Int64? TotalContractorDeductions { get; set; }


        //change float to Int64
        [DisplayName("بیمه سهم کارفرما")]
        public Int64? MasterInsurance { get; set; }


        //change float to Int64
        [DisplayName("مالیات بر ارزش افزوده")]
        public Int64? ValueTax { get; set; }

        [DisplayName("جمع کسورات کارفرما")]
        public Int64? TotalMasterDeductions { get; set; }

        [DisplayName("مبلغ قابل پرداخت")]
        public Int64? PayablePrice { get; set; }

        [DisplayName("مبلغ پرداختی")]
        public Int64? PaidPrice { get; set; }


        [DisplayName("تاریخ پرداخت")]
        public DateTime? PaidDate { get; set; }



        [DisplayName("تاریخ پرداخت")]
        [NotMapped]
        public string PaidDate_ { get; set; }


        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("شرح")]
        public string Description { get; set; }

        public virtual ICollection<ProjectSectionPaidPriceFile> ProjectSectionPaidPriceFiles { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }

        [ForeignKey("ProjectSectionDraftId")]
        public virtual ProjectSectionDraft ProjectSectionDraft { get; set; }

        public virtual ICollection<ProjectSectionGovermentWorthyDocumentsPaidPrice> ProjectSectionGovermentWorthyDocumentsPaidPrice { get; set; }


    }
}
