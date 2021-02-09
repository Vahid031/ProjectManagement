using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.ReqularExpression
{
    public static class CustomRegex
    {
        public const string EMAIL = @"^[_A-Za-z0-9-\+]+(\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,4})$";

        public const string PERSIAN_DATE = @"(13|14)([0-9][0-9])\/(((0?[1-6])\/(([12][0-9])|(3[0-1])|(0?[1-9])))|(((0?[7-9])|(1[0-2]))\/(([12][0-9])|(30)|(0?[1-9]))))";

        public const string TIME5 = @"^[0-5][0-6]:[0-5][0-6]";

        public const string Empty = @"(?!انتخاب کنید...).+";
    }
}
