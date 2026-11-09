using ErrorOr;

namespace Argus.Constants.Errors
{
    public class AuditoriumErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Auditorium.NotFound",
                description: "Auditorium not found.");

        public static readonly Error InUse =
            Error.Conflict(
                code: "Auditorium.InUse",
                description: "The auditorium cannot be deleted because it is referenced by placement history or support requests.");
    }
}
