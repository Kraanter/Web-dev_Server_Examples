using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITestVoorToets;
using Microsoft.AspNetCore.Mvc;
using TestVoorToets.Models;

namespace TestVoorToets.Controllers
{
    public class ExternalController : Controller
    {
        private readonly HttpClient _httpClient;
        
        public ExternalController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<IActionResult> Index()
        {
            // Je kunt ook een Model aanmaken en deze vullen met de data die je terugkrijgt van de API:
            var model = new ExternalModel();

            try
            {
                var response = _httpClient.GetAsync("http://localhost:5229/WeatherForecast").Result;

                model.Status = response.StatusCode.ToString();
                model.Payload = response.Content.ReadAsStringAsync().Result;
                
                // Dit is een andere manier om de data uit de response te halen als je een Model hebt die overeenkomt met de data die je terugkrijgt:
                // Let op dat het terug gegeven object ook nullable is, dus je moet er rekening mee houden dat het null kan zijn.
                IEnumerable<WeatherForecast>? data = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
            }
            catch (Exception e)
            {
                model.Status = "Error";
                model.Payload = e.Message;
            }
           
            return View(model);
        }
    }
}