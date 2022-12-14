using CsvHelper;
using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Controllers
{
    public class EmployeeController : Controller
    {
        public List<Employee> Employees = new List<Employee>();
        public async void LoadDatausingStreamReader()
        {
            try
            {
                //using var streamReader = File.OpenText([],@"C:\Users\mohit\Downloads\ms.csv");
                using (var streamReader = new StreamReader(@"C:\Users\mohit\Downloads\ms.csv"))
                {
                    using (var csvReader = new CsvReader(streamReader, false))
                    {
                        Employees = csvReader.GetRecords<Employee>().ToList();
                    }
                }
                foreach (var item in Employees)
                {
                    if ((item.Name != "") & (item.State != "") & (item.City != "") & (item.Age >= 21 & item.Age <= 60) & (item.DOB != null) & (item.DOJ != null))
                    {
                        var emp = new PostEmployee
                        {
                            //ID = 0,
                            Name = item.Name,
                            State = item.State,
                            City = item.City,
                            Age = item.Age,
                            DOB = item.DOB,
                            DOJ = item.DOJ,
                        };
                        var client = new HttpClient();
                        client.BaseAddress = new Uri(Baseurl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var payload = Newtonsoft.Json.JsonConvert.SerializeObject(emp);
                        var buffer = Encoding.UTF8.GetBytes(payload);
                        var bytes = new System.Net.Http.ByteArrayContent(buffer);
                        bytes.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = await client.PostAsync(
       "https://localhost:44372/api/employee/addemployee", bytes);
                        response.EnsureSuccessStatusCode();

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.ToString());
            }
        }


        string Baseurl = "https://localhost:44372/";
        // GET: EmployeesController
        public async Task<IActionResult> Index()
        {
            List<Employee> EmpInfo = new List<Employee>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Employee/GetAllEmployee");
                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    EmpInfo = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);
                }
                return View(EmpInfo);
            }
        }

        // GET: EmployeesController/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: EmployeesController/Create
        public ActionResult Create()
        {
            //LoadData();
            LoadDatausingStreamReader();
            //return View(new PostEmployee());
            return RedirectToAction(nameof(Index));
        }


        // POST: EmployeesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee emp)
        {
            try
            {

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(new Employee());
            }
        }

        // GET: EmployeesController/Edit/5
        public ActionResult Edit(int id)
        {

            return View();
        }

        // POST: EmployeesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Employee emp)
        {
            try
            {

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeesController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.DeleteAsync("api/Employee/deleteemployee/" + id.ToString());
            }
            //return RedirectToAction(nameof(Index));
            return View();
        }

        // POST: EmployeesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Employee emp)
        {
            try
            {

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}
