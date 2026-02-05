namespace SmartBudget.Utilities
{
    public static class DateTimeExtensions
    {
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            return date.GetFirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        public static bool IsInMonth(this DateTime date, int month, int year)
        {
            return date.Month == month && date.Year == year;
        }

        public static bool IsCurrentMonth(this DateTime date)
        {
            var now = DateTime.Now;
            return date.IsInMonth(now.Month, now.Year);
        }

        public static bool IsCurrentYear(this DateTime date)
        {
            return date.Year == DateTime.Now.Year;
        }
    }

    public static class DecimalExtensions
    {
        public static decimal ToTwoDecimalPlaces(this decimal value)
        {
            return Math.Round(value, 2);
        }

        public static string ToCurrencyString(this decimal value)
        {
            return value.ToString("C2");
        }
    }
}
