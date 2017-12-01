namespace StarStuff.Web.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string Slice(this string text, int length)
        {
            if (text.Length > length)
            {
                return text.Substring(0, length) + "...";
            }

            return text;
        }
    }
}