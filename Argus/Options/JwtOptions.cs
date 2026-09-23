using System.ComponentModel.DataAnnotations;

namespace Argus.Options
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";
        [Required]
        [MinLength(32)]
        public string Key { get; set; } = string.Empty;
        [Required]
        public string Issuer { get; set; } = string.Empty;
        [Required]
        public string Audience { get; set; } = string.Empty;
        //[Required] General rule: [Required] is meaningful only for reference types and Nullable<T>. For a value type, its role is fulfilled by a range check that excludes the default value.
        [Range(1, 15)]
        public int AccessTokenLifetimeInMinutes { get; set; }
    }
}
