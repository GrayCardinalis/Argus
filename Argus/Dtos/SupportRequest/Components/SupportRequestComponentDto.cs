namespace Argus.Dtos.SupportRequest.Components
{
    public class SupportRequestComponentDto
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
