namespace StarStuff.Services.Infrastructure.Extensions
{
    using Data.Models;
    using System.Linq;

    public static class LogsExtensions
    {
        public static IQueryable<Log> Filter(this IQueryable<Log> logs, string search)
        {
            if (!string.IsNullOrEmpty(search)
                && !string.IsNullOrWhiteSpace(search))
            {
                return logs
                    .Where(l => l.Username.ToLower().Contains(search));
            }

            return logs;
        }
    }
}