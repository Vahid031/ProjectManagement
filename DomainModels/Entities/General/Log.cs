using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.General;

namespace DomainModels.Entities.General
{
    [Table("Logs", Schema = "General")]
    public class Log
    {
        [Key]
        [DisplayName("شناسه لاگ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه کاربران سیستم")]
        public Int64? MemberId { get; set; }

        [StringLength(200, MinimumLength=0, ErrorMessage="حد اکثر 200 کاراکتر")]
        [DisplayName("نام جدول")]
        public string TableName { get; set; }

        [StringLength(200, MinimumLength=0, ErrorMessage="حد اکثر 200 کاراکتر")]
        [DisplayName("نام عملیات")]
        public string OperationName { get; set; }

        [DisplayName("تاریخ")]
        public DateTime? Date { get; set; }

        [DisplayName("سطر")]
        public Int64? RowId { get; set; }

        public virtual ICollection<LogDetail> LogDetails { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }


    }
}
