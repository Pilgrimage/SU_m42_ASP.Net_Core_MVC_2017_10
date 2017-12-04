namespace CarDealer.Web.Models.Sales
{
    using System.ComponentModel.DataAnnotations;

    public class SalesReviewViewModel
    {
        private const string DiscountDriverString = "(+5%)";

        public int CustomerId { get; set; }

        [Display(Name = "Customer:")]
        public string CustomerName { get; set; }

        public int CarId { get; set; }

        [Display(Name = "Car:")]
        public string CarMakeModel { get; set; }

        public int DiscountCar { get; set; }

        public int DiscountDriver { get; set; }
        
        [Display(Name = "Car Price:")]
        public decimal Price { get; set; }


        [Display(Name = "Discount:")]
        public string DiscountPresentation => $"{this.DiscountCar}% {(this.DiscountDriver == 0 ? string.Empty : DiscountDriverString)}";

        public int DiscountTotal => (DiscountCar + DiscountDriver);


        [Display(Name = "Final Car Price:")]
        public decimal FinalPrice => (this.Price * (decimal)(100m - (this.DiscountCar + DiscountDriver))/100m);

    }
}