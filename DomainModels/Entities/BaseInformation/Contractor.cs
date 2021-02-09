using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.ReqularExpression;
using System.Web.Mvc;

namespace DomainModels.Entities.BaseInformation
{
    [Table("Contractors", Schema = "BaseInformation")]
    public class Contractor
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نام شرکت")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("CompanyNameValidation", "Contractor", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        public string CompanyName { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نام مدیرعامل")]
        [Required(ErrorMessage= "الزامی است")]
        public string ManagerFirstName { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نام خانوادگی مدیرعامل")]
        [Required(ErrorMessage= "الزامی است")]
        public string ManagerLastName { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "موبایل نامعتبر است")]
        [DisplayName("موبایل مدیرعامل")]
        [Required(ErrorMessage= "الزامی است")]
        public string ManagerMobileNumber { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("کد ملی مدیرعامل")]
        [Remote("ManagerNationalCodeValidation", "Contractor", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "کدملی معتبر نیست")]
        public string ManagerNationalCode { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نام نماینده")]
        public string AgentFirstName { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نام خانوادگی نماینده")]
        public string AgentLastName { get; set; }


        [StringLength(11, MinimumLength = 11, ErrorMessage = "موبایل نامعتبر است")]
        [DisplayName("موبایل نماینده")]
        public string AgentMobileNumber { get; set; }

        [StringLength(30, MinimumLength = 0, ErrorMessage = "حد اکثر 30 کاراکتر")]
        [DisplayName("شماره تلفن")]
        [Required(ErrorMessage= "الزامی است")]
        public string PhoneNumber { get; set; }

        [StringLength(30, MinimumLength=0, ErrorMessage="حد اکثر 30 کاراکتر")]
        [DisplayName("شماره فکس")]
        [Required(ErrorMessage= "الزامی است")]
        public string FaxNumber { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("آدرس")]
        [Required(ErrorMessage= "الزامی است")]
        public string Address { get; set; }

        [RegularExpression(CustomRegex.EMAIL, ErrorMessage = "ایمیل را بدرستی وارد کنید")]
        [StringLength(200, MinimumLength=0, ErrorMessage="حد اکثر 200 کاراکتر")]
        [DisplayName("ایمیل")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("سایت")]
        public string Website { get; set; }

        [DisplayName("فعال")]
        public bool? IsActive { get; set; }

        [DisplayName("شماره ثبت")]
        public string RegistrationNumber { get; set; }

        [DisplayName("تاریخ ثبت")]
        public DateTime? RegistrationDate { get; set; }

        [DisplayName("تاریخ ثبت")]
        [NotMapped]
        public string RegistrationDate_ { get; set; }

        [DisplayName("شناسه ملی")]
        public string NationalID { get; set; }


        [DisplayName("کد اقتصادی")]
        public string EconomicCode { get; set; }


        [DisplayName("شماره مجوز ارزش افزوده")]
        public string  LicenseValueAddedNumber { get; set; }

        [DisplayName("نوع پیمانکار")]
        public string ContractorType { get; set; }


        public virtual ICollection<ProjectSectionFinalParticipant> ProjectSectionFinalParticipants { get; set; }
        public virtual ICollection<ProjectSectionFinanceEvaluation> ProjectSectionFinanceEvaluations { get; set; }
        public virtual ICollection<ProjectSectionWinner> ProjectSectionWinners { get; set; }
        public virtual ICollection<ContractorLevel> ContractorLevels { get; set; }
    }
}
