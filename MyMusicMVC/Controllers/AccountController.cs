using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyMusicMVC.Models;
using MyMusicMVC.ViewModels;
using Newtonsoft.Json;

namespace MyMusicMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _config;

        private string URLBase
        {
            get
            {
                return _config.GetSection("BaseURL").GetSection("URL").Value;
            }
        }

        public AccountController(ILogger<AccountController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var user = new User();

                    user.UserName = loginViewModel.UserName;
                    user.Password = loginViewModel.Password;

                    var stringData = JsonConvert.SerializeObject(user);
                    var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(URLBase + "User/authenticate", contentData);

                    if (response.IsSuccessStatusCode)
                    {
                        // Get the token
                        string stringJWT = response.Content.ReadAsStringAsync().Result;
                        var jwt = JsonConvert.DeserializeObject<System.IdentityModel.Tokens.Jwt.JwtPayload>(stringJWT);
                        var jwtString = jwt["token"].ToString();

                        // Save in session
                        HttpContext.Session.SetString("token", jwtString); // username
                        HttpContext.Session.SetString("username", jwt["username"].ToString());

                        ViewBag.Message = "User logged in successfully ! " + jwt["username"].ToString();
                        //return RedirectToAction("Index", "Home");
                    }

                    ViewBag.Message = response.ReasonPhrase;
                }

            }
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    // TO DO : Use Maping
                    var user = new User();

                    user.FirstName = registerViewModel.FirstName;
                    user.LastName = registerViewModel.LastName;
                    user.UserName = registerViewModel.UserName;
                    user.Password = registerViewModel.Password;

                    var stringJson = JsonConvert.SerializeObject(user);
                    var stringData = new StringContent(stringJson, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(URLBase + "User/register", stringData);

                    if (response.IsSuccessStatusCode)
                    {
                        // Get the token
                        var stringJWT = response.Content.ReadAsStringAsync().Result;
                        var jwt = JsonConvert.DeserializeObject<System.IdentityModel.Tokens.Jwt.JwtPayload>(stringJWT);
                        var token = jwt["token"].ToString();
                        var username = jwt["username"].ToString();

                        // Save in session
                        HttpContext.Session.SetString("token", token);
                        HttpContext.Session.SetString("username", username);

                        ViewBag.Message = "Your account has been created and you are now connected " + username + " !";
                    }
                }
            }
            return View(registerViewModel);
        }

        public IActionResult Index()
        {
            var loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }
    }
}
