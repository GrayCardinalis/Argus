using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Argus.Infrastructure
{
    // Global filter: before calling any controller method, it finds the FluentValidation validator for each argument and checks it.
    // If there are errors, the response is 400 (RFC 7807), and the controller method is not called.
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is null)
                    continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

                var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

                if (validator is null)
                    continue;

                var result = await validator.ValidateAsync(
                    new ValidationContext<object>(argument),
                    context.HttpContext.RequestAborted);

                foreach (var error in result.Errors)
                    context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            if(!context.ModelState.IsValid)
            {
                var problemFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                var problem = problemFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);

                context.Result = new ObjectResult(problem) { StatusCode = problem.Status };
                return;
            }

            await next();

        }
    }
}
