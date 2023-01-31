using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiKalum.Utilities
{
    public class ActionFilter : IActionFilter
    {
        private readonly ILogger<ActionFilter> Logger;
        public ActionFilter(ILogger<ActionFilter> _Logger)
        {
            this.Logger = _Logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Logger.LogInformation("Esto se ejecuta antes de la acción a realizar");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Logger.LogInformation("Esto se ejecuta despues de la acciión realizada");
        }
        
    }
}