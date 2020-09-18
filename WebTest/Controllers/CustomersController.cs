﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
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
            lock (this) {
                Customer customer = _context.Customers.Find(id);
                customer.Balance += amount;

                _context.SaveChanges();

                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
        }
        
        public HttpResponseMessage Withdraw(int id, [FromBody] float amount)
        {
            Customer customer = _context.Customers.Find(id);
            customer.Balance -= amount;

            _context.SaveChanges();

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
