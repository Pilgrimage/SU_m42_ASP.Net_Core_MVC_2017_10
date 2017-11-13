namespace CarDealer.Web.Models.Cars
{
    using System.Collections.Generic;
    using CarDealer.Services.Models.Cars;

    public class CarPageListingViewModel
    {
        public IEnumerable<CarWithPartsModel> Cars { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PreviousPage => this.CurrentPage <= 1
            ? 1
            : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage >= this.TotalPages
            ? this.TotalPages
            : this.CurrentPage + 1;

    }
}