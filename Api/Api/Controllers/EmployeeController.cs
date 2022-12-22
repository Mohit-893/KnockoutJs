using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly APIDBContext _context;
        public EmployeeController(APIDBContext context)
        {
            _context = context;
        }


        [HttpGet("getallemployee")]
        public IActionResult GetAllEmployees()
        {
            return Ok(_context.Emp.ToList());
        }

        [HttpGet("searchemployeebyid/{id}")]
        public IActionResult SearchById(int id)
        {
                return Ok(_context.Emp.Where(x => x.ID == id).FirstOrDefault());
        }

        [HttpGet("searchemployeebyname/{name}")]
        public IActionResult SearchByName(string name)
        {
                return Ok(_context.Emp.Where(x => x.Name == name).ToList());
        }

        [HttpPost("addemployee")]
        public void AddEmployee(Employee emp)
        {
                _context.Emp.Add(emp);
                _context.SaveChanges();
        }
        [HttpDelete("deleteemployee/{id}")]
        public void DeleteEmployee(int id)
        {
            _context.Emp.Remove(_context.Emp.FirstOrDefault(x => x.ID == id));
            _context.SaveChanges();
        }

        [HttpPut("updateemployee/{id}")]
        public void UpdateEmployee(int id,[FromBody]Employee emp)
        {
            var oldData = _context.Emp.FirstOrDefault(e => e.ID == id);
            if (oldData != null)
            {
                oldData.Name = emp.Name;
                oldData.Age = emp.Age;
                oldData.DOB = emp.DOB;
                oldData.DOJ = emp.DOJ;
                oldData.City = emp.City;
                oldData.State = emp.State;
                _context.SaveChanges();
            }
            
        }
    }
}
