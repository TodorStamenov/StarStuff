namespace StarStuff.Test.Services
{
    using StarStuff.Data;
    using StarStuff.Data.Models;
    using StarStuff.Services.Implementations;
    using StarStuff.Services.Models.Comments;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class CommentServiceTest : BaseServiceTest
    {
        [Fact]
        public void Total_WithExistingPublicationId_ShouldReturnCommentsCount()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);
            this.SeedCommanets(db);

            int expected = this.GetFakeComments().Count;

            // Act
            int actual = commentService.Total(1);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Total_WithNotExistingPublicationId_ShouldReturnZero()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);

            int expected = 0;

            // Act
            int actual = commentService.Total(1);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEdit_WithExistingPublicationIdAndExistingUserId_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);
            this.SeedCommanets(db);

            // Act
            bool result = commentService.CanEdit(1, 1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanEdit_WithNotExistingPublicationIdAndExistingUserId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);
            this.SeedCommanets(db);

            // Act
            bool result = commentService.CanEdit(11, 1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanEdit_WithExistingPublicationIdAndNotExistingUserId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);
            this.SeedCommanets(db);

            // Act
            bool result = commentService.CanEdit(1, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanEdit_WithNotExistingPublicationIdAndNotExistingUserId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);
            this.SeedCommanets(db);

            // Act
            bool result = commentService.CanEdit(11, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Create_WithExistingPublicationIdAndExistingUserId_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);

            this.SeedUser(db);
            this.SeedPublication(db);

            // Act
            bool result = commentService.Create(1, 1, "Test Content");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Create_WithExistingPublicationIdAndExistingUserId_ShouldAddComment()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);

            this.SeedUser(db);
            this.SeedPublication(db);

            Comment expected = this.GetFakeComment();

            // Act
            commentService.Create(expected.PublicationId, expected.UserId, expected.Content);
            Comment actual = db.Comments.Find(1);

            // Assert
            this.CompareComments(expected, actual);
        }

        [Fact]
        public void Create_WithNotExistingPublicationIdAndExistingUserId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);

            this.SeedUser(db);

            // Act
            bool result = commentService.Create(1, 1, "Test Content");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Create_WithExistingPublicationIdAndNotExistingUserId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);

            this.SeedPublication(db);

            // Act
            bool result = commentService.Create(1, 1, "Test Content");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Create_WithNotExistingPublicationIdAndNotExistingUserId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);

            // Act
            bool result = commentService.Create(1, 1, "Test Content");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Edit_WithExistingCommentId_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);
            this.SeedCommanets(db);

            // Act
            bool result = commentService.Edit(1, "Test Content");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Edit_WithExistingCommentId_ShouldEditComment()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);
            this.SeedCommanets(db);

            Comment expected = this.GetFakeComment();

            expected.DateAdded = expected.DateAdded
               .AddDays(-1)
               .AddHours(-1)
               .AddMinutes(-1);

            // Act
            commentService.Edit(expected.Id, expected.Content);
            Comment actual = db.Comments.Find(expected.Id);

            // Assert
            this.CompareComments(expected, actual);
        }

        [Fact]
        public void Edit_WithNotExistingCommentId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);

            // Act
            bool result = commentService.Edit(1, "Test Content");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Delete_WithExistingCommentId_ShouldReturnTrue()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);
            this.SeedCommanets(db);

            // Act
            bool result = commentService.Delete(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Delete_WithExistingCommentId_ShouldDeleteComment()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);
            this.SeedCommanets(db);

            const int commentId = 1;

            // Act
            commentService.Delete(commentId);

            // Assert
            Assert.False(db.Comments.Any(c => c.Id == commentId));
        }

        [Fact]
        public void Delete_WithNotExistingCommentId_ShouldReturnFalse()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);

            // Act
            bool result = commentService.Delete(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetForm_WithExistingCommentId_ShouldReturnCorrectResult()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);
            this.SeedCommanets(db);

            const int commentId = 1;

            Comment expected = db.Comments.Find(commentId);

            // Act
            CommentFormServiceModel actual = commentService.GetForm(commentId);

            // Assert
            this.CompareComments(expected, actual);
        }

        [Fact]
        public void GetForm_WithExistingCommentId_ShouldReturnNull()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);

            const int commentId = 1;

            // Act
            CommentFormServiceModel result = commentService.GetForm(commentId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void All_WithExistingPublicationId_ShouldReturnComments()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);

            this.SeedCommanets(db);
            this.SeedPublication(db);
            this.SeedUser(db);

            List<Comment> fakeComments = this.GetFakeComments()
                .Skip(5)
                .Take(5)
                .ToList();

            int i = -1;

            // Act
            IEnumerable<ListCommentsServiceModel> comments = commentService.All(1, 2, 5, 1);

            // Assert
            foreach (var actual in comments)
            {
                Comment expected = fakeComments[++i];
                expected.DateAdded = expected.DateAdded.ToLocalTime();

                this.CompareComments(expected, actual);
            }
        }

        [Fact]
        public void All_WithNotExistingPublicationId_ShouldReturnEmptyCollection()
        {
            // Arrange
            StarStuffDbContext db = this.Database;
            CommentService commentService = new CommentService(db);

            // Act
            IEnumerable<ListCommentsServiceModel> result = commentService.All(1, 2, 5, 1);

            // Assert
            Assert.False(result.Any());
        }

        private void CompareComments(Comment expected, Comment actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Content, actual.Content);
            Assert.Equal(expected.PublicationId, actual.PublicationId);
            Assert.Equal(expected.UserId, actual.UserId);
            this.CompareDatesExact(expected.DateAdded, actual.DateAdded);
        }

        private void CompareComments(Comment expected, ListCommentsServiceModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Content, actual.Content);
            this.CompareDatesExact(expected.DateAdded, actual.DateAdded);
        }

        private void CompareComments(Comment expected, CommentFormServiceModel actual)
        {
            Assert.Equal(expected.Content, actual.Content);
        }

        private Comment GetFakeComment()
        {
            return new Comment
            {
                Id = 1,
                Content = $"Test Content {1}",
                DateAdded = DateTime.UtcNow,
                PublicationId = 1,
                UserId = 1
            };
        }

        private void SeedCommanets(StarStuffDbContext db)
        {
            db.Comments.AddRange(this.GetFakeComments());
            db.SaveChanges();
        }

        private void SeedUser(StarStuffDbContext db)
        {
            db.Users.Add(new User
            {
                Id = 1,
                UserName = "Fake Username",
                ProfileImage = new byte[] { }
            });

            db.SaveChanges();
        }

        private void SeedPublication(StarStuffDbContext db)
        {
            db.Publications.Add(new Publication { Id = 1 });
            db.SaveChanges();
        }

        private List<Comment> GetFakeComments()
        {
            List<Comment> comments = new List<Comment>();

            for (int i = 1; i <= 10; i++)
            {
                comments.Add(new Comment
                {
                    Id = i,
                    Content = $"Fake Content {i}",
                    DateAdded = DateTime.UtcNow.AddDays(-i).AddHours(-i).AddMinutes(-i),
                    PublicationId = 1,
                    UserId = 1
                });
            }

            return comments.OrderBy(c => c.DateAdded).ToList();
        }
    }
}