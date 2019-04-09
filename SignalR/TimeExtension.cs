using System;
namespace SignalR
{
    public static class TimeExtension
    {
        public static string AddTimeStamp(this string row)
        {
            return $"{DateTime.Now.ToString("mm:ss.fff")}: ";
        }
    }
}
