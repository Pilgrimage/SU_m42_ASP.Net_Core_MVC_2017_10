namespace CarDealer.Services.Models.Sales
{
    public class SaleListModel : SaleModel
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public bool IsYoungDriver { get; set; }

        public double TotalDiscount => (this.Discount + (this.IsYoungDriver ? 0.05 : 0));

        public decimal DiscountedPrice => this.Price * (decimal)(1-this.TotalDiscount);
    }
}