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
        public List<InvalidEmployee> InvalidEmployees = new List<InvalidEmployee>();
        readonly string Baseurl = "https://localhost:44372/";
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
                ViewBag.Invalid = InvalidEmployees;
                return View(EmpInfo);
            }
        }
        public IActionResult InvalidData()
        {
            try
            {
                using (var streamReader = new StreamReader(@"C:\Users\mohit\Downloads\ms.csv"))
                {
                    using (var csvReader = new CsvReader(streamReader, false))
                    {
                        Employees = csvReader.GetRecords<Employee>().ToList();

                    }
                }
                foreach (var item in Employees)
                {
                    InvalidEmployee emp = new InvalidEmployee();
                    if (!((item.Name != "") & (item.State != "") & (item.City != "") & (item.Age >= 21 & item.Age <= 60) & (item.DOB != null) & (item.DOJ != null)))
                    {
                        emp.ID = item.ID;
                        emp.Name = item.Name;
                        emp.State = item.State;
                        emp.City = item.City;
                        emp.Age = item.Age;
                        emp.DOB = item.DOB;
                        emp.DOJ = item.DOJ;
                        if (item.Name == "")
                        {
                            emp.InvalidString = "Name Cannot be empty";
                        }
                        if (item.City == "")
                        {
                            emp.InvalidString = "City Cannot be empty";
                        }
                        if (item.Age < 21)
                        {
                            emp.InvalidString = "Age is less then 21";
                        }
                        if (item.Age > 60)
                        {
                            emp.InvalidString = "Age is more then 60";
                        }
                        if (item.DOB == null)
                        {
                            emp.InvalidString = "DOB cannot be empty";
                        }
                        if (item.DOJ == null)
                        {
                            emp.InvalidString = "DOJ cannot be empty";
                        }
                        InvalidEmployees.Add(emp);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return View(InvalidEmployees);
        }
        public async Task<ActionResult> Details(int id)
        {
            Employee emp = new Employee();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Employee/searchemployeebyid/" + id.ToString());
                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    emp = JsonConvert.DeserializeObject<Employee>(EmpResponse);
                }
            }
            return View(emp);
        }
        public ActionResult Import()
        {
            LoadDatausingStreamReader();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Create()
        {
            return View(new PostEmployee());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PostEmployee emp)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/Employee/addemployee", emp);

                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                return View(new PostEmployee());
            }


        }
        public async Task<ActionResult> Edit(int id)
        {
            Employee emp = new Employee();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Employee/searchemployeebyid/" + id.ToString());
                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    emp = JsonConvert.DeserializeObject<Employee>(EmpResponse);
                }
            }

            return View(emp);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Employee emp)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var payload = Newtonsoft.Json.JsonConvert.SerializeObject(emp);
                    var buffer = Encoding.UTF8.GetBytes(payload);
                    var bytes = new System.Net.Http.ByteArrayContent(buffer);
                    bytes.Headers.ContentType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json");
                    HttpResponseMessage Res = await client.PutAsync("api/Employee/updateemployee/" + id.ToString(), bytes);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(emp);
            }
        }
        public async Task<ActionResult> Delete(int id)
        {
            Employee emp = new Employee();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Employee/searchemployeebyid/" + id.ToString());
                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    emp = JsonConvert.DeserializeObject<Employee>(EmpResponse);
                }
            }

            return View(emp);
         
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Employee emp)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = await client.DeleteAsync("api/Employee/deleteemployee/" + emp.ID.ToString());
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(emp);
            }
        }
    }
}
