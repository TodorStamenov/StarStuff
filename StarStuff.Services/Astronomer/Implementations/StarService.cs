namespace StarStuff.Services.Astronomer.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Models.Stars;
    using System.Collections.Generic;
    using System.Linq;

    public class StarService : IStarService
    {
        private readonly StarStuffDbContext db;

        public StarService(StarStuffDbContext db)
        {
            this.db = db;
        }

        public bool Create(int discoveryId, string name, int temperature)
        {
            if (!this.db.Discoveries.Any(d => d.Id == discoveryId))
            {
                return false;
            }

            Star star = new Star
            {
                DiscoveryId = discoveryId,
                Name = name,
                Temperature = temperature
            };

            this.db.Stars.Add(star);
            this.db.SaveChanges();

            return true;
        }

        public bool Edit(int id, string name, int temperature)
        {
            Star star = this.db.Stars.Find(id);

            if (star == null)
            {
                return false;
            }

            star.Name = name;
            star.Temperature = temperature;

            this.db.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            Star star = this.db.Stars.Find(id);

            if (star == null)
            {
                return false;
            }

            this.db.Stars.Remove(star);
            this.db.SaveChanges();

            return true;
        }

        public StarFormServiceModel GetForm(int id)
        {
            return this.db
                .Stars
                .Where(s => s.Id == id)
                .ProjectTo<StarFormServiceModel>()
                .FirstOrDefault();
        }

        public IEnumerable<ListStarsServiceModel> Stars(int discoveryId)
        {
            return this.db
                .Stars
                .Where(s => s.DiscoveryId == discoveryId)
                .ProjectTo<ListStarsServiceModel>()
                .ToList();
        }
    }
}