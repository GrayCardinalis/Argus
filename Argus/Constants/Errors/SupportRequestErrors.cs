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

        public static readonly Error AssignmentForbidden =
                Error.Forbidden(
                    code: "SupportRequest.AssignmentForbidden",
                    description: "You do not have permission to assign executor.");

        public static readonly Error ExecutorMustBeTechnician =
                Error.Conflict(
                    code: "SupportRequest.ExecutorMustBeTechnician",
                    description: "The assigned executor must have the role of Technician.");

        public static readonly Error ResolvedRequestCannotBeReassigned =
                Error.Conflict(
                    code: "SupportRequest.ResolvedRequestCannotBeReassigned",
                    description: "Not possible to assign a person to a completed service support request.");

        public static readonly Error AssignmentConflict =
                Error.Conflict(
                    code: "SupportRequest.AssignmentConflict",
                    description: "The support request changed during assignment. Reload it and try again.");
    }
}
