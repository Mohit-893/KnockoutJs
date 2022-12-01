using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace readDataFromCSV.Pages.Employee
{
    public class IndexModel : PageModel
    {
        public List<EmployeeInfo> Employees = new List<EmployeeInfo>();
        public List<EmployeeInfo> ValidEmployees = new List<EmployeeInfo>();
        public void OnGet()
        {
            try
            {
                //using var streamReader = File.OpenText([],@"C:\Users\mohit\Downloads\ms.csv");
                using (var streamReader = new StreamReader(@"C:\Users\mohit\Downloads\ms.csv"))
                {
                    using (var csvReader = new CsvReader(streamReader, false))
                    {
                        Employees = csvReader.GetRecords<EmployeeInfo>().ToList();
                    }
                }
                foreach (var item in Employees)
                {
                    if((item.Name != "") & (item.State != "") & (item.City != "") & (item.Age >= 21 & item.Age <= 60 ) & (item.DOB != null) & (item.DOJ != null))
                    {
                        ValidEmployees.Add(item);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.ToString());
            }
        }
    }
    public class EmployeeInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? DOJ { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
