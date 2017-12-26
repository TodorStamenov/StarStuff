namespace StarStuff.Services.Areas.Moderator.Implementations
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

        public bool Exists(int journalId, int discoveryId)
        {
            return this.db
                .Publications
                .Any(p => p.JournalId == journalId
                    && p.DiscoveryId == discoveryId);
        }

        public bool TitleExists(string title)
        {
            return this.db.Publications.Any(p => p.Title == title);
        }

        public string GetTitle(int id)
        {
            return this.db
                .Publications
                .Where(p => p.Id == id)
                .Select(p => p.Title)
                .FirstOrDefault();
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

        public int Create(string title, string content, int discoveryId, int journalId, int authorId)
        {
            bool hasJournal = this.db
                .Journals
                .Any(j => j.Id == journalId);

            bool hasDiscovery = this.db
                .Discoveries
                .Any(d => d.Id == discoveryId
                    && d.IsConfirmed);

            bool hasModerator = this.db
                .Users
                .Any(u => u.Id == authorId);

            if (!hasJournal || !hasDiscovery || !hasModerator)
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
                Title = title,
                Content = content,
                JournalId = journalId,
                DiscoveryId = discoveryId,
                AuthorId = authorId,
                ReleaseDate = DateTime.UtcNow.Date
            };

            this.db.Publications.Add(publication);
            this.db.SaveChanges();

            return publication.Id;
        }

        public bool Edit(int id, string title, string content)
        {
            Publication publication = this.db
                .Publications
                .Find(id);

            if (publication == null)
            {
                return false;
            }

            publication.Title = title;
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
            Publication publication = this.db.Publications.Find(id);

            if (publication == null)
            {
                return null;
            }

            publication.Views++;
            this.db.SaveChanges();

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