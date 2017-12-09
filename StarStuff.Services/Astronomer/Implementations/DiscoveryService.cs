namespace StarStuff.Services.Astronomer.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Infrastructure.Extensions;
    using Models.Astronomers;
    using Models.Discoveries;
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

        public bool Exists(string starSystem)
        {
            return this.db.Discoveries.Any(d => d.StarSystem == starSystem);
        }

        public int Total(bool? confirmed)
        {
            return this.db
                .Discoveries
                .Confirmed(confirmed)
                .Count();
        }

        public int Total(bool? confirmed, AstronomerType astronomerType, int astronomerId)
        {
            return this.db
                .Discoveries
                .Confirmed(confirmed)
                .ByAstronomerType(astronomerId, astronomerType)
                .Count();
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

        public int Create(string starSystem, int telescopeId, int astronomerId, IEnumerable<int> astronomerIds)
        {
            if (this.db.Discoveries.Any(d => d.StarSystem == starSystem)
                || !this.db.Telescopes.Any(t => t.Id == telescopeId))
            {
                return -1;
            }

            List<int> pioneerIds = new List<int>();

            if (astronomerIds != null)
            {
                pioneerIds.AddRange(astronomerIds);
            }

            pioneerIds.Add(astronomerId);

            Discovery discovery = new Discovery
            {
                StarSystem = starSystem,
                DateMade = DateTime.UtcNow.Date,
                TelescopeId = telescopeId
            };

            foreach (var pioneerId in pioneerIds.Distinct())
            {
                discovery.Pioneers.Add(new Pioneers
                {
                    PioneerId = pioneerId
                });
            }

            this.db.Discoveries.Add(discovery);
            this.db.SaveChanges();

            return discovery.Id;
        }

        public bool Edit(int id, string starSystem)
        {
            Discovery discovery = this.db.Discoveries.Find(id);

            if (discovery == null
                || (discovery.StarSystem != starSystem
                    && this.db.Discoveries.Any(d => d.StarSystem == starSystem)))
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

        public string GetName(int id)
        {
            return this.db
                .Discoveries
                .Where(d => d.Id == id)
                .Select(d => d.StarSystem)
                .FirstOrDefault();
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
            return this.db
                .Discoveries
                .OrderByDescending(d => d.DateMade)
                .Confirmed(confirmed)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ListDiscoveriesServiceModel>()
                .ToList();
        }

        public IEnumerable<ListDiscoveriesServiceModel> All(
            int page,
            int pageSize,
            int astronomerId,
            AstronomerType astronomerType,
            bool? confirmed)
        {
            return this.db
                .Discoveries
                .OrderByDescending(d => d.DateMade)
                .Confirmed(confirmed)
                .ByAstronomerType(astronomerId, astronomerType)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ListDiscoveriesServiceModel>()
                .ToList();
        }
    }
}