using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionContracts", Schema = "ProjectDevision")]
    public class ProjectSectionContract
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("نوع قرارداد")]
        public Int64? ContractTypeId { get; set; }

        [StringLength(20, MinimumLength = 0, ErrorMessage = "حد اکثر 20 کاراکتر")]
        [DisplayName("شماره قرارداد")]
        public string ContractNumber { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ قرارداد")]
        public string ContractDate { get; set; }

        [StringLength(10, MinimumLength = 0, ErrorMessage = "حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ اتمام قرارداد")]
        public string EndDate { get; set; }

        [DisplayName("مبلغ قرارداد")]
        public Int64? ContractPrice { get; set; }

        [StringLength(200, MinimumLength=0, ErrorMessage="حد اکثر 200 کاراکتر")]
        [DisplayName("مدت قرارداد (ماه)")]
        public string ContractDuration { get; set; }

        [DisplayName("نقشه پلان موقعیت")]
        public int? PlanMapSituation { get; set; }

        [DisplayName("نقشه معماری")]
        public int? ArchitecturePlan { get; set; }

        [DisplayName("نقشه جزیات اجرایی")]
        public int? ExecutionDetailPlan { get; set; }

        [DisplayName("نقشه سازه")]
        public int? StructurePlan { get; set; }

        [DisplayName("نقشه تاسیسات مکانیکی")]
        public int? MechanicalInstallationPlan { get; set; }

        [DisplayName("نقشه تاسیسات برقی")]
        public int? ElectronicalInstallationPlan { get; set; }

        [DisplayName("نقشه حصارکشی")]
        public int? FencePlan { get; set; }

        [DisplayName("قیمت هر واحد")]
        public Int64? UnitPrice { get; set; }

        [DisplayName("درصد پلوس")]
        [DataType("decimal(18 ,2")]
        public decimal? PaulusPercent { get; set; }

        [DisplayName("درصد مینوس")]
        [DataType("decimal(18 ,2")]
        public decimal? MinosPercent { get; set; }

        [StringLength(200, MinimumLength=0, ErrorMessage="حد اکثر 200 کاراکتر")]
        [DisplayName("نحوه پرداخت")]
        public string PaidPriceType { get; set; }

        //[StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        //[DisplayName("تاریخ تحویل موقت")]
        //public string TempDelivaryDate { get; set; }

        //[StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        //[DisplayName("تاریخ تحویل دائم")]
        //public string FixedDeliveryDate { get; set; }


        [DisplayName("دوره تضمین")]
        [Required(ErrorMessage = "الزامی است")]
        [Range(0, 10000, ErrorMessage = "حداکثر 10000 روز می باشد")]
        [RegularExpression("([0-9]+)", ErrorMessage = "فقط عدد وارد کنید")]
        public long GuaranteePeriod { get; set; }


        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ تحویل کارگاه")]
        public string WorkshopDeliveryDate { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ تجهیز کارگاه")]
        public string WorkshopToolingDate { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ شروع به کار")]
        public string StartDate { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ برچیدن کارگاه")]
        public string WorkshopRemoveDate { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("سرپرست کارگاه")]
        public string WorkshopSupervisor { get; set; }

        [ForeignKey("ContractTypeId")]
        public virtual ContractType ContractType { get; set; }

        public virtual ICollection<ProjectSectionContractFile> ProjectSectionContractFiles { get; set; }

        public virtual ICollection<ProjectSectionContractDetail> ProjectSectionContractDetails { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }


    }
}
