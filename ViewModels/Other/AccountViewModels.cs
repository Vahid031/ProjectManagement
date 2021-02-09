using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModels.Entities;

namespace ViewModels.Other
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "کلمه عبور را وارد کنید")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }
    }


    public class RecoveryViewModel
    {
        [Required]
        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [Required]
        [Display(Name = "موبایل")]
        public string MobileNo { get; set; }
    }


    public class AdminViewModel 
    {
        public AdminViewModel(Int64 id, Int64 roleId, string username, string roleCode)
        {
            Id = id;
            RoleId = roleId;
            UserName = username;
            RoleCode = roleCode;
        }

        public Int64 Id { get; set; }

        public Int64 RoleId { get; set; }

        public string UserName { get; set; }

        public string RoleCode { get; set; }

    }
}
