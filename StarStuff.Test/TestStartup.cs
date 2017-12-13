namespace StarStuff.Test
{
    using AutoMapper;
    using StarStuff.Web.Infrastructure.Mapping;

    public class TestStartup
    {
        private static bool testsInitialized = false;

        public static void Initialize()
        {
            if (!testsInitialized)
            {
                Mapper.Initialize(config => config.AddProfile<AutoMapperProfile>());
                testsInitialized = true;
            }
        }
    }
}