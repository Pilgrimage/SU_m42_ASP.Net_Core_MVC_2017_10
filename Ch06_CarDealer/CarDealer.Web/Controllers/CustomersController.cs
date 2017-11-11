namespace CarDealer.Web.Controllers
{
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models;
    using CarDealer.Services.Models.Customers;
    using CarDealer.Web.Infrastructure.Extensions;
    using CarDealer.Web.Models.Customers;
    using Microsoft.AspNetCore.Mvc;

    public class CustomersController : Controller
    {
        private readonly ICustomerService customers;

        public CustomersController(ICustomerService customers)
        {
            this.customers = customers;
        }


        [Route("customers/all/{order?}")]
        public IActionResult All(string order)
        {
            OrderDirection orderDirection = (order.ToLower() == "descending") 
                ? OrderDirection.Descending 
                : OrderDirection.Ascending;


            var result = this.customers.Ordered(orderDirection);

            return this.View(new AllCustomersModel
            {
                Customers = result,
                OrderDirection = orderDirection
            });
        }
        

        [Route("customers/{id}")]
        public IActionResult TotalSales(int id)
        {
            CustomerTotalSalesModel customerWithSales = this.customers.TotalSalesById(id);

            return this.ViewOrNotFound(customerWithSales);
        }
    }
}