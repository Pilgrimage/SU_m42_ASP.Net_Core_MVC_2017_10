namespace CarDealer.Web.Controllers
{
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models;
    using CarDealer.Services.Models.Customers;
    using CarDealer.Web.Infrastructure.Extensions;
    using CarDealer.Web.Models.Customers;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;

    [Route("customers")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService customers;

        public CustomersController(ICustomerService customers)
        {
            this.customers = customers;
        }


        [Route("all/{order?}")]
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


        [Route("{id}")]
        public IActionResult TotalSales(int id)
        {
            CustomerTotalSalesModel customerWithSales = this.customers.TotalSalesById(id);

            return this.ViewOrNotFound(customerWithSales);
        }


        [Route("create")]
        public IActionResult Create() => this.View();


        [HttpPost]
        [Route("create")]
        public IActionResult Create(CustomerFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(formModel);
            }

            this.customers.Create(formModel.Name, formModel.BirthDate, formModel.IsYoungDriver);

            return this.RedirectToAction(nameof(All), new { order = OrderDirection.Ascending });
        }


        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var customer = this.customers.ById(id);

            if (customer == null)
            {
                return this.NotFound();
            }

            return this.View(new CustomerFormModel
            {
                Name = customer.Name,
                BirthDate = customer.BirthDate,
                IsYoungDriver = customer.IsYoungDriver
            });
        }


        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, CustomerFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(formModel);
            }

            var customerExist = this.customers.Exists(id);

            if (!customerExist)
            {
                return this.NotFound();
            }

            this.customers.Edit(id, formModel.Name, formModel.BirthDate, formModel.IsYoungDriver);

            return this.RedirectToAction(nameof(All), new { order = OrderDirection.Ascending });

        }
    }
}