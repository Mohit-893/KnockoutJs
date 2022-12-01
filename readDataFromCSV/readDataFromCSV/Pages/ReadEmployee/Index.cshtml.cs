using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IronXL;

namespace readDataFromCSV.Pages.ReadEmployee
{
    public class IndexModel : PageModel
    {
        public List<EmployeeInfo> searchParameters = new List<EmployeeInfo>();
        public void OnGet()
        {
       
            var dtEmployee = new DataTable(); //data table variable 
            dtEmployee = ReadExcel(@"C:\Users\mohit\Downloads\ms.csv");

            

            for (int i = 0; i < dtEmployee.Rows.Count; i++)
            {
                try
                {
                    if ((dtEmployee.Rows[i][1].ToString() != "") & (dtEmployee.Rows[i][6].ToString() != "") & (dtEmployee.Rows[i][5].ToString() != "") & (Convert.ToInt32(dtEmployee.Rows[i][2]) >= 21 & Convert.ToInt32(dtEmployee.Rows[i][2]) <= 60) & (Convert.ToDateTime(dtEmployee.Rows[i][3]) != null) & (Convert.ToDateTime(dtEmployee.Rows[i][4]) != null))
                    { 
                        searchParameters.Add(new EmployeeInfo
                    {
                        ID = Convert.ToInt32(dtEmployee.Rows[i][0]),
                        Name = dtEmployee.Rows[i][1].ToString(),
                        Age = Convert.ToInt32(dtEmployee.Rows[i][2]),
                        DOB = Convert.ToDateTime(dtEmployee.Rows[i][3]),
                        DOJ = Convert.ToDateTime(dtEmployee.Rows[i][4]),
                        City = dtEmployee.Rows[i][5].ToString(),
                        State = dtEmployee.Rows[i][6].ToString()
                    });
                         }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Exception : " + ex.Message);
                }
            }


            //tostring and convert 

        }

        private DataTable ReadExcel(string csvFileName)
        {
            WorkBook workbook = WorkBook.Load(csvFileName);
            WorkSheet sheet = workbook.DefaultWorkSheet;
            return sheet.ToDataTable(true);
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
}
