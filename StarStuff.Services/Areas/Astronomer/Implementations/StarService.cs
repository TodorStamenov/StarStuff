namespace StarStuff.Services.Areas.Astronomer.Implementations
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

        public bool Exists(string name)
        {
            return this.db.Stars.Any(s => s.Name == name);
        }

        public bool Create(int discoveryId, string name, int temperature)
        {
            var discoveryInfo = this.db
                .Discoveries
                .Where(d => d.Id == discoveryId)
                .Select(d => new
                {
                    StarsCount = d.Stars.Count
                })
                .FirstOrDefault();

            bool hasStar = this.db.Stars.Any(s => s.Name == name);

            if (discoveryInfo == null
                || hasStar
                || discoveryInfo.StarsCount >= DataConstants.DiscoveryConstants.MaxStarsPerDiscovery)
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

            if (star == null
                || (star.Name != name
                    && this.db.Stars.Any(s => s.Name == name)))
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

            var discoveryInfo = this.db
                .Discoveries
                .Where(d => d.Stars.Any(s => s.Id == id))
                .Select(d => new
                {
                    IsLastStar = d.Stars.Count == 1
                })
                .FirstOrDefault();

            if (discoveryInfo.IsLastStar)
            {
                return false;
            }

            this.db.Stars.Remove(star);
            this.db.SaveChanges();

            return true;
        }

        public string GetName(int id)
        {
            return this.db
                .Stars
                .Where(s => s.Id == id)
                .Select(s => s.Name)
                .FirstOrDefault();
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
                .OrderBy(p => p.Id)
                .ProjectTo<ListStarsServiceModel>()
                .ToList();
        }
    }
}