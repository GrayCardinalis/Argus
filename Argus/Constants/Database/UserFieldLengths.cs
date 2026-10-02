namespace Argus.Constants.Database
{
    // Maximum lengths of columns in the users table.
    // Both EF configurations and validators are needed, so they live in the same place.
    public class UserFieldLengths
    {
        public const int FullNameMax = 200;
        public const int DepartmentMax = 200;
        public const int EmailMax = 100;
        public const int UserNameMax = 100;
        public const int RoleMax = 50;
    }
}
