using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;

namespace DomainModels.Entities.General
{
    [Table("Notifications", Schema = "General")]
    public class Notification
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان ")]
        [Required(ErrorMessage= "الزامی است")]
        public string Title { get; set; }

        [StringLength(1000, MinimumLength=0, ErrorMessage="حد اکثر 1000 کاراکتر")]
        [DisplayName("مسیر")]
        [Required(ErrorMessage= "الزامی است")]
        public string Url { get; set; }

        [StringLength(16, MinimumLength=0, ErrorMessage="حد اکثر 16 کاراکتر")]
        [DisplayName("تاریخ")]
        [Required(ErrorMessage= "الزامی است")]
        public string DateTime { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 NotificationTypeId { get; set; }

        [DisplayName("اولویت")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Priority { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("محتوا")]
        [Required(ErrorMessage= "الزامی است")]
        public string Content { get; set; }

        [DisplayName("فعال")]
        public bool? IsActive { get; set; }

        [DisplayName("شماره ستون")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 ColumnId { get; set; }

        [StringLength(1000, MinimumLength=0, ErrorMessage="حد اکثر 1000 کاراکتر")]
        [DisplayName("لینک صفحه")]
        [Required(ErrorMessage= "الزامی است")]
        public string GotoPage { get; set; }

        [ForeignKey("NotificationTypeId")]
        public virtual NotificationType NotificationType { get; set; }


    }
}
