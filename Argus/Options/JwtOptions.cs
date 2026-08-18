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
        //[Required] Общее правило: [Required] осмыслен только для ссылочных типов и Nullable<T>. Для типа-значения его роль выполняет проверка диапазона, отсекающая значение по умолчанию.
        [Range(1, 60)]
        public int AccessTokenLifetimeInMinutes { get; set; }
    }
}
