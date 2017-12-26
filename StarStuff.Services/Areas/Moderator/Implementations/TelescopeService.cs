namespace StarStuff.Services.Areas.Moderator.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Models.Telescopes;
    using System.Collections.Generic;
    using System.Linq;

    public class TelescopeService : ITelescopeService
    {
        private readonly StarStuffDbContext db;

        public TelescopeService(StarStuffDbContext db)
        {
            this.db = db;
        }

        public bool Exists(string name)
        {
            return this.db.Telescopes.Any(t => t.Name == name);
        }

        public int Total()
        {
            return this.db.Telescopes.Count();
        }

        public string GetName(int telescopeId)
        {
            return this.db
                .Telescopes
                .Where(t => t.Id == telescopeId)
                .Select(t => t.Name)
                .FirstOrDefault();
        }

        public int Create(
            string name,
            string location,
            string description,
            double mirrorDiameter,
            string imageUrl)
        {
            if (this.db.Telescopes.Any(t => t.Name == name))
            {
                return -1;
            }

            Telescope telescope = new Telescope
            {
                Name = name,
                Location = location,
                Description = description,
                MirrorDiameter = mirrorDiameter,
                ImageUrl = imageUrl
            };

            this.db.Telescopes.Add(telescope);
            this.db.SaveChanges();

            return telescope.Id;
        }

        public bool Edit(
            int id,
            string name,
            string location,
            string description,
            double mirrorDiameter,
            string imageUrl)
        {
            Telescope telescope = this.db.Telescopes.Find(id);

            if (telescope == null
                || (telescope.Name != name
                    && this.db.Telescopes.Any(t => t.Name == name)))
            {
                return false;
            }

            telescope.Name = name;
            telescope.Location = location;
            telescope.Description = description;
            telescope.MirrorDiameter = mirrorDiameter;
            telescope.ImageUrl = imageUrl;

            this.db.SaveChanges();

            return true;
        }

        public TelescopeDetailsServiceModel Details(int id)
        {
            return this.db
                .Telescopes
                .Where(t => t.Id == id)
                .ProjectTo<TelescopeDetailsServiceModel>()
                .FirstOrDefault();
        }

        public TelescopeFormServiceModel GetForm(int id)
        {
            return this.db
                .Telescopes
                .Where(t => t.Id == id)
                .ProjectTo<TelescopeFormServiceModel>()
                .FirstOrDefault();
        }

        public IEnumerable<TelescopeServiceModel> TelescopeDropdown()
        {
            return this.db
                .Telescopes
                .OrderBy(t => t.Id)
                .ProjectTo<TelescopeServiceModel>()
                .ToList();
        }

        public IEnumerable<ListTelescopesServiceModel> All(int page, int pageSize)
        {
            return this.db
                .Telescopes
                .OrderBy(t => t.MirrorDiameter)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ListTelescopesServiceModel>()
                .ToList();
        }
    }
}