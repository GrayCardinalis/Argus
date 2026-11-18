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
        public static readonly Error InsufficientStock = 
            Error.Conflict(
                code: "Component.InsufficientStock",
                description: "There are not enough components in stock.");

        public static readonly Error ConsumptionQuantityLimitExceeded =
            Error.Conflict(
                code: "Component.ConsumptionQuantityLimitExceeded",
                description: "The total consumed quantity exceeds the supported limit.");
        public static readonly Error StockConflict =
            Error.Conflict(
                code: "Component.StockConflict",
                description: "Stock has changed. Read the current quantity before adjusting it.");
    }
}
