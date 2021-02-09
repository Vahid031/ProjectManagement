using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infrastracture.AppCode
{
    public static class ChangeValue
    {
        public static string ChangeValues(int num)
        {
            string text;
            switch (num)
            {
                case 1:
                    text = "بیمه اعتبارات تملک و داراییها";
                    break;
                case 2:
                    text = "بیمه دستمزدی با مصالح";
                    break;
                case 3:
                    text = "بیمه دستمزدی بدون مصالح";
                    break;
                default:
                    text = "تعریف نشده";
                    break;
            }
            return text;
        }
    }
}