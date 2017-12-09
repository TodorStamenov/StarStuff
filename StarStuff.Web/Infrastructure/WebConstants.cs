namespace StarStuff.Web.Infrastructure
{
    using System.IO;

    public class WebConstants
    {
        public const string AdminRole = "Administrator";
        public const string ModeratorRole = "Moderator";
        public const string AstronomerRole = "Astronomer";
        public const string TempDataSuccessMessage = "SuccessMessage";
        public const string TempDataErrorMessage = "ErrorMessage";

        public const string EntryExists = "{0} name already exists in Database";
        public const string DataImage = "data:image/jpeg;base64,";

        public static readonly byte[] DefaultImage = File.ReadAllBytes($@"{Directory.GetCurrentDirectory()}\wwwroot\images\default-user-image.png");
        public static readonly string Lorem = File.ReadAllText($@"{Directory.GetCurrentDirectory()}\wwwroot\Lorem.txt");
        public static readonly string CommentContent = File.ReadAllText($@"{Directory.GetCurrentDirectory()}\wwwroot\Comment.txt");
    }
}