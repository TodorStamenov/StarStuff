namespace StarStuff.Services.Moderator.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Models.Telescopes;
    using StarStuff.Data;
    using StarStuff.Data.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class TelescopeService : ITelescopeService
    {
        private readonly StarStuffDbContext db;

        public TelescopeService(StarStuffDbContext db)
        {
            this.db = db;
        }

        public int Total()
        {
            return this.db.Telescopes.Count();
        }

        public int Create(
            string name,
            string location,
            string description,
            double mirrorDiameter,
            string imageUrl)
        {
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

            if (telescope == null)
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