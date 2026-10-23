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
    }
}
