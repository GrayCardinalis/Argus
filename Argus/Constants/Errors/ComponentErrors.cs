using ErrorOr;

namespace Argus.Constants.Errors
{
    public class ComponentErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Component.NotFound",
                description: "Component not found.");

        public static readonly Error InUse =
            Error.Conflict(
                code: "Component.InUse",
                description: "The component cannot be deleted because it is referenced by support requests.");

        public static readonly Error NonZeroStock =
            Error.Conflict(
                code: "Component.NonZeroStock",
                description: "The component cannot be deleted while its stock quantity is non-zero.");
    }
}
