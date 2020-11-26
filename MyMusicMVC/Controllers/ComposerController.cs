using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyMusicMVC.Models;
using MyMusicMVC.ViewModels;
using Newtonsoft.Json;

namespace MyMusicMVC.Controllers
{
    public class ComposerController : Controller
    {
        private readonly ILogger<ComposerController> _logger;
        private readonly IConfiguration _config;

        private string URLBase
        {
            get
            {
                return _config.GetSection("BaseURL").GetSection("URL").Value;
            }
        }

        public ComposerController(ILogger<ComposerController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            var composerViewModel = new ListComposerViewModel();
            List<Composer> composerList = new List<Composer>();

            using (var httpclient = new HttpClient())
            {
                using (var response = await httpclient.GetAsync(URLBase + "Composer"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    composerList = JsonConvert.DeserializeObject<List<Composer>>(apiResponse);
                }

                composerViewModel.ListComposer = composerList;
            }

            return View(composerViewModel);
        }
    }
}
