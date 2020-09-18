using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTest.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public float Balance { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Customer customer &&
                   Id == customer.Id &&
                   Name == customer.Name &&
                   Surname == customer.Surname &&
                   Patronymic == customer.Patronymic &&
                   DateOfBirth == customer.DateOfBirth &&
                   Balance == customer.Balance;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Surname, Patronymic, DateOfBirth, Balance);
        }
    }
}
