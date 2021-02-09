/// <summary>
/// Get String Messages
/// Version 1.4
/// Last Update : 2014/6/17
/// Powered By H.Moradof
/// </summary>
namespace Web.Utilities.AppCode
{

    public static class Message
    {

        public enum TypeAlarm
        {
            success,
            danger,
            info,
            warning
        }

        #region External Properties

        public static string ValidPicExt { get { return "jpg, jpeg, bmp"; } }
        
        public static string ValidAnimatedPicExt { get { return "jpg, bmp, png , gif, ico"; } }
        
        public static string ValidDocExt { get { return "pdf"; } }
        
        public static string ValidZipExt { get { return "rar, zip"; } }
        
        public static string ValidFileExt { get { return "pdf, jpg, jpeg, png, gif, flv, zip, rar"; } }


        #endregion


        #region Properties

        public enum MessageType
        {
            ALLFieldsRequired,
            StarFieldsRequired,
            PassNotEqualToRepass,
            SuccessRegister,
            SuccessInsert, SuccessUpdate, SuccessDelete, SuccessAccept, SuccessDecline, SuccessPending,
            NotAccessAdmin, NotAccessUser,
            ErrorInSystem, ErrorSameEmail, ErrorInInsert, ErrorInUpdate, ErrorInDelete, ErrorInInsertOrUpdate, ErrorInDeleteOrUpdate, ErrorInInputParameter,
            ErrorInValidInputData,
            ErrorInTypingData,
            ErrorTypingDataIsOutOfRanges,
            ErrorInImageResizer,
            ErrorInMailSender,
            ErrorInGettingData,
            ErrorChoosePic,
            ErrorChooseDoc,
            ErrorChooseZip,
            ErrorInvalidAnimatedPicExt,
            ErrorInvalidFileExt,
            ErrorInvalidPicExt,
            ErrorInvalidDocExt,
            ErrorInvalidZipExt,
            ErrorInPicLen,
            ErrorOutputIsEmpty,
            ErrorInSearch,
            NewsLetterSended,
            ErrorInRss,
            ErrorInDeleteOrSet,
            SuccessSetGalleryCover,
            SuccessRefresh,
            ErrorInRefresh,
            ErrorNoCity,
            ErrorNoCountry,
            ErrorEmptyBasket,
            ErrorNotAcceptPayToGetCard,
            ErrorEndCapacity,
            ErrorHaveChild,
            ErrorSameUser,
            ErrorCityHaveMember,
            ErrorInputOneTBox,
            ErrorOldPassword,
            ErrorCantDelete,
            ErrorDeleteSubset,
            ErrorDeleteOrActive,
            ErrorUsernameMistake
        };

        public static bool IsSuccess { get; set; }

        public static MessageType Error { get; set; }

        public static long Number { get; set; }

        #endregion


        #region Methods

        public static string GetMessage(MessageType error = MessageType.SuccessInsert, long number = 0)
        {
            Error = error;
            Number = number;

            string Temp = string.Empty;

            switch (Error)
            {
                case MessageType.ALLFieldsRequired: Temp = "پر کردن تمامی فیلد ها الزامی است."; break;
                case MessageType.StarFieldsRequired: Temp = "پر کردن فیلد های ستاره دار الزامی است."; break;
                case MessageType.PassNotEqualToRepass: Temp = "کلمه عبور و تکرار کلمه عبور یکسان نیست."; break;
                case MessageType.SuccessRegister: Temp = "ثبت نام با موفقیت انجام شد."; break;
                case MessageType.SuccessInsert: Temp = "اطلاعات با موفقیت ثبت شد."; break;
                case MessageType.SuccessUpdate: Temp = "اطلاعات با موفقیت ویرایش شد."; break;
                case MessageType.SuccessDelete: Temp = "اطلاعات با موفقیت حذف شد."; break;
                case MessageType.NotAccessAdmin: Temp = "کاربر محترم ، شما دسترسی لازم جهت انجام این عملیات را ندارید ؛ لطفا از طریق دکمه خروج به صفحه ورود بازگشته و مجددا لاگین نمایید."; break;
                case MessageType.NotAccessUser: Temp = "کاربر محترم ، شما دسترسی لازم جهت انجام این عملیات را ندارید ؛ لطفا از طریق دکمه خروج به صفحه اصلی سایت بازگشته و مجددا لاگین نمایید."; break;
                case MessageType.ErrorInSystem: Temp = "مشکلی در سیستم رخ داده است."; break;
                case MessageType.ErrorSameEmail: Temp = "این آدرس ایمیل قبلا ثبت در سایت شده است."; break;
                case MessageType.ErrorInInsert: Temp = "مشکلی در سیستم ثبت رخ داده است."; break;
                case MessageType.ErrorInUpdate: Temp = "مشکلی در سیستم ویرایش رخ داده است."; break;
                case MessageType.ErrorInDelete: Temp = "آیتم دارای زیرمجموعه می باشد، لذا حذف آن امکان پذیر نیست."; break;
                case MessageType.ErrorInDeleteOrSet: Temp = "مشکلی در سیستم حذف/ست رخ داده است."; break;
                case MessageType.ErrorInDeleteOrUpdate: Temp = "مشکلی در سیستم حذف/ویرایش رخ داده است."; break;
                case MessageType.ErrorInInsertOrUpdate: Temp = "مشکلی در سیستم ثبت/ویرایش رخ داده است."; break;
                case MessageType.ErrorInInputParameter: Temp = "پارامترهای ورودی نادرست است. ، لطفا از دست کاری آدرس بپرهیزید"; break;
                case MessageType.ErrorInTypingData: Temp = "مقدار وارد شده نادرست است."; break;
                case MessageType.ErrorInValidInputData: Temp = "مقدار وارد شده غیر مجاز است."; break;
                case MessageType.ErrorTypingDataIsOutOfRanges: Temp = "مقدار وارد شده بیش از حد مجاز است."; break;
                case MessageType.ErrorInImageResizer: Temp = "مشکلی در سیستم ریسایزر رخ داده است."; break;
                case MessageType.ErrorInMailSender: Temp = "مشکلی در سیستم ارسال ایمیل رخ داده است."; break;
                case MessageType.ErrorInGettingData: Temp = "مشکلی در دریافت اطلاعات رخ داده است"; break;
                case MessageType.ErrorChoosePic: Temp = "انتخاب تصویر الزامی است."; break;
                case MessageType.ErrorChooseDoc: Temp = "انتخاب فایل متنی الزامی است."; break;
                case MessageType.ErrorChooseZip: Temp = "انتخاب فایل فشرده الزامی است."; break;
                case MessageType.ErrorInvalidAnimatedPicExt: Temp = string.Format("فرمت فایل تصویر غیرمجاز است.<br/>(فرمت های مجاز: {0})", ValidAnimatedPicExt); break;
                case MessageType.ErrorInvalidPicExt: Temp = string.Format("فرمت فایل تصویر غیرمجاز است.<br/> (فرمت های مجاز: {0})", ValidPicExt); break;
                case MessageType.ErrorInvalidDocExt: Temp = string.Format("فرمت فایل متنی غیرمجاز است.<br/> (فرمت های مجاز: {0})", ValidDocExt); break;
                case MessageType.ErrorInvalidZipExt: Temp = string.Format("فرمت فایل فشرده غیرمجاز است.<br/> (فرمت های مجاز: {0})", ValidZipExt); break;
                case MessageType.ErrorInvalidFileExt: Temp = string.Format("فرمت فایل غیرمجاز است.<br/> (فرمت های مجاز: {0})", ValidFileExt); break;
                case MessageType.ErrorInPicLen: Temp = "حجم تصویر بیش از حد مجاز است."; break;
                case MessageType.ErrorOutputIsEmpty: Temp = "موردی یافت نشد."; break;
                case MessageType.ErrorInSearch: Temp = "مشکلی در سیستم جستجو رخ داده است."; break;
                case MessageType.NewsLetterSended: Temp = string.Format("تعداد {0} خبرنامه ارسال گردید.",Number); break;
                case MessageType.SuccessRefresh: Temp = "به روز رسانی با موفقیت انجام شد"; break;
                case MessageType.ErrorInRefresh: Temp = "مشکلی در سیستم به روزرسانی رخ داد ه است."; break;
                case MessageType.SuccessAccept: Temp = "اطلاعات با موفقیت تایید شد."; break;
                case MessageType.SuccessDecline: Temp = "اطلاعات با موفقیت رد شد."; break;
                case MessageType.SuccessPending: Temp = "اطلاعات با موفقیت در انتظار تایید شد."; break;
                case MessageType.ErrorSameUser: Temp = "نام کاربری انتخابی شما قبلا در سیستم ثبت شده است ، لطفا نام کاربری دیگری برگزینید."; break;
                case MessageType.ErrorInputOneTBox: Temp = "حداقل باید یکی از فیلدها را پر نمایید."; break;
                case MessageType.ErrorHaveChild: Temp = "این گزینه دارای زیر مجموعه می باشد ، لذا حذف آن میسر نیست."; break;
                case MessageType.ErrorOldPassword: Temp = "رمز عبور قبلی اشتباه است."; break;
                case MessageType.ErrorCantDelete: Temp = "این شخص قابل حذف نمی باشد."; break;
                case MessageType.ErrorDeleteSubset: Temp = "این شخص زیرمجموعه دارد و سیستم قادر به حذف نیست."; break;
                case MessageType.ErrorDeleteOrActive: Temp = "شما مجاز به حذف این شخص نیستید این شخص فعال می باشد و فقط مدیر سیستم مجاز به حذف می باشد."; break;
                case MessageType.SuccessSetGalleryCover: Temp = "کاور گالری با موفقیت تنظیم شد."; break;
                case MessageType.ErrorUsernameMistake: Temp = "نام کاربری یا کلمه عبور اشتباه است."; break;
                    
                default:
                    Temp = "پیام مورد نظر یافت نشد!";
                    break;
            }


            return Temp;

        }

        #endregion

    }
}