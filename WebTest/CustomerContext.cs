using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using WebTest.Models;

namespace WebTest
{
    public class CustomerContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
