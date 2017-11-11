namespace CarDealer.Services.Models.Sales
{
    using CarDealer.Services.Models.Cars;

    public class SaleDetailsModel : SaleListModel
    {
        public CarModel Car { get; set; }
    }
}