using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.General;

namespace DomainModels.Entities.General
{
    [Table("LogDetails", Schema = "General")]
    public class LogDetail
    {
        [Key]
        [DisplayName("شناسه جرئیات لاگ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه لاگ")]
        public Int64? LogId { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نام خصوصیت")]
        public string PropertyName { get; set; }

        [StringLength(1000, MinimumLength=0, ErrorMessage="حد اکثر 1000 کاراکتر")]
        [DisplayName("مقدار خصوصیت")]
        public string PropertyValue { get; set; }

        [ForeignKey("LogId")]
        public virtual Log Log { get; set; }


    }
}
