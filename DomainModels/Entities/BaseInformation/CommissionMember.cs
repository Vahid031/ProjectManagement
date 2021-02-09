using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DomainModels.Entities.BaseInformation
{
    [Table("CommissionMembers", Schema = "BaseInformation")]
    public class CommissionMember
    {
        [Key]
        [DisplayName("اعضای کمیسیون")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نام")]
        [Required(ErrorMessage= "الزامی است")]
        public string FirstName { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نام خانوادگی")]
        [Required(ErrorMessage= "الزامی است")]
        public string LastName { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("سمت")]
        public string Post { get; set; }

        [StringLength(11, MinimumLength=11, ErrorMessage="موبایل معتبر نیست")]
        [DisplayName("موبایل")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("MobileNumberValidation", "CommissionMember", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        public string MobileNumber { get; set; }

        [DisplayName("فعال")]
        public bool? IsActive { get; set; }


    }
}
