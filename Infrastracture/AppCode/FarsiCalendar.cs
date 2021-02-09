using System;

namespace Infrastracture.AppCode
{
    /// <summary>
    /// Product Name:     AutumnalCalender
    /// Version:          2.1.0.0
    /// "Develop by:      Ali Paiizi"
    /// Copyright ©  2010 Open source and Free
    /// Address:          http://www.apaiizi.blodfa.com
    /// Email:            apaiizi@yahoo.com
    /// </summary>
    public class FarsiCalendar
    {
        #region Property

        //===================================================================================================
        private int yearPersian;
        /// <summary>
        /// سال خورشیدی به عدد برای مثال: 1361
        /// </summary>
        public int YearPersian
        {
            get
            {
                return yearPersian;
            }
        }

        private int monthPersian;
        /// <summary>
        /// ماه خورشیدی به عدد برای مثال: 8
        /// </summary>
        public int MonthPersian
        {
            get
            {
                return monthPersian;
            }
        }

        private int dayPersian;
        /// <summary>
        /// روز خورشیدی به عدد برای مثال: 6
        /// </summary>
        public int DayPersian
        {
            get
            {
                return dayPersian;
            }
        }

        private string strMonthPersian;
        /// <summary>
        /// ماه خورشیدی به فارسی برای مثال: آبان
        /// </summary>
        public string StrMonthPersian
        {
            get
            {
                return strMonthPersian;
            }
        }

        private string strDayPersian;
        /// <summary>
        /// روز خورشیدی به فارسی برای مثال: پنجشنبه
        /// </summary>
        public string StrDayPersian
        {
            get
            {
                return strDayPersian;
            }
        }
        //===================================================================================================
        private int yearHijri;
        /// <summary>
        /// سال هجری به عدد برای مثال: 1403
        /// </summary>
        public int YearHijri
        {
            get
            {
                return yearHijri;
            }
        }

        private int monthHijri;
        /// <summary>
        /// ماه هجری به عدد برای مثال: 1
        /// </summary>
        public int MonthHijri
        {
            get
            {
                return monthHijri;
            }
        }

        private int dayHijri;
        /// <summary>
        /// روز هجری به عدد برای مثال: 11
        /// </summary>
        public int DayHijri
        {
            get
            {
                return dayHijri;
            }
        }

        private string strMonthHijri;
        /// <summary>
        /// ماه هجری به فارسی برای مثال: محرم
        /// </summary>
        public string StrMonthHijri
        {
            get
            {
                return strMonthHijri;
            }
        }
        //===================================================================================================
        private int yearEnglish;
        /// <summary>
        /// سال میلادی به عدد برای مثال: 1982
        /// </summary>
        public int YearEnglish
        {
            get
            {
                return yearEnglish;
            }
        }

        private int monthEnglish;
        /// <summary>
        /// ماه میلادی به عدد برای مثال: 10
        /// </summary>
        public int MonthEnglish
        {
            get
            {
                return monthEnglish;
            }
        }

        private int dayEnglish;
        /// <summary>
        /// روز میلادی به عدد برای مثال: 28
        /// </summary>
        public int DayEnglish
        {
            get
            {
                return dayEnglish;
            }
        }

        private string strMonthEnglish;
        /// <summary>
        /// ماه میلادی به انگلیسی برای مثال: October
        /// </summary>
        public string StrMonthEnglish
        {
            get
            {
                return strMonthEnglish;
            }
        }

        private string strTodayEnglish;
        /// <summary>
        /// روز میلادی به انگلیسی برای مثال: Thursday
        /// </summary>
        public string StrTodayEnglish
        {
            get
            {
                return strTodayEnglish;
            }
        }
        //===================================================================================================        
        private Boolean isHoliday = false;
        /// <summary>
        /// با فراخوانی هریک از توابع 
        ///  ConvertDateToHijri یا  ConvertDateToPersian 
        ///این متغیر مقدار میگیرد.
        ///در صورت درست بودن مقدار این متغیر یعنی آن روز ، یکی از روزهای تعطیل رسمی کشور است.
        /// </summary>
        public Boolean IsHoliday
        {
            get
            {
                return isHoliday;
            }
            set { isHoliday = value; }
        }
        //===================================================================================================
        private int startDayInMonth;
        /// <summary>
        /// اولین روز شروع هر ماه را نشان میدهد. اگر مقدار این متغیر برابر 1 بود یعنی اولین روز ماه جاری شنبه است: 
        /// شنبه 1   ، یکشنبه 2   ، دوشنبه 3   ، سه شنبه 4   ، چهارشنبه 5   ، پنجشنبه 6   ، جمعه 7
        /// </summary>
        public int StartDayInMonth
        {
            get
            {
                return startDayInMonth;
            }
        }
        //===================================================================================================
        private string commentDay = null;
        /// <summary>
        /// توضیحات ارائه شده برای بعضی از روزها 
        /// برای مثال: میلاد حضرت امام علی علیه السلام
        /// </summary>
        public string CommentDay
        {
            get
            {
                return commentDay;
            }
            set { commentDay = value; }
        }
        //===================================================================================================
        #endregion

        #region Private Function
        //===================================================================================================
        private bool IsHolidayPa()
        {
            if (IsHoliday == true)
                return true;

            bool key = false;

            //HolidayPa==========================
            if (((monthPersian == 1) && (dayPersian < 5)) || ((monthPersian == 1) && (dayPersian > 11) && (dayPersian < 14)))
                key = true;
            else if (((monthPersian == 3) && (dayPersian > 13) && (dayPersian < 16)))
                key = true;
            else if ((monthPersian == 11) && (dayPersian == 22))
                key = true;
            else if ((monthPersian == 12) && (dayPersian == 29))
                key = true;
            //===================================
            return key;
        }
        //===================================================================================================
        private bool IsHolidayHi()
        {
            if (isHoliday == true)
                return true;

            bool key = false;

            //HolidayHi==========================
            if (((monthHijri == 1) && (dayHijri == 9)) || ((monthHijri == 1) && (dayHijri == 10)))
                key = true;
            else if (((monthHijri == 2) && (dayHijri == 20)) || ((monthHijri == 2) && (dayHijri == 28)) || ((monthHijri == 2) && (dayHijri == 30)))
                key = true;
            else if ((monthHijri == 3) && (dayHijri == 17))
                key = true;
            else if ((monthHijri == 6) && (dayHijri == 3))
                key = true;
            else if (((monthHijri == 7) && (dayHijri == 13)) || ((monthHijri == 7) && (dayHijri == 27)))
                key = true;
            else if ((monthHijri == 8) && (dayHijri == 15))
                key = true;
            else if ((monthHijri == 9) && (dayHijri == 21))
                key = true;
            else if (((monthHijri == 10) && (dayHijri == 1)) || ((monthHijri == 10) && (dayHijri == 25)))
                key = true;
            else if (((monthHijri == 12) && (dayHijri == 10)) || ((monthHijri == 12) && (dayHijri == 18)))
                key = true;
            //===================================
            return key;
        }
        //===================================================================================================
        private void SearchStrDayPa()
        {
            #region Text Day For Persian
            switch (monthPersian)
            {
                case 1:
                    switch (dayPersian)
                    {
                        case 1:
                            commentDay = "آغاز نوروز (تعطیل)";
                            break;
                        case 2:
                            commentDay = "عید نوروز (تعطیل)";
                            break;
                        case 3:
                            commentDay = "عید نوروز (تعطیل)";
                            break;
                        case 4:
                            commentDay = "عید نوروز (تعطیل)";
                            break;
                        case 12:
                            commentDay = "روز جمهوری اسلامی (تعطیل)";
                            break;
                        case 13:
                            commentDay = "روز طبیعت (تعطیل)";
                            break;
                        case 18:
                            commentDay = "روز سلامتی (روز جهانی بهداشت)";
                            break;
                    }
                    break;
                case 2:
                    switch (dayPersian)
                    {
                        case 1:
                            commentDay = "روز بزرگداشت سعدی";
                            break;
                        case 10:
                            commentDay = "روز ملی خلیج فارس";
                            break;
                        case 11:
                            commentDay = "روز جهانی کار و کارگر";
                            break;
                        case 12:
                            commentDay = "شهادت استاد مرتضی مطهری – روز معلم";
                            break;
                        case 15:
                            commentDay = "روز جهانی ماما";
                            break;
                        case 18:
                            commentDay = "روز جهانی صلیب سرخ و هلال احمر";
                            break;
                        case 25:
                            commentDay = "روز بزرگداشت فردوسی";
                            break;
                        case 28:
                            commentDay = "رو جهانی موزه و میراث فرهنگی";
                            break;
                    }
                    break;
                case 3:
                    switch (dayPersian)
                    {
                        case 1:
                            commentDay = "روز بزرگداشت ملاصدرا (صدرالمتالهین)";
                            break;
                        case 10:
                            commentDay = "روز جهانی بدون دخانیات";
                            break;
                        case 14:
                            commentDay = "رحلت حضرت امام خمینی (ره) (تعطیل)";
                            break;
                        case 15:
                            commentDay = "قیام خونین 15 خرداد (تعطیل) – روز جهانی محیط زیست";
                            break;
                        case 20:
                            commentDay = "روز جهانی صنایع دستی";
                            break;
                        case 27:
                            commentDay = "روز جهانی بیابان زدایی";
                            break;
                        case 29:
                            commentDay = "شهادت  یا درگذشت دکتر علی شریعتی";
                            break;
                    }
                    break;
                case 4:
                    switch (dayPersian)
                    {
                        case 5:
                            commentDay = "روز جهانی مبارزه با مواد مخدر";
                            break;
                    }
                    break;
                case 5:
                    switch (dayPersian)
                    {
                        case 8:
                            commentDay = "روز بزرگداشت شیخ شهاب الدین شهروردی (شیخ اشراق) (متولد زنجان)";
                            break;
                        case 10:
                            commentDay = "روز جهانی شیر مادر";
                            break;
                        case 30:
                            commentDay = "روز جهانی مسجد";
                            break;
                    }
                    break;
                case 7:
                    switch (dayPersian)
                    {
                        case 8:
                            commentDay = "روز بزرگداشت مولوی – روز جهانی ناشنوایان – روز جهانی دریانوردی";
                            break;
                        case 16:
                            commentDay = "روز جهانی کودک";
                            break;
                        case 17:
                            commentDay = "روز جهانی پست";
                            break;
                        case 20:
                            commentDay = "روز بزرگداشت حافظ";
                            break;
                        case 22:
                            commentDay = "روز جهانی استاندارد";
                            break;
                        case 23:
                            commentDay = "روز جهانی نابینایان (عصای سفید)";
                            break;
                        case 24:
                            commentDay = "روز جهانی غذا";
                            break;
                    }
                    break;
                case 9:
                    switch (dayPersian)
                    {
                        case 16:
                            commentDay = "روز دانشجو";
                            break;
                        case 30:
                            commentDay = "شب یلدا";
                            break;
                    }
                    break;
                case 10:
                    switch (dayPersian)
                    {
                        case 11:
                            commentDay = "آغاز سال میلادی";
                            break;
                    }
                    break;
                case 11:
                    switch (dayPersian)
                    {
                        case 22:
                            commentDay = "پیروزی انقلاب اسلامی (تعطیل)";
                            break;
                    }
                    break;
                case 12:
                    switch (dayPersian)
                    {
                        case 5:
                            commentDay = "روز مهندسی";
                            break;
                        case 15:
                            commentDay = "روز درختکاری";
                            break;
                        case 29:
                            commentDay = "روز ملی شدن صنعت نفت ایران (تعطیل)";
                            break;
                    }
                    break;
            }
            #endregion
        }
        //===================================================================================================
        private void SearchStrDayHi()
        {
            #region Text Day For Hijri
            switch (monthHijri)
            {
                case 1://"محرم"
                    switch (dayHijri)
                    {
                        case 1:
                            commentDay = "آغاز سال هجری قمری";
                            break;
                        case 9:
                            commentDay = "تاسوعای حسینی (تعطیل)";
                            break;
                        case 10:
                            commentDay = "عاشورای حسینی (تعطیل)";
                            break;
                        case 12:
                            commentDay = "شهادت حضرت امام زین العابدین (ع)";
                            break;
                        case 25:
                            commentDay = "شهادت حضرت امام زین العابدین (ع) به روایتی";
                            break;
                    }
                    break;
                case 2://"صفر"
                    switch (dayHijri)
                    {
                        case 3:
                            commentDay = "ولادت حضرت امام محمد باقر (ع)";
                            break;
                        case 7:
                            commentDay = "ولادت حضرت امام موسی کاظم (ع)";
                            break;
                        case 20:
                            commentDay = "اربعین حسینی (ع)";
                            break;
                        case 28:
                            commentDay = "رحلت حضرت رسول اکرم (ص) – شهادت امام حضرت امام حسن (ع) (تعطیل)";
                            break;
                        case 29:
                            commentDay = "شهادت حضرت امام رضا (ع) (تعطیل)";
                            break;
                    }
                    break;
                case 3://"ربيع الاول"
                    switch (dayHijri)
                    {
                        case 8:
                            commentDay = "شهادت حضرت امام حسن عسکری (ع)";
                            break;
                        case 12:
                            commentDay = "میلاد حضرت رسول اکرم (ص) به روایت اهل سنت";
                            break;
                        case 17:
                            commentDay = "میلاد حضرت رسول اکرم (ص) – میلاد حضرت امام جعفر صادق (ع) (تعطیل)";
                            break;
                    }
                    break;
                case 4://ربيع الثاني
                    switch (dayHijri)
                    {
                        case 8:
                            commentDay = "ولادت حضرت امام حسن عسگری (ع)";
                            break;
                        case 10:
                            commentDay = "وفات حضرت معصومه (س)";
                            break;
                    }
                    break;
                case 5://"جمادي الاولي"
                    switch (dayHijri)
                    {
                        case 5:
                            commentDay = "ولادت حضرت زینب (س)";
                            break;
                        case 13:
                            commentDay = "شهادت حضرت فاطمه زهرا (س) به روایتی";
                            break;
                    }
                    break;
                case 6://"جمادي الثانيه"
                    switch (dayHijri)
                    {
                        case 4:
                            commentDay = "شهادت حضرت فاطمه زهرا (س) (تعطیل)";
                            break;
                        case 20:
                            commentDay = "ولادت حضرت فاطمه زهرا (س) – روز زن";
                            break;
                    }
                    break;
                case 7://"رجب"
                    switch (dayHijri)
                    {
                        case 1:
                            commentDay = "ولادت حضرت امام محمد باقر (ع)";
                            break;
                        case 3:
                            commentDay = "شهادت حضرت امام علی النقی الهادی (ع)";
                            break;
                        case 10:
                            commentDay = "ولادت حضرت امام محمد تقی (ع)";
                            break;
                        case 13:
                            commentDay = "ولادت حضرت امام علی (ع) (تعطیل) (آغاز اعتکاف)";
                            break;
                        case 15:
                            commentDay = "وفات حضرت زینب (س)";
                            break;
                        case 25:
                            commentDay = "شهادت حضرت امام موسی کاظم (ع)";
                            break;
                        case 27:
                            commentDay = "مبعث حضرت پیامبر (ص) (تعطیل)";
                            break;
                    }
                    break;
                case 8://"شعبان"
                    switch (dayHijri)
                    {
                        case 3:
                            commentDay = "ولادت حضرت امام حسن (ع)";
                            break;
                        case 4:
                            commentDay = "ولادت حضرت ابوالفضل العباس (ع)";
                            break;
                        case 5:
                            commentDay = "ولادت حضرت امام سجاد (ع)";
                            break;
                        case 11:
                            commentDay = "ولادت حضرت علی اکبر (ع)";
                            break;
                        case 15:
                            commentDay = "ولادت حضرت قائم عجل الله تعالی فرجه (تعطیل)";
                            break;
                    }
                    break;
                case 9://"رمضان"
                    switch (dayHijri)
                    {
                        case 10:
                            commentDay = "وفات حضرت خدیجه (س)";
                            break;
                        case 15:
                            commentDay = "ولادت حضرت امام حسن مجتبی (ع)";
                            break;
                        case 18:
                            commentDay = "شب قدر";
                            break;
                        case 19:
                            commentDay = "ضربت خوردن حضرت امام علی (ع)";
                            break;
                        case 20:
                            commentDay = "شب قدر";
                            break;
                        case 21:
                            commentDay = "شهادت حضرت امام علی (ع) (تعطیل)";
                            break;
                        case 22:
                            commentDay = "شب قدر";
                            break;
                    }
                    break;
                case 10://"شوال"
                    switch (dayHijri)
                    {
                        case 1:
                        case 2:
                            commentDay = "عید سعید فطر (تعطیل)";
                            break;
                        case 25:
                                commentDay = "شهادت حضرت امام جعفر صادق (ع) (تعطیل)";
                            break;
                    }
                    break;
                case 11://"ذي القعده"
                    switch (dayHijri)
                    {
                        case 1:
                            commentDay = "ولادت حضرت معصومه (س) – روز دختر";
                            break;
                        case 11:
                            commentDay = "ولادت حضرت امام رضا (ع)";
                            break;
                        case 30:
                            commentDay = "شهادت حضرت امام محمد تقی (ع)";
                            break;
                    }
                    break;
                case 12://"ذي الحجه"
                    switch (dayHijri)
                    {
                        case 1:
                            commentDay = "سالروز ازدواج حضرت علی (ع) و حضرت فاطمه (س) – روز ازدواج و خانواده";
                            break;
                        case 7:
                            commentDay = "شهادت حضرت امام محمد باقر (ع)";
                            break;
                        case 9:
                            commentDay = "روز عرفه";
                            break;
                        case 10:
                            commentDay = "عید سعید قربان (تعطیل)";
                            break;
                        case 15:
                            commentDay = "ولادت حضرت امام علی النقی الهادی (ع)";
                            break;
                        case 18:
                            commentDay = "عید سعید غدیر خم (تعطیل)";
                            break;
                    }
                    break;
            }
            #endregion
        }
        //===================================================================================================
        #endregion
        //===================================================================================================

        #region Public Function
        //===================================================================================================
        /// <summary>
        /// تابع مبدل تقویم میلادی سیستم به خورشیدی 
        /// با فراخوانی این تابع تبدیل های لازم انجام گرفته و خصوصیات این کلاس مقدار دهی میشود.
        /// </summary>
        public void ConvertDateToPersian(DateTime dt)
        {
            int inti = 0;
            System.Globalization.PersianCalendar persian = new System.Globalization.PersianCalendar();
            DateTime Pa = dt;
            yearPersian = persian.GetYear(Pa);
            monthPersian = persian.GetMonth(Pa);
            dayPersian = persian.GetDayOfMonth(Pa);

            //===================================Calendar En
            strTodayEnglish = System.Convert.ToString(DateTime.Now.DayOfWeek.ToString());
            monthEnglish = Convert.ToInt32(dt.Month);
            dayEnglish = dt.Day;
            yearEnglish = dt.Year;
            if (monthEnglish == 1)
                strMonthEnglish = "January";
            else if (monthEnglish == 2)
                strMonthEnglish = "February";
            else if (monthEnglish == 3)
                strMonthEnglish = "March";
            else if (monthEnglish == 4)
                strMonthEnglish = "April";
            else if (monthEnglish == 5)
                strMonthEnglish = "May";
            else if (monthEnglish == 6)
                strMonthEnglish = "June";
            else if (monthEnglish == 7)
                strMonthEnglish = "July";
            else if (monthEnglish == 8)
                strMonthEnglish = "August";
            else if (monthEnglish == 9)
                strMonthEnglish = "September";
            else if (monthEnglish == 10)
                strMonthEnglish = "October";
            else if (monthEnglish == 11)
                strMonthEnglish = "November";
            else if (monthEnglish == 12)
                strMonthEnglish = "December";

            //StrDay=============================
            if (strTodayEnglish == "Saturday")
            {
                strDayPersian = "شنبه";
                inti = 0;
            }
            else if (strTodayEnglish == "Sunday")
            {
                strDayPersian = "یکشنبه";
                inti = 1;
            }
            else if (strTodayEnglish == "Monday")
            {
                strDayPersian = "دوشنبه";
                inti = 2;
            }
            else if (strTodayEnglish == "Tuesday")
            {
                strDayPersian = "سه شنبه";
                inti = 3;
            }
            else if (strTodayEnglish == "Wednesday")
            {
                strDayPersian = "چهارشنبه";
                inti = 4;
            }
            else if (strTodayEnglish == "Thursday")
            {
                strDayPersian = "پنجشنبه";
                inti = 5;
            }
            else if (strTodayEnglish == "Friday")
            {
                strDayPersian = "جمعه";
                //isHoliday = true;
                inti = 6;
            }
            
            //StrMonthPersian====================
            if (monthPersian == 1)
                strMonthPersian = "فروردين";
            else if (monthPersian == 2)
                strMonthPersian = "ارديبهشت";
            else if (monthPersian == 3)
                strMonthPersian = "خرداد";
            else if (monthPersian == 4)
                strMonthPersian = "تير";
            else if (monthPersian == 5)
                strMonthPersian = "مرداد";
            else if (monthPersian == 6)
                strMonthPersian = "شهريور";
            else if (monthPersian == 7)
                strMonthPersian = "مهر";
            else if (monthPersian == 8)
                strMonthPersian = "آبان";
            else if (monthPersian == 9)
                strMonthPersian = "آذر";
            else if (monthPersian == 10)
                strMonthPersian = "دي";
            else if (monthPersian == 11)
                strMonthPersian = "بهمن";
            else if (monthPersian == 12)
                strMonthPersian = "اسفند";

            //===================================
            startDayInMonth = dayPersian;
            --startDayInMonth;
            startDayInMonth %= 7;
            inti -= startDayInMonth;
            if (inti < 0)
                inti += 7;
            startDayInMonth = inti;
            isHoliday = IsHolidayPa();
            SearchStrDayPa();
        }
        //===================================================================================================
        /// <summary>
        /// تابع مبدل تقویم میلادی سیستم به هجری 
        /// با فراخوانی این تابع تبدیل های لازم انجام گرفته و خصوصیات این کلاس مقدار دهی میشود.
        /// </summary>
        public void ConvertDateToHijri(DateTime dt)
        {
            System.Globalization.HijriCalendar Hijri = new System.Globalization.HijriCalendar();
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();
            DateTime Hi = dt;
            //
            yearPersian = p.GetYear(dt);
            monthPersian = p.GetMonth(dt);
            dayPersian = p.GetDayOfMonth(dt);
            //
            yearHijri = Hijri.GetYear(Hi);
            monthHijri = Hijri.GetMonth(Hi);
            dayHijri = Hijri.GetDayOfMonth(Hi);

            //StrMonthHijri======================
            if (monthHijri == 1)
                strMonthHijri = "محرم";
            else if (monthHijri == 2)
                strMonthHijri = "صفر";
            else if (monthHijri == 3)
                strMonthHijri = "ربيع الاول";
            else if (monthHijri == 4)
                strMonthHijri = "ربيع الثاني";
            else if (monthHijri == 5)
                strMonthHijri = "جمادي الاولي";
            else if (monthHijri == 6)
                strMonthHijri = "جمادي الثانيه";
            else if (monthHijri == 7)
                strMonthHijri = "رجب";
            else if (monthHijri == 8)
                strMonthHijri = "شعبان";
            else if (monthHijri == 9)
                strMonthHijri = "رمضان";
            else if (monthHijri == 10)
                strMonthHijri = "شوال";
            else if (monthHijri == 11)
                strMonthHijri = "ذي القعده";
            else if (monthHijri == 12)
                strMonthHijri = "ذي الحجه";
            isHoliday = IsHolidayHi();
            SearchStrDayHi();
        }
        //===================================================================================================
        #endregion
        //===================================================================================================
    }
}

