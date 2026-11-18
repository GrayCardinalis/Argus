namespace Argus.Dtos.Components
{
    public class UpdateComponentStockDto
    {
        public required int ExpectedQuantity { get; set; }
        public required int Quantity { get; set; }
    }
}
