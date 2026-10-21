using ErrorOr;

namespace Argus.Constants.Errors
{
    public class PlacementHistoryErrors
    {
        public static readonly Error EquipmentNotFound = 
            Error.NotFound(
                    code: "PlacementHistory.EquipmentNotFound",
                    description: "The specified equipment was not found.");

        public static readonly Error AuditoriumNotFound = 
            Error.NotFound(
                code: "PlacementHistory.AuditoriumNotFound",
                description: "The target auditorium (location) was not found.");
        public static readonly Error AlreadyInLocation =
            Error.Conflict(
                    code: "PlacementHistory.AlreadyInLocation",
                    description: "The equipment is already in the specified location.");

    }
}
