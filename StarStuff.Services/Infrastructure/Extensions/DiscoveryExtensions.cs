namespace StarStuff.Services.Infrastructure.Extensions
{
    using Areas.Astronomer.Models.Astronomers;
    using Data.Models;
    using System.Linq;

    public static class DiscoveryExtensions
    {
        public static IQueryable<Discovery> Filter(this IQueryable<Discovery> discoveries, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return discoveries
                    .Where(d => d.StarSystem.ToLower().Contains(search.ToLower()));
            }

            return discoveries;
        }

        public static IQueryable<Discovery> Confirmed(this IQueryable<Discovery> discoveries, bool? confirmed)
        {
            if (confirmed != null)
            {
                return discoveries
                    .Where(d => d.IsConfirmed == confirmed.Value);
            }

            return discoveries;
        }

        public static IQueryable<Discovery> ByAstronomerType(this IQueryable<Discovery> discoveries, int astronomerId, AstronomerType astronomerType)
        {
            if (astronomerType == AstronomerType.All)
            {
                return discoveries
                    .Where(d => d.Pioneers.Any(p => p.PioneerId == astronomerId)
                        || d.Observers.Any(o => o.ObserverId == astronomerId));
            }
            else if (astronomerType == AstronomerType.Pioneer)
            {
                return discoveries
                    .Where(d => d.Pioneers.Any(p => p.PioneerId == astronomerId));
            }
            else if (astronomerType == AstronomerType.Observer)
            {
                return discoveries
                    .Where(d => d.Observers.Any(o => o.ObserverId == astronomerId));
            }

            return discoveries;
        }
    }
}