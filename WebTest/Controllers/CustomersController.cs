using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTest.Models;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace WebTest.Controllers
{
    public class CustomersController : Controller
    {
        private CustomerContext _context;

        public CustomersController(CustomerContext context)
        {
            _context = context;
        }

        
        // GET: Customers
        [HttpGet]
        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers;
        }

        
        public Customer GetCustomer(int id)
        {
            Customer customer = _context.Customers.Find(id);
            return customer;
        }
        
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public void Post([FromBody] Customer customer)
        {
            _context.Add(customer);
            _context.SaveChanges();
        }

        public HttpResponseMessage Deposit(int id, [FromBody] float amount)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Database.ExecuteSqlRaw("UPDATE " + "\"Customers\"" + " SET " + "\"Balance\"" + " = " + "\"Balance\"" + " + {0} WHERE " + "\"Id\"" + " = {1}", amount, id);
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }
        
        public HttpResponseMessage Withdraw(int id, [FromBody] float amount)
        {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                    _context.Database.ExecuteSqlRaw("UPDATE " + "\"Customers\"" + " SET " + "\"Balance\"" +  " = " + "\"Balance\"" + " - {0} WHERE " + "\"Id\"" + " = {1}", amount, id);
                    _context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }

                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            
        }

        [HttpDelete]
        public void Delete()
        {
            _context.Customers.RemoveRange(_context.Customers);
            _context.SaveChanges();
        }

        public void DeleteCustomerFromId(int id)
        {
            Customer customer = _context.Customers.Find(id);
            _context.Customers.Remove(customer);

            _context.SaveChanges();
        }

 
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CheckBalance()
        {
            return View();
        }

        public IActionResult EditCustomerBalance()
        {
            return View();
        }

        public IActionResult DeleteCustomer()
        {
            return View();
        }
    }
}
