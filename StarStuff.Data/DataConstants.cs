namespace StarStuff.Data
{
    public static class DataConstants
    {
        public static class UserConstants
        {
            public const int FirstNameMinLength = 2;
            public const int FirstNameMaxLength = 50;
            public const int LastNameMinLength = 2;
            public const int LastNameMaxLength = 50;
            public const int MaxImageSize = 1024 * 1024;
        }

        public static class TelescopeConstants
        {
            public const int NameMaxLength = 255;
            public const int LocationMaxLength = 255;
            public const double MinMirrorDiameter = 0.01;
        }

        public static class JournalConstants
        {
            public const int NameMaxLength = 50;
        }

        public static class DiscoveryConstants
        {
            public const int MaxStarsPerDiscovery = 3;
            public const int StarSystemMaxLength = 255;

            public const string MaxStarsPerDiscoveryErrorMessage = "Maximum allowed Stars count per Discovery is {0}";
        }

        public static class PlanetConstants
        {
            public const int NameMaxLength = 255;
            public const double MassMinValue = double.Epsilon;
            public const double MassMaxValue = double.MaxValue;
        }

        public static class StarConstants
        {
            public const int NameMaxLength = 255;
            public const int TemperatureMinValue = 2400;
            public const int TemperatureMaxValue = int.MaxValue;
        }

        public static class PublicationConstants
        {
            public const int TitleMaxLength = 100;
        }

        public const int ImageUrlMinLength = 10;
        public const int ImageUrlMaxLength = 2000;

        public const string ImageUrlPattern = @"^(http|https):\/{2}[^\/].*";
        public const string InvalidUrlFormatMessage = "Image Url should be valid url address";
    }
}