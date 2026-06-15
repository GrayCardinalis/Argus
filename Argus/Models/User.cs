namespace Argus.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public enum UserRole
        {
            Client = 1,
            Technician = 2,
            Admin = 3
        }
    }
}
