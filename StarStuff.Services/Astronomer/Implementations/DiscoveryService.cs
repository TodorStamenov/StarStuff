namespace StarStuff.Services.Astronomer.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Models.Astronomers;
    using Models.Discoveries;
    using StarStuff.Data;
    using StarStuff.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DiscoveryService : IDiscoveryService
    {
        private readonly StarStuffDbContext db;

        public DiscoveryService(StarStuffDbContext db)
        {
            this.db = db;
        }

        public int Total(bool? confirmed = null)
        {
            IQueryable<Discovery> discoveriesQuery =
                this.db.Discoveries.AsQueryable();

            if (confirmed != null)
            {
                discoveriesQuery = discoveriesQuery
                    .Where(d => d.IsConfirmed == confirmed.Value);
            }

            return discoveriesQuery.Count();
        }

        public bool IsPioneer(int discoveryId, int pioneerId)
        {
            return this.db
                .Find<Pioneers>(pioneerId, discoveryId) != null;
        }

        public bool IsObserver(int discoveryId, int observerId)
        {
            return this.db
                .Find<Observers>(observerId, discoveryId) != null;
        }

        public int Create(string starSystem, int telescopeId, int astronomerId)
        {
            if (!this.db.Telescopes.Any(t => t.Id == telescopeId)
                || this.db.Discoveries.Any(d => d.StarSystem == starSystem))
            {
                return -1;
            }

            Discovery discovery = new Discovery
            {
                StarSystem = starSystem,
                DateMade = DateTime.UtcNow.Date,
                TelescopeId = telescopeId
            };

            Pioneers pioneer = new Pioneers
            {
                PioneerId = astronomerId
            };

            discovery.Pioneers.Add(pioneer);

            this.db.Discoveries.Add(discovery);
            this.db.SaveChanges();

            return discovery.Id;
        }

        public bool Edit(int id, string starSystem)
        {
            Discovery discovery = this.db.Discoveries.Find(id);

            if (discovery == null
                || this.db.Discoveries.Any(d => d.StarSystem == starSystem))
            {
                return false;
            }

            discovery.StarSystem = starSystem;

            this.db.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            Discovery discovery = this.db.Discoveries.Find(id);

            if (discovery == null)
            {
                return false;
            }

            this.db.Discoveries.Remove(discovery);
            this.db.SaveChanges();

            return true;
        }

        public bool Confirm(int discoveryId, int userId)
        {
            var discoveryInfo = this.db
                .Discoveries
                .Where(d => d.Id == discoveryId)
                .Select(d => new
                {
                    d.IsConfirmed,
                    Observers = d.Observers
                        .Select(o => new
                        {
                            o.ObserverId
                        })
                })
                .FirstOrDefault();

            if (discoveryInfo == null
                || discoveryInfo.IsConfirmed
                || discoveryInfo.Observers
                    .Any(o => o.ObserverId == userId))
            {
                return false;
            }

            Observers observer = new Observers
            {
                ObserverId = userId,
                DiscoveryId = discoveryId
            };

            this.db.Add(observer);

            if (discoveryInfo.Observers.Count() == 2)
            {
                this.db
                    .Discoveries
                    .Find(discoveryId)
                    .IsConfirmed = true;
            }

            this.db.SaveChanges();

            return true;
        }

        public DiscoveryDetailsServiceModel Details(int id)
        {
            return this.db
                .Discoveries
                .Where(d => d.Id == id)
                .ProjectTo<DiscoveryDetailsServiceModel>()
                .FirstOrDefault();
        }

        public DiscoveryFormServiceModel GetForm(int id)
        {
            return this.db
                .Discoveries
                .Where(d => d.Id == id)
                .ProjectTo<DiscoveryFormServiceModel>()
                .FirstOrDefault();
        }

        public IEnumerable<AstronomerServiceModel> Pioneers(int discoveryId)
        {
            return this.db
                .Discoveries
                .Where(d => d.Id == discoveryId)
                .SelectMany(d => d.Pioneers.Select(p => p.Pioneer))
                .ProjectTo<AstronomerServiceModel>()
                .OrderBy(o => o.FirstName)
                .ThenBy(o => o.LastName)
                .ToList();
        }

        public IEnumerable<AstronomerServiceModel> Observers(int discoveryId)
        {
            return this.db
                .Discoveries
                .Where(d => d.Id == discoveryId)
                .SelectMany(d => d.Observers.Select(p => p.Observer))
                .ProjectTo<AstronomerServiceModel>()
                .OrderBy(o => o.FirstName)
                .ThenBy(o => o.LastName)
                .ToList();
        }

        public IEnumerable<DiscoveryServiceModel> DiscoveryDropdown(int journalId)
        {
            return this.db
                .Discoveries
                .Where(d => d.IsConfirmed)
                .Where(d => !d.Publications
                    .Any(p => p.JournalId == journalId))
                .OrderByDescending(d => d.DateMade)
                .ProjectTo<DiscoveryServiceModel>()
                .ToList();
        }

        public IEnumerable<ListDiscoveriesServiceModel> All(int page, int pageSize, bool? confirmed = null)
        {
            IQueryable<Discovery> discoveriesQuery =
                this.db.Discoveries.AsQueryable();

            if (confirmed != null)
            {
                discoveriesQuery = discoveriesQuery
                    .Where(d => d.IsConfirmed == confirmed.Value);
            }

            return discoveriesQuery
                .OrderByDescending(d => d.DateMade)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ListDiscoveriesServiceModel>()
                .ToList();
        }
    }
}