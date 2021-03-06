﻿namespace StarStuff.Services.Areas.Moderator.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Models.Journals;
    using System.Collections.Generic;
    using System.Linq;

    public class JournalService : IJournalService
    {
        private readonly StarStuffDbContext db;

        public JournalService(StarStuffDbContext db)
        {
            this.db = db;
        }

        public bool Exists(string name)
        {
            return this.db
                .Journals
                .Any(j => j.Name == name);
        }

        public int Total()
        {
            return this.db
                .Journals
                .Count();
        }

        public string GetName(int journalId)
        {
            return this.db
                .Journals
                .Where(j => j.Id == journalId)
                .Select(j => j.Name)
                .FirstOrDefault();
        }

        public int Create(string name, string description, string imageUrl)
        {
            if (this.Exists(name))
            {
                return -1;
            }

            Journal journal = new Journal
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl
            };

            this.db.Journals.Add(journal);
            this.db.SaveChanges();

            return journal.Id;
        }

        public bool Edit(int id, string name, string description, string imageUrl)
        {
            Journal journal = this.db.Journals.Find(id);

            if (journal == null
                || (journal.Name != name
                    && this.Exists(name)))
            {
                return false;
            }

            journal.Name = name;
            journal.Description = description;
            journal.ImageUrl = imageUrl;

            this.db.SaveChanges();

            return true;
        }

        public JournalDetailsServiceModel Details(int id)
        {
            return this.db
                .Journals
                .Where(j => j.Id == id)
                .ProjectTo<JournalDetailsServiceModel>()
                .FirstOrDefault();
        }

        public JournalFormServiceModel GetForm(int id)
        {
            return this.db
                .Journals
                .Where(j => j.Id == id)
                .ProjectTo<JournalFormServiceModel>()
                .FirstOrDefault();
        }

        public IEnumerable<ListJournalsServiceModel> All(int page, int pageSize)
        {
            return this.db
                .Journals
                .OrderBy(j => j.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ListJournalsServiceModel>()
                .ToList();
        }
    }
}