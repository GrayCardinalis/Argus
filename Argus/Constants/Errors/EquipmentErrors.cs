using ErrorOr;

namespace Argus.Constants.Errors
{
    public static class EquipmentErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Equipment.NotFound",
                description: "Equipment not found.");

        public static readonly Error DuplicateInventoryNumber =
            Error.Conflict(
                code: "Equipment.DuplicateInventoryNumber",
                description: "Equipment with this inventory number already exists.");

        public static readonly Error AuditoriumNotFound = 
            Error.NotFound(
                code: "Equipment.AuditoriumNotFound",
                description: "Auditorium not found.");

        public static readonly Error AlreadyRecycled =
            Error.Conflict(
                code: "Equipment.AlreadyRecycled",
                description: "Recycled equipment cannot be updated.");

        public static readonly Error RecyclingRequiresDelete =
            Error.Validation(
                code: "Equipment.RecyclingRequiresDelete",
                description: "Use the delete endpoint to recycle equipment.");
    }
}
