using System.Net;
using System.Text;

namespace ExerciseTrackingAnalytics.Extensions
{
    public static class IDictionaryExtensions
    {
        public static string ToUrlQueryString(this Dictionary<string, string> dictionary)
        {
            var sb = new StringBuilder(128);
            sb.Append('?');

            foreach (var entry in dictionary)
            {
                sb.AppendFormat("{0}={1}&", entry.Key, WebUtility.UrlEncode(entry.Value));
            }

            return sb.ToString().TrimEnd('&');
        }
    }
}
