namespace BL.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = "";
                foreach (var error in context.ModelState.Values)
                {
                    foreach (var field in error.Errors)
                    {
                        errors += $"\n{field.ErrorMessage}";
                    }
                }
                var result = new ObjectResult(errors)
                {
                    StatusCode = 400
                };
                context.Result = result;
            }
            base.OnActionExecuting(context);
        }
    }
}
