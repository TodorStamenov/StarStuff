﻿namespace StarStuff.Web.Infrastructure
{
    using System.IO;

    public class WebConstants
    {
        public const string AdminRole = "Administrator";
        public const string ModeratorRole = "Moderator";
        public const string AstronomerRole = "Astronomer";
        public const string TempDataSuccessMessage = "SuccessMessage";
        public const string TempDataErrorMessage = "ErrorMessage";

        public const string DataImage = "data:image/jpeg;base64,";
        public static byte[] DefaultImage { get; } = File.ReadAllBytes($"{Directory.GetCurrentDirectory()}\\wwwroot\\images\\default-user-image.png");
    }
}