namespace StarStuff.Web.Infrastructure.Filters
{
    using Data.Models;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Services.Admin;

    public class LogAttribute : ActionFilterAttribute
    {
        private readonly LogType logType;
        private readonly string tableName;

        public LogAttribute(LogType logType, string tableName)
        {
            this.logType = logType;
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

            logService.Log(username, this.logType, this.tableName);
        }
    }
}