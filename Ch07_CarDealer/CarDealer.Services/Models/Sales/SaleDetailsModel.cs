namespace CarDealer.Services.Models.Sales
{
    using CarDealer.Services.Models.Cars;

    public class SaleDetailsModel : SaleListingModel
    {
        public CarModel Car { get; set; }
    }
}