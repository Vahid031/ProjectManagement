using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.General;

namespace DomainModels.Entities.BaseInformation
{
    [Table("NotificationTypes", Schema = "BaseInformation")]
    public class NotificationType
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان ")]
        [Required(ErrorMessage= "الزامی است")]
        public string Title { get; set; }

        [StringLength(5, MinimumLength=0, ErrorMessage="حد اکثر 5 کاراکتر")]
        [DisplayName("زمان قبل از ارسال")]
        public string TimeBeforeSend { get; set; }

        [StringLength(1000, MinimumLength=0, ErrorMessage="حد اکثر 1000 کاراکتر")]
        [DisplayName("مسیر")]
        [Required(ErrorMessage= "الزامی است")]
        public string Url { get; set; }

        [DisplayName("فعال")]
        public bool? IsActive { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }


    }
}
