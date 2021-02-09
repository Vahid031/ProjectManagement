using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.General;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.ReqularExpression;
using System.Web.Mvc;

namespace DomainModels.Entities.General
{
    [Table("Members", Schema = "General")]
    public class Member
    {
        [Key]
        [DisplayName("کاربران سیستم")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("گروه کاربری")]
        public Int64? MemberGroupId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("نقش")]
        public Int64? RoleId { get; set; }

        [Remote("FirstNameValidation", "Member", "General", AdditionalFields = "Id,LastName,FatherName", ErrorMessage = "تکراری است")]
        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نام")]
        [Required(ErrorMessage= "الزامی است")]
        public string FirstName { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نام خانوادگی")]
        [Required(ErrorMessage= "الزامی است")]
        public string LastName { get; set; }

        [Remote("UserNameValidation", "Member", "General", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        [StringLength(1000, MinimumLength=0, ErrorMessage="حد اکثر 1000 کاراکتر")]
        [DisplayName("نام کاربری")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(1000, MinimumLength=0, ErrorMessage="حد اکثر 1000 کاراکتر")]
        [DisplayName("رمز عبور")]
        public string Password { get; set; }

        [NotMapped]
        [DisplayName("تکرار رمز عبور")]
        [StringLength(1000, MinimumLength=0, ErrorMessage="حد اکثر 1000 کاراکتر")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "مقادیر یکسان نیست")]
        [DataType(DataType.Password)]
        public string PasswordCampare { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نام پدر")]
        public string FatherName { get; set; }

        [RegularExpression(CustomRegex.PERSIAN_DATE, ErrorMessage = "تاریخ را بدرستی وارد کنید")]
        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ تولد")]
        public string BirthdayDate { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("شغل")]
        public string Job { get; set; }

        [DisplayName("جنسیت")]
        public bool? Gender { get; set; }

        [Remote("NationalCodeValidation", "Member", "General", ErrorMessage = "کد ملی معتبر نیست")]
        [DisplayName("کد ملی")]
        public string NationalCode { get; set; }

        [RegularExpression(CustomRegex.EMAIL, ErrorMessage = "ایمیل را بدرستی وارد کنید")]
        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("ایمیل")]
        public string Email { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("آدرس")]
        public string Address { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("شماره کارت")]
        public string CartNumber { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("شرح")]
        public string Description { get; set; }

        [DisplayName("فعال")]
        public bool? IsActive { get; set; }

        [ForeignKey("MemberGroupId")]
        public virtual MemberGroup MemberGroup { get; set; }

        public virtual ICollection<ProjectSectionMember> ProjectSectionMembers { get; set; }

        public virtual ICollection<Log> Logs { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}
