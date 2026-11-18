namespace Argus.Dtos.Components
{
    public class AdjustComponentStockDto
    {
        public required int ExpectedQuantity { get; set; }
        public required int ActualQuantity { get; set; }
    }
}