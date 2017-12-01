namespace StarStuff.Web.Infrastructure.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Linq;

    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                Controller controller = context.Controller as Controller;

                if (controller == null)
                {
                    return;
                }

                object model = context
                    .ActionArguments
                    .FirstOrDefault(a => a.Key.ToLower().Contains("model"))
                    .Value;

                if (model == null)
                {
                    return;
                }

                context.Result = controller.View(model);
            }
        }
    }
}