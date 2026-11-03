namespace Argus.Dtos.SupportRequest.Components
{
    public class SupportRequestComponentDto
    {
        public Guid ComponentId { get; set; }
        public string ComponentName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
