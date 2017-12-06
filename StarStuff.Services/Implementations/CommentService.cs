namespace StarStuff.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Models.Comments;
    using StarStuff.Data;
    using StarStuff.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommentService : ICommentService
    {
        private readonly StarStuffDbContext db;

        public CommentService(StarStuffDbContext db)
        {
            this.db = db;
        }

        public int Total(int publicationId)
        {
            return this.db.Comments.Count(c => c.PublicationId == publicationId);
        }

        public bool CanEdit(int id, int userId)
        {
            return this.db.Comments.Any(c => c.Id == id && c.UserId == userId);
        }

        public bool Create(int publicationId, int userId, string content)
        {
            if (!this.db.Publications
                .Any(p => p.Id == publicationId))
            {
                return false;
            }

            Comment comment = new Comment
            {
                PublicationId = publicationId,
                UserId = userId,
                Content = content,
                DateAdded = DateTime.UtcNow
            };

            this.db.Comments.Add(comment);
            this.db.SaveChanges();

            return true;
        }

        public bool Edit(int id, string content)
        {
            Comment comment = this.db.Comments.Find(id);

            if (comment == null)
            {
                return false;
            }

            comment.Content = content;

            this.db.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            Comment comment = this.db.Comments.Find(id);

            if (comment == null)
            {
                return false;
            }

            this.db.Comments.Remove(comment);
            this.db.SaveChanges();

            return true;
        }

        public CommentFormServiceModel GetForm(int id)
        {
            return this.db
                .Comments
                .Where(c => c.Id == id)
                .ProjectTo<CommentFormServiceModel>()
                .FirstOrDefault();
        }

        public IEnumerable<ListCommentsServiceModel> All(int publicationId, int page, int pageSize, int? userId)
        {
            return this.db
                .Comments
                .Where(c => c.PublicationId == publicationId)
                .OrderBy(c => c.DateAdded)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ListCommentsServiceModel
                {
                    Id = c.Id,
                    Content = c.Content,
                    DateAdded = c.DateAdded,
                    Username = c.User.UserName,
                    ProfileImage = ServiceConstants.DataImage + Convert.ToBase64String(c.User.ProfileImage),
                    IsOwner = c.UserId == userId
                })
                .ToList();
        }
    }
}