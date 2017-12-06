namespace StarStuff.Services
{
    using Models.Comments;
    using System.Collections.Generic;

    public interface ICommentService
    {
        int Total(int publicationId);

        bool CanEdit(int id, int userId);

        bool Create(int publicationId, int userId, string content);

        bool Edit(int id, string content);

        bool Delete(int id);

        CommentFormServiceModel GetForm(int id);

        IEnumerable<ListCommentsServiceModel> All(int publicationId, int page, int pageSize, int? userId);
    }
}