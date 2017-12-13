namespace StarStuff.Test
{
    using Microsoft.EntityFrameworkCore;
    using StarStuff.Data;
    using System;

    public abstract class BaseTest
    {
        protected BaseTest()
        {
            TestStartup.Initialize();
        }

        protected StarStuffDbContext Database
        {
            get
            {
                DbContextOptions<StarStuffDbContext> options = new DbContextOptionsBuilder<StarStuffDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new StarStuffDbContext(options);
            }
        }
    }
}