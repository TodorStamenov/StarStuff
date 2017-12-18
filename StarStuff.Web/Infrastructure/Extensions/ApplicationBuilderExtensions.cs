namespace StarStuff.Web.Infrastructure.Extensions
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        private const int AdminsCount = 1;
        private const int ModeratorsCount = 3;
        private const int AstronomersCount = 50;
        private const int UsersCount = 20;
        private const int TelescopesCount = 11;
        private const int JournalsCount = 11;
        private const int DiscoveriesCount = 200;

        private static readonly Random random = new Random();

        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<StarStuffDbContext>().Database.Migrate();
            }

            return app;
        }

        public static IApplicationBuilder UseSeedRoles(this IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                StarStuffDbContext db = serviceScope.ServiceProvider.GetService<StarStuffDbContext>();

                UserManager<User> userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                RoleManager<Role> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();

                Task.Run(async () =>
                {
                    await CreateRoleAsync(WebConstants.AdminRole, roleManager, db);
                    await CreateRoleAsync(WebConstants.ModeratorRole, roleManager, db);
                    await CreateRoleAsync(WebConstants.AstronomerRole, roleManager, db);

                    await SeedUsersAsync(WebConstants.AdminRole, AdminsCount, userManager, db);
                })
                .GetAwaiter()
                .GetResult();
            }

            return app;
        }

        public static IApplicationBuilder UseSeedDatabase(this IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                StarStuffDbContext db = serviceScope.ServiceProvider.GetService<StarStuffDbContext>();

                UserManager<User> userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                RoleManager<Role> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();

                Task.Run(async () =>
                {
                    await SeedAsync(db, userManager, roleManager);
                })
                .GetAwaiter()
                .GetResult();
            }

            return app;
        }

        private static async Task SeedAsync(StarStuffDbContext db, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            await SeedUsersAsync(UsersCount, userManager, db);

            await SeedUsersAsync(WebConstants.ModeratorRole, ModeratorsCount, userManager, db);
            await SeedUsersAsync(WebConstants.AstronomerRole, AstronomersCount, userManager, db);

            await SeedTelescopesAsync(db, TelescopesCount);
            await SeedJournalsAsync(db, JournalsCount);
            await SeedDiscoveriesAsync(db, DiscoveriesCount);
            await SeedPublicationsAsync(db);
            await SeedCommentsAsync(db);
        }

        private static async Task CreateRoleAsync(string roleName, RoleManager<Role> roleManager, StarStuffDbContext db)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                Role role = new Role
                {
                    Name = roleName
                };

                await roleManager.CreateAsync(role);
            }
        }

        private static async Task SeedUsersAsync(int usersCount, UserManager<User> userManager, StarStuffDbContext db)
        {
            if (await db.Users.AnyAsync(u => !u.Roles.Any()))
            {
                return;
            }

            for (int i = 1; i <= usersCount; i++)
            {
                string username = $"Username{i}";

                User user = await userManager.FindByNameAsync(username);

                if (user != null)
                {
                    continue;
                }

                bool sendApplication = random.Next(0, 2) == 1 ? true : false;

                user = new User
                {
                    UserName = $"{username}",
                    ProfileImage = WebConstants.DefaultImage
                };

                if (sendApplication)
                {
                    user.SendApplication = sendApplication;
                    user.FirstName = $"{username}FirstName";
                    user.LastName = $"{username}LastName";
                    user.Email = $"{username}@{username}.com";
                    user.PhoneNumber = $"+3598888888{(i < 10 ? i * 10 : i)}";
                    user.BirthDate = DateTime.UtcNow
                        .Date
                        .AddYears(-(i + 30))
                        .AddDays(-i);
                }

                await userManager.CreateAsync(user, "123");
            }
        }

        private static async Task SeedUsersAsync(string roleName, int count, UserManager<User> userManager, StarStuffDbContext db)
        {
            bool hasUsersWithRole = await db.Users
                .AnyAsync(u => u.Roles.Any(r => r.Role.Name == roleName));

            if (hasUsersWithRole)
            {
                return;
            }

            for (int i = 1; i <= count; i++)
            {
                string username = $"{roleName}{i}";

                User user = await userManager.FindByNameAsync(username);

                if (user != null)
                {
                    continue;
                }

                user = new User
                {
                    UserName = $"{username}",
                    ProfileImage = WebConstants.DefaultImage,
                    BirthDate = null
                };

                if (roleName == WebConstants.AstronomerRole)
                {
                    user.FirstName = $"{username}FirstName";
                    user.LastName = $"{username}LastName";
                    user.Email = $"{username}@{username}.com";
                    user.BirthDate = DateTime.UtcNow
                        .Date
                        .AddYears(-(i + 30))
                        .AddDays(-i);
                }

                await userManager.CreateAsync(user, "123");
                await userManager.AddToRoleAsync(user, roleName);
            }
        }

        private static async Task SeedTelescopesAsync(StarStuffDbContext db, int telescopesCount)
        {
            if (await db.Telescopes.AnyAsync())
            {
                return;
            }

            for (int i = 1; i <= telescopesCount; i++)
            {
                Telescope telescope = new Telescope
                {
                    Name = $"Telescope Name {i}",
                    Location = $"Telescope Location {i}",
                    MirrorDiameter = i * 1.1,
                    Description = WebConstants.Lorem,
                    ImageUrl = "http://www.astro.caltech.edu/palomar/about/telescopes/images/SOTTimeLapse.jpg"
                };

                await db.Telescopes.AddAsync(telescope);
            }

            await db.SaveChangesAsync();
        }

        private static async Task SeedJournalsAsync(StarStuffDbContext db, int journalsCount)
        {
            if (await db.Journals.AnyAsync())
            {
                return;
            }

            for (int i = 1; i <= journalsCount; i++)
            {
                Journal journal = new Journal
                {
                    Name = $"Journal Name {i}",
                    Description = WebConstants.Lorem,
                    ImageUrl = "https://cdn.magzter.com/1462885409/1463047998/images/thumb/390_thumb_1.jpg"
                };

                await db.Journals.AddAsync(journal);
            }

            await db.SaveChangesAsync();
        }

        private async static Task SeedDiscoveriesAsync(StarStuffDbContext db, int discoveriesCount)
        {
            if (await db.Discoveries.AnyAsync())
            {
                return;
            }

            List<int> telescopeIds = await db.Telescopes
                .Select(t => t.Id)
                .ToListAsync();

            List<int> astronomerIds = await db.Users
                .Where(u => u.Roles.Any(r => r.Role.Name == WebConstants.AstronomerRole))
                .Select(u => u.Id)
                .ToListAsync();

            for (int i = 1; i <= discoveriesCount; i++)
            {
                Discovery discovery = new Discovery
                {
                    StarSystem = $"StarSystem{CreateGuid()}",
                    DateMade = DateTime.UtcNow
                        .Date
                        .AddMonths(-i)
                        .AddDays(-i),
                    TelescopeId = telescopeIds[random.Next(0, telescopeIds.Count)]
                };

                int planetsCount = random.Next(0, 10);
                int starsCount = random.Next(1, 4);
                int discoverersCount = random.Next(1, 6);
                int observersCount = random.Next(0, 4);

                for (int j = 1; j <= planetsCount; j++)
                {
                    Planet planet = new Planet
                    {
                        Name = $"Planet{CreateGuid()}",
                        Mass = Math.Round((i + j) * Math.PI, 2)
                    };

                    discovery.Planets.Add(planet);
                }

                for (int j = 1; j <= starsCount; j++)
                {
                    Star star = new Star
                    {
                        Name = $"Star{CreateGuid()}",
                        Temperature = random.Next(
                            DataConstants.StarConstants.TemperatureMinValue,
                            DataConstants.StarConstants.TemperatureMaxValue)
                    };

                    discovery.Stars.Add(star);
                }

                for (int j = 0; j < discoverersCount; j++)
                {
                    int pioneerId = astronomerIds[random.Next(0, astronomerIds.Count)];

                    if (discovery.Pioneers.Any(d => d.PioneerId == pioneerId))
                    {
                        j--;
                        continue;
                    }

                    discovery.Pioneers.Add(new Pioneers
                    {
                        PioneerId = pioneerId
                    });
                }

                for (int j = 0; j < observersCount; j++)
                {
                    int observerId = astronomerIds[random.Next(0, astronomerIds.Count)];

                    if (discovery.Observers.Any(d => d.ObserverId == observerId)
                        || discovery.Pioneers.Any(d => d.PioneerId == observerId))
                    {
                        j--;
                        continue;
                    }

                    discovery.Observers.Add(new Observers
                    {
                        ObserverId = observerId
                    });
                }

                if (observersCount >= 3)
                {
                    discovery.IsConfirmed = true;
                }

                await db.Discoveries.AddAsync(discovery);
            }

            await db.SaveChangesAsync();
        }

        private async static Task SeedPublicationsAsync(StarStuffDbContext db)
        {
            if (await db.Publications.AnyAsync())
            {
                return;
            }

            var discoveries = await db.Discoveries
                .Where(d => d.IsConfirmed)
                .Select(d => new
                {
                    d.Id,
                    d.DateMade
                })
                .ToListAsync();

            List<int> journalIds = await db.Journals
                .Select(j => j.Id)
                .ToListAsync();

            List<int> moderatorIds = await db.Users
                .Where(u => u.Roles.Any(r => r.Role.Name == WebConstants.ModeratorRole))
                .Select(u => u.Id)
                .ToListAsync();

            foreach (var discovery in discoveries)
            {
                Publication publication = new Publication
                {
                    Content = WebConstants.Lorem,
                    DiscoveryId = discovery.Id,
                    JournalId = journalIds[random.Next(0, journalIds.Count)],
                    AuthorId = moderatorIds[random.Next(0, moderatorIds.Count)],
                    ReleaseDate = discovery.DateMade.AddMonths(5)
                };

                await db.Publications.AddAsync(publication);
            }

            await db.SaveChangesAsync();
        }

        private async static Task SeedCommentsAsync(StarStuffDbContext db)
        {
            if (await db.Comments.AnyAsync())
            {
                return;
            }

            var publications = await db.Publications
                .Select(p => new
                {
                    p.Id,
                    p.ReleaseDate
                })
                .ToListAsync();

            List<int> usersIds = await db.Users
                .Select(u => u.Id)
                .ToListAsync();

            foreach (var publication in publications)
            {
                int commentsCount = random.Next(10, 41);

                for (int i = 0; i < commentsCount; i++)
                {
                    Comment comment = new Comment
                    {
                        Content = WebConstants.CommentContent,
                        DateAdded = publication
                            .ReleaseDate
                            .AddDays(i)
                            .AddHours(i)
                            .AddMinutes(i),
                        PublicationId = publication.Id,
                        UserId = usersIds[random.Next(0, usersIds.Count)]
                    };

                    await db.Comments.AddAsync(comment);
                }
            }

            await db.SaveChangesAsync();
        }

        private static string CreateGuid()
        {
            return Guid.NewGuid()
                .ToString()
                .Replace("-", string.Empty)
                .Substring(0, 10)
                .ToUpper();
        }
    }
}