namespace DisprzTraining.Extensions
{
    public class DateTimeExtension
    {
        public static DateTime GetEndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }
    }
}