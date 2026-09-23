using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Argus.Controllers
{

    // All common attributes (such as [ApiController]) are moved here
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected ActionResult Problem(List<Error> errors)
        {
            if (errors.Count == 0)
                return Problem();

            // If ALL errors in the list are validation errors, return 400 Bad Request with details.
            if (errors.All(error => error.Type == ErrorType.Validation))
                return ValidationProblem(errors);

            // Otherwise, we take the first error and map its status.
            return Problem(errors[0]);
        }

        private ActionResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(
                detail: error.Description,
                statusCode: statusCode,
                title: error.Code
            );
        }

        private ActionResult ValidationProblem(List<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(
                    error.Code,
                    error.Description
                );
            }

            return ValidationProblem(modelStateDictionary);
        }
    }
}