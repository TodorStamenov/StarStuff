namespace StarStuff.Web.Infrastructure
{
    using System.IO;

    public class WebConstants
    {
        public const string AdminRole = "Administrator";
        public const string ModeratorRole = "Moderator";
        public const string AstronomerRole = "Astronomer";

        public const string AdminArea = "Admin";
        public const string AstronomerArea = "Astronomer";
        public const string ModeratorArea = "Moderator";

        public const string Added = "Added";
        public const string Edited = "Edited";
        public const string Deleted = "Deleted";

        public const string TempDataSuccessMessage = "SuccessMessage";
        public const string TempDataErrorMessage = "ErrorMessage";

        public const string SuccessfullEntityOperation = "{0} Successfully {1}";
        public const string NotExistingUser = "User with {0} id was not found";

        public const string EntryExists = "{0} name already exists in Database";
        public const string PublicationFromJournalExists = "Publication from {0} Journal for {1} Discovery already exists!";

        public const string DataImage = "data:image/jpeg;base64,";

        public const int TelescopeDescriptionSliceLength = 800;
        public const int JournalDescriptionSliceLength = 800;

        public const string UserAddedToRole = "User {0} successfully added to role {1}";
        public const string UserRemovedFormRole = "User {0} successfully removed from role {1}";
        public const string UserLocked = "User {0} successfully Locked";
        public const string UserUnlocked = "User {0} successfully Unlocked";

        public static readonly byte[] DefaultImage = File.ReadAllBytes($@"{Directory.GetCurrentDirectory()}\wwwroot\images\default-user-image.png");
        public static readonly string Lorem = File.ReadAllText($@"{Directory.GetCurrentDirectory()}\wwwroot\Lorem.txt");
        public static readonly string CommentContent = File.ReadAllText($@"{Directory.GetCurrentDirectory()}\wwwroot\Comment.txt");
    }
}