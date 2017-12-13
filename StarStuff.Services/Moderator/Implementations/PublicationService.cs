namespace StarStuff.Services.Moderator.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Models.Publications;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PublicationService : IPublicationService
    {
        private readonly StarStuffDbContext db;

        public PublicationService(StarStuffDbContext db)
        {
            this.db = db;
        }

        public int Total()
        {
            return this.db.Publications.Count();
        }

        public int TotalByJournal(int journalId)
        {
            return this.db.Publications.Count(p => p.JournalId == journalId);
        }

        public int TotalByTelescope(int telescopeId)
        {
            return this.db.Publications.Count(p => p.Discovery.TelescopeId == telescopeId);
        }

        public int Create(string content, int discoveryId, int journalId)
        {
            bool hasJournal = this.db
                .Journals
                .Any(j => j.Id == journalId);

            bool hasDiscovery = this.db
                .Discoveries
                .Any(d => d.Id == discoveryId
                    && d.IsConfirmed);

            if (!hasJournal || !hasDiscovery)
            {
                return -1;
            }

            bool hasPublication = this.db
                .Publications
                .Any(p => p.JournalId == journalId
                    && p.DiscoveryId == discoveryId);

            if (hasPublication)
            {
                return -1;
            }

            Publication publication = new Publication
            {
                Content = content,
                JournalId = journalId,
                DiscoveryId = discoveryId,
                ReleaseDate = DateTime.UtcNow.Date
            };

            this.db.Publications.Add(publication);
            this.db.SaveChanges();

            return publication.Id;
        }

        public bool Edit(int id, string content)
        {
            Publication publication = this.db
                .Publications
                .Find(id);

            if (publication == null)
            {
                return false;
            }

            publication.Content = content;

            this.db.SaveChanges();

            return true;
        }

        public PublicationFormServiceModel GetForm(int id)
        {
            return this.db
                .Publications
                .Where(p => p.Id == id)
                .ProjectTo<PublicationFormServiceModel>()
                .FirstOrDefault();
        }

        public PublicationDetailsServiceModel Details(int id)
        {
            return this.db
                .Publications
                .Where(p => p.Id == id)
                .ProjectTo<PublicationDetailsServiceModel>()
                .FirstOrDefault();
        }

        public IEnumerable<ListPublicationsServiceModel> All(int page, int pageSize)
        {
            return this.db
                .Publications
                .OrderByDescending(p => p.ReleaseDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ListPublicationsServiceModel>()
                .ToList();
        }

        public IEnumerable<ListPublicationsServiceModel> AllByJournal(int journalId, int page, int pageSize)
        {
            return this.db
               .Publications
               .Where(p => p.JournalId == journalId)
               .OrderByDescending(p => p.ReleaseDate)
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ProjectTo<ListPublicationsServiceModel>()
               .ToList();
        }

        public IEnumerable<ListPublicationsServiceModel> AllByTelescope(int telescopeId, int page, int pageSize)
        {
            return this.db
               .Publications
               .Where(p => p.Discovery.TelescopeId == telescopeId)
               .OrderByDescending(p => p.ReleaseDate)
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ProjectTo<ListPublicationsServiceModel>()
               .ToList();
        }
    }
}