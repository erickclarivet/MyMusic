using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyMusicMVC.Models;
using MyMusicMVC.ViewModels;
using Newtonsoft.Json;

namespace MyMusicMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        private string URLBase
        {
            get
            {
                return _config.GetSection("BaseURL").GetSection("URL").Value;
            }
        }

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            var listMusicViewModel = new ListMusicViewModel();
            var listMusic = new List<Music>();

            using (var httpclient = new HttpClient())
            {
                using (var response = await httpclient.GetAsync(URLBase + "Music"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    listMusic = JsonConvert.DeserializeObject<List<Music>>(apiResponse);
                }

                listMusicViewModel.ListMusic = listMusic;
            }

            return View(listMusicViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
