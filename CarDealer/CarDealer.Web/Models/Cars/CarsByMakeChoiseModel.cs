namespace CarDealer.Web.Models.Cars
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CarsByMakeChoiseModel : CarsByMakeModel
    {
        public IEnumerable<SelectListItem> Makers { get; set; }
    }
}