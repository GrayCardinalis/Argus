namespace Argus.Constants.Database
{
    public static class DatabaseFilters
    {
        public const string NotDeleted = "is_deleted = false";
        public const string ActivePlacement = "removed_at IS NULL";
    }
}
