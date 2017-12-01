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
            public const int ImageUrlMinLength = 10;

            public const string ImageUrlPattern = @"^(http|https):\/{2}[^\/].*";
        }
    }
}