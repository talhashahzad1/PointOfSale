namespace PointOfSale.Models
{
    public class VMPurchase
    {
        public int Id { get; set; }

        public string SupplierName { get; set; }

        //public string SupplierAddress { get; set; }

        public string ProductName { get; set; }

        public decimal PurchasePrice { get; set; }

        public int Quantity { get; set; }

        public int SupplierId { get; set; }

        public int ProductId { get; set; }

        // Additional computed property for total cost
        public decimal TotalCost => PurchasePrice * Quantity;
    }
}
