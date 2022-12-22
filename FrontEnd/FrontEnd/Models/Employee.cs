using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? DOJ { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
    public class PostEmployee
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? DOJ { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }

    public class InvalidEmployee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? DOJ { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string InvalidString { get; set; }
    }
}
