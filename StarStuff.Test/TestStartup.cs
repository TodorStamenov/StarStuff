namespace StarStuff.Test
{
    using AutoMapper;
    using StarStuff.Web.Infrastructure.Mapping;

    public class TestStartup
    {
        private static object sync = new object();
        private static bool testsInitialized = false;

        public static void Initialize()
        {
            lock (sync)
            {
                if (!testsInitialized)
                {
                    Mapper.Initialize(config => config.AddProfile<AutoMapperProfile>());
                    testsInitialized = true;
                }
            }
        }
    }
}