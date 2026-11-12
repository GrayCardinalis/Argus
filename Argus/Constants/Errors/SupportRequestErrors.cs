using ErrorOr;

namespace Argus.Constants.Errors
{
    public class SupportRequestErrors
    {
        public static readonly Error SupportRequestEquipmentRecycled =
            Error.Conflict(
                    code: "SupportRequest.EquipmentSupportRequestRecycled",
                    description: "Cannot create a support request for recycled equipment.");

        public static readonly Error SupportRequestNotFound =
            Error.NotFound(
                    code: "SupportRequest.SupportRequestNotFound",
                    description: "The specified support request was not found.");
    }
}
