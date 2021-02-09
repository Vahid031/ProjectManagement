using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.AppCode
{
    public static class ConvertDate
    {
        public static string GetShamsi(DateTime? dto = null)
        {
            if (dto == null)
                return "";

            DateTime dt = Convert.ToDateTime(dto);

            System.Globalization.PersianCalendar pcl = new System.Globalization.PersianCalendar();
            return string.Format("{0:d4}/{1:d2}/{2:d2}", pcl.GetYear(dt), pcl.GetMonth(dt), pcl.GetDayOfMonth(dt));
        }

        public static string GetShamsiTime(DateTime? dto = null)
        {
            if (dto == null)
                return "";

            DateTime dt = Convert.ToDateTime(dto);

            System.Globalization.PersianCalendar pcl = new System.Globalization.PersianCalendar();
            return string.Format("{0:d4}/{1:d2}/{2:d2} {3:d2}:{4:d2}", pcl.GetYear(dt), pcl.GetMonth(dt), pcl.GetDayOfMonth(dt), dt.Hour, dt.Minute);
        }


        public static DateTime? GetMiladi(string dt)
        {
            if (string.IsNullOrEmpty(dt))
                return null;

            PersianCalendar pcl = new PersianCalendar();
            string[] dts = dt.Split('/');

            return pcl.ToDateTime(Convert.ToInt32(dts[0]), Convert.ToInt32(dts[1]), Convert.ToInt32(dts[2]), 0, 0, 0, 0);
        }


        public static DateTime GetMiladiNowTime(string dt)
        {
            System.Globalization.PersianCalendar pcl = new System.Globalization.PersianCalendar();
            DateTime dtt = DateTime.Now;
            string[] dts = dt.Split('/');

            return pcl.ToDateTime(Convert.ToInt32(dts[0]), Convert.ToInt32(dts[1]), Convert.ToInt32(dts[2]), dtt.Hour, dtt.Minute, dtt.Second, 0);
        }
    }
}
