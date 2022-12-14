using CsvHelper;
using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        string Baseurl = "https://localhost:44372/";
        public async Task<ActionResult> Index()
        {
            List<Employee> EmpInfo = new List<Employee>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Employee/GetAllEmployee");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    EmpInfo = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);
                }
                //returning the employee list to view
                return View(EmpInfo);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
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
                        HttpResponseMessage response = await client.PostAsJsonAsync(
       "https://localhost:44372/api/employee/addemployee", emp);
                        response.EnsureSuccessStatusCode();


                    }

                }
                //_context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.ToString());
            }
        }

        public ActionResult Create()
        {
            //LoadData();
            LoadDatausingStreamReader();
            //return View(new PostEmployee());
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        public async Task<ActionResult> EditAsync(int id)
        {
            Employee EmpInfo = new Employee();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:44372/api/Employee/searchemployeebyid/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    EmpInfo = JsonConvert.DeserializeObject<Employee>(EmpResponse);
                }
                return View(EmpInfo);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Employee emp)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    return RedirectToAction(nameof(Index));
                    var payload = Newtonsoft.Json.JsonConvert.SerializeObject(emp);
                    var buffer = Encoding.UTF8.GetBytes(payload);
                    var bytes = new System.Net.Http.ByteArrayContent(buffer);
                    bytes.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    HttpResponseMessage Res = await client.PutAsync("api/Employee/updateemployee/" + id.ToString(), bytes).ConfigureAwait(false);
                }
            }
            catch
            {
                return View();
            }
        }




        public async Task<ActionResult> DeleteAsync(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.DeleteAsync("api/Employee/deleteemployee/"+ id.ToString());
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
