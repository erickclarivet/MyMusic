using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyMusic.API.Resources;
using MyMusic.API.Validation;
using MyMusic.Core.Models;
using MyMusic.Core.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserController(IUserService userService, IMapper mapper, IConfiguration config)
        {
            _userService = userService;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<UserResource>> Authenticate(UserResource userResource)
        {
            var user = await _userService.Authenticate(userResource.UserName, userResource.Password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("AppSettings:SecretKey")); // Not optimal

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResource>> Register(UserResource userResource)
        {
            // Validation
            var validation = new SaveUserResourceValidation();
            var validationResult = await validation.ValidateAsync(userResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            // Mappage
            var user = _mapper.Map<UserResource, User>(userResource);
            var userSave = await _userService.Create(user, userResource.Password);

            // Send Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("AppSettings:SecretKey")); // Not optimal

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }
    }
}
