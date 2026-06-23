namespace Argus.Dtos.Components
{
    public class CreateComponentDto
    {
        public required string Name { get; set; } = string.Empty;
        public required int Quantity { get; set; }
    }
}
