using DemoDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DemoDatabase.Controllers
{
    public class DatabaseController : ApiController
    {
        //get Customer Details
        [HttpGet]
        [Route("api/Database/GetCustomer")]
        public HttpResponseMessage GetCustomer()
        {
            using (CustomersDBContext dbContext = new CustomersDBContext())
            {
                var customers = dbContext.Customers.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, customers);
            }
        }
        [HttpGet]
        [Route("api/Database/GetCustomerByID/{id}")]
        public HttpResponseMessage GetCustomerByID(int id)
        {
            using (CustomersDBContext dbContext = new CustomersDBContext())
            {
                var customers = dbContext.Customers.FirstOrDefault(c => c.CustomerID == id);
                if (customers != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, customers);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer with ID" + id.ToString() + "not found");
                }
            }
        }
        //Create new customer data
        [HttpPost]
        [Route("api/Database/AddCustomer")]
        public HttpResponseMessage AddCustomer([FromBody]Customer customer)
        {
            try
            {
                using (CustomersDBContext dbContext = new CustomersDBContext())
                {
                    dbContext.Customers.Add(customer);
                    dbContext.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, customer);
                    message.Headers.Location = new Uri(Request.RequestUri + customer.CustomerID.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        //Update Customer Details
        [HttpPut]
        [Route("api/Database/UpdateCustomer/{id}")]
        public HttpResponseMessage UpdateCustomer(int id, [FromBody] Customer customer)
        {
            try
            {
                using (CustomersDBContext dbContext = new CustomersDBContext())
                {
                    var entity = dbContext.Customers.FirstOrDefault(c => c.CustomerID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer with id" + id.ToString() + "Not found");

                    }
                    else
                    {
                        entity.Name = customer.Name;
                        entity.Email = customer.Email;
                        entity.Mobile = customer.Mobile;
                        dbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        [HttpDelete]
        [Route("api/Database/DeleteCustomer/{id}")]
        public HttpResponseMessage DeleteCustomer(int id)
        {
            try
            {
                using (CustomersDBContext dbContext = new CustomersDBContext())
                {
                    var entity = dbContext.Customers.FirstOrDefault(c => c.CustomerID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer with id" + id.ToString() + "Not found to delete");

                    }
                    else
                    {
                        dbContext.Customers.Remove(entity);
                        dbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
