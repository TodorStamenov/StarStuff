namespace StarStuff.Services.Models.Comments
{
    using System;

    public class ListCommentsServiceModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string ProfileImage { get; set; }

        public string Content { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsOwner { get; set; }
    }
}