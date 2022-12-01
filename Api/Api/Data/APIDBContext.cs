using Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data
{
    public class APIDBContext : DbContext
    {
        public APIDBContext()
        {

        }
        public APIDBContext(DbContextOptions<APIDBContext> options) : base(options)
        {

        }

        public virtual DbSet<Employee> Emp { get; set; }
    }
}
