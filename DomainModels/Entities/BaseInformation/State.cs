using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using System.Web.Mvc;

namespace DomainModels.Entities.BaseInformation
{
    [Table("States", Schema = "BaseInformation")]
    public class State
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("کد استان")]
        [Remote("CodeValidation", "State", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        public string Code { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان استان")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("TitleValidation", "State", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        public virtual ICollection<City> Cities { get; set; }

        public virtual ICollection<Project> Projects { get; set; }


    }
}
