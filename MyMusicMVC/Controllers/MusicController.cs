using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyMusicMVC.Models;
using MyMusicMVC.ViewModels;
using Newtonsoft.Json;

namespace MyMusicMVC.Controllers
{
    public class MusicController : Controller
    {
        private readonly ILogger<MusicController> _logger;
        private readonly IConfiguration _config;

        private string URLBase
        {
            get
            {
                return _config.GetSection("BaseURL").GetSection("URL").Value;
            }
        }

        public MusicController(ILogger<MusicController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddMusic()
        {
            var musicViewModel = new MusicViewModel();
            List<Artist> artistList = new List<Artist>();

            using (var httpclient = new HttpClient())
            {
                using (var response = await httpclient.GetAsync(URLBase + "Artist"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    artistList = JsonConvert.DeserializeObject<List<Artist>>(apiResponse);
                }

                musicViewModel.ArtistList = new SelectList(artistList, "Id", "Name");
            }

            return View(musicViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddMusic(MusicViewModel musicViewModel)
        {
            if (ModelState.IsValid)
            {
                // Send data to API with a string content
                using (var client = new HttpClient())
                {
                    var music = new Music() { IdArtist = int.Parse(musicViewModel.IdArtist), Name = musicViewModel.music.Name };
                    // Get the token and check if is not null
                    var jwToken = HttpContext.Session.GetString("token");
                    if (string.IsNullOrEmpty(jwToken))
                    {
                        ViewBag.MessageError = "You Must be authenticate";
                        return View(musicViewModel);
                    }
                    // if we have a token we add it in header
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwToken);

                        string stringData = JsonConvert.SerializeObject(music);
                        var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(URLBase + "Music", contentData);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ViewBag.MessageError = response.ReasonPhrase;
                }
            }
            return View(musicViewModel);
        }
    }
}
