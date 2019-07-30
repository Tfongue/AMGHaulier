using System;
using System.Collections.Generic;
using System.Text;

namespace AMGHaulier.Common.Helpers
{
    public static class Utility
    {
        public static string DateTimeFormat => "yyyy MMM dd hh:mm";

        public static string ValidateSummary(string text)
        {
            if (string.IsNullOrEmpty(text)) return "Value is empty";
            if (text.Length > 255) return "Value exceeds 255 characters length";

            return string.Empty;
        }

        public static string ValidateLocation(string text)
        {
            if (string.IsNullOrEmpty(text)) return "Value is empty";
            if (text.Length > 255) return "Value exceeds 255 characters length";

            return string.Empty;
        }

        public static string ValidateStartAndEndDates(DateTime start, DateTime end)
        {
            if (start > end) return "Start Date-Time cannot be greather than End Date-Time";

            return string.Empty;
        }

    }

}
