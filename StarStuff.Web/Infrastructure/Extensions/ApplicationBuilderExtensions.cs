namespace StarStuff.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using StarStuff.Data;
    using StarStuff.Data.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        private const int AdminsCount = 1;
        private const int ModeratorsCount = 3;
        private const int AstronomersCount = 20;
        private const int UsersCount = 20;
        private const int TelescopesCount = 20;
        private const int JournalsCount = 20;

        private static readonly Random random = new Random();

        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<StarStuffDbContext>().Database.Migrate();
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
            await CreateRoleAsync(WebConstants.AdminRole, roleManager, db);
            await CreateRoleAsync(WebConstants.ModeratorRole, roleManager, db);
            await CreateRoleAsync(WebConstants.AstronomerRole, roleManager, db);

            await SeedUsersAsync(UsersCount, userManager, db);

            await SeedUsersAsync(WebConstants.AdminRole, AdminsCount, userManager, db);
            await SeedUsersAsync(WebConstants.ModeratorRole, ModeratorsCount, userManager, db);
            await SeedUsersAsync(WebConstants.AstronomerRole, AstronomersCount, userManager, db);

            await SeedTelescopesAsync(db, TelescopesCount);
            await SeedJournalsAsync(db, JournalsCount);
        }

        private static async Task CreateRoleAsync(string roleName, RoleManager<Role> roleManager, StarStuffDbContext db)
        {
            bool roleExists = await roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
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
            bool hasUsers = db.Users.Any();

            if (hasUsers)
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
            bool hasUsersWithRole = db.Users.Any(u => u.Roles.Any(r => r.Role.Name == roleName));

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
            if (db.Telescopes.Any())
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

                db.Telescopes.Add(telescope);
            }

            await db.SaveChangesAsync();
        }

        private static async Task SeedJournalsAsync(StarStuffDbContext db, int journalsCount)
        {
            if (db.Journals.Any())
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

                db.Journals.Add(journal);
            }

            await db.SaveChangesAsync();
        }
    }
}