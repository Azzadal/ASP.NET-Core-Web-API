using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading;
using WebTest.Controllers;
using WebTest.Models;
using Xunit;

namespace WebTest.Tests
{
    public class CustomersControllerTests
    {
        CustomerContext _context;
        CustomersController controller;

        public CustomersControllerTests()
        {
            var options = new DbContextOptionsBuilder<CustomerContext>()
                            .UseInMemoryDatabase(databaseName: "Test")
                            .Options;
            _context = new CustomerContext(options);
            controller = new CustomersController(_context);
        }

            [Fact]
        public void PutTests()
        {
            // Arrange

            int threadCount = 10;
            int customerCount = 50;
            Thread[] threads = new Thread[threadCount];
            HttpResponseMessage[] resultsDeposit = new HttpResponseMessage[threadCount * customerCount];
            HttpResponseMessage[] resultsWithDraw = new HttpResponseMessage[threadCount * customerCount];
            int[] customerIds = new int[customerCount];
            for(int i = 0; i < customerCount; ++i)
            {
                Customer customer = new Customer {
                    Name = "Customer " + i,
                };
                _context.Add(customer);
                _context.SaveChanges();
                customerIds[i] = customer.Id;
            }

            // Act
            for (int i = 0; i < threadCount; i++)
            {
                int threadIndex = i;
                threads[i] = new Thread(
                                () =>
                                {
                                    for (int j = 0; j < customerCount; j++)
                                    {
                                        int customerId = customerIds[j];
                                        resultsDeposit[threadIndex * customerCount + j] = controller.Deposit(customerId, 123);
                                        resultsWithDraw[threadIndex * customerCount + j] = controller.Withdraw(customerId, 123);
                                    }
                                }
                                );
            }
            foreach (Thread thread in threads) 
            {
                thread.Start();
            }

            // Assert
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            foreach (var result in resultsDeposit)
            {
                Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
            }
            foreach (var result in resultsWithDraw)
            {
                Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
            }
            foreach (var customerId in customerIds)
            {
                Customer customer = _context.Customers.Find(customerId);
                Assert.Equal(0, customer.Balance);
            }
        }
    }
}
