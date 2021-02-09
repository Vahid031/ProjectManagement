using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Caching;
using System.Web.Mvc;
using System.IO;

namespace Web.Utilities.AppCode
{
    public static class Messages
    {
        public static string GetMsg(MsgKey key)
        {
            try
            {
                XmlDocument _XmlDocument = new XmlDocument();
                string message = "";

                // اگر فایل پیغام ها کش نبود این کد فایل را در سرور کش می کند
                if (HttpContext.Current.Cache["Messages"] == null)
                {
                    _XmlDocument.Load(HttpContext.Current.Server.MapPath("~/Utilities/Files/Messages.xml"));

                    // کش زمان نامحدود
                    HttpContext.Current.Cache.Add("Messages", _XmlDocument, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

                    // کش محدود به زمان
                   // HttpContext.Current.Cache.Add("Messages", _XmlDocument, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
                }


                // اگر فایل در سرور کش بود این کد فایل را از کش سرور دریافت می کند
                if (!_XmlDocument.HasChildNodes)
                    _XmlDocument = HttpContext.Current.Cache["Messages"] as XmlDocument;


                // این کد برای جستجوی کلید مورد نظر برای گرفتن پیغام می باشد
                XmlNodeList _NodeList = _XmlDocument.SelectNodes("Catalog/Message");
                foreach (XmlNode item in _NodeList)
                {
                    if (item.Attributes["Key"].Value == key.ToString())
                    {
                        message = item.SelectNodes("Text").Item(0).InnerText;
                        break;
                    }
                }

                return message;
            }
            catch (Exception)
            {
                return "سیستم پیغام با مشکل مواجه شده است";
            }
        }
    }

    public enum AlarmType
    {
        none,
        success,
        danger,
        info,
        warning
    }


    public enum MsgKey
    {
        SuccessInsert,
        SuccessUpdate,
        SuccessDelete,
        ErrorSystem,
        ErrorSameEmail,
        ErrorInsert,
        ErrorUpdate,
        ErrorDelete,
        ErrorDeleteOrUpdate,
        ErrorInsertOrUpdate,
        ErrorInputParameter,
        ALLFieldsRequired,
        StarFieldsRequired,
        PassNotEqualToRepass,
        SuccessRegister,
        ErrorTypingData,
        ErrorValidInputData,
        ErrorTypingDataIsOutOfRanges,
        ErrorImageResizer,
        ErrorMailSender,
        ErrorGettingData,
        ErrorChoosePic,
        ErrorChooseDoc,
        ErrorChooseZip,
        ErrorInvalidAnimatedPicExt,
        ErrorInvalidPicExt,
        ErrorInPicLen,
        ErrorOutputIsEmpty,
        ErrorSearch,
        SuccessRefresh,
        ErrorRefresh,
        SuccessAccept,
        SuccessDecline,
        SuccessPending,
        ErrorSameUser,
        ErrorInputOneTBox,
        ErrorHaveChild,
        ErrorOldPassword,
        ErrorCantDelete,
        ErrorDeletePersonSubset,
        ErrorCantDeletePerson,
        ErrorDeleteOrActive,
        ErrorUsernameMistake,
        SuccessSetGalleryCover,
        ErrorLoadData
    };

}