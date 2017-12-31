namespace StarStuff.Web.Infrastructure.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Services.Areas.Admin;

    public class LogAttribute : ActionFilterAttribute
    {
        private readonly string action;
        private readonly string tableName;

        public LogAttribute(string action, string tableName)
        {
            this.action = action;
            this.tableName = tableName;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            IAdminUserService logService = context
                .HttpContext
                .RequestServices
                .GetService<IAdminUserService>();

            string username = context
                .HttpContext
                .User
                .Identity
                .Name;

            logService.Log(username, this.action, this.tableName);
        }
    }
}