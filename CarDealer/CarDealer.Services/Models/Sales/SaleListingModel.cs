namespace CarDealer.Services.Models.Sales
{
    public class SaleListingModel : SaleModel
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public bool IsYoungDriver { get; set; }

        public double TotalDiscount => this.Discount;

        public decimal DiscountedPrice => this.Price * (decimal)(1-this.Discount);
    }
}