namespace Argus.Dtos.SupportRequest.Components
{
    public class AddSupportRequestComponentDto
    {
        public required Guid ComponentId { get; set; }
        public required int Quantity { get; set; }
    }
}
