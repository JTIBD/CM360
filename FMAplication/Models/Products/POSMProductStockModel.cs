namespace FMAplication.Models.Products
{
    public class POSMProductStockModel : POSMProductModel
    {
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
    }
}