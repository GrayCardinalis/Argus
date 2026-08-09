using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Argus.Controllers
{

    // Все общие атрибуты (типа [ApiController]) переезжают сюда
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected ActionResult Problem(List<Error> errors)
        {
            if (errors.Count == 0)
                return Problem();

            // Если ВСЕ ошибки в списке — это ошибки валидации, отдаем 400 Bad Request с деталями
            if (errors.All(error => error.Type == ErrorType.Validation))
                return ValidationProblem(errors);

            // В противном случае берем первую ошибку и маппим её статус
            return Problem(errors[0]);
        }

        private ActionResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
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