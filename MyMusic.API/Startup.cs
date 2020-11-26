using AutoMapper;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MyMusic.Core.Models;
using MyMusic.Core.Repositories;
using MyMusic.Core.Services;
using MyMusic.Data;
using MyMusic.Data.MogoDB.Repositories;
using MyMusic.Data.MogoDB.Settings;
using MyMusic.Services.Services;
using System.Text;
using System.Threading.Tasks;

// REMEMBER :
// services.AddTransient : EveryTime we see IMusicService an new instance of MusicService is created
// services.AddScoped : Every http call an new instance of ComposerRepository is created
// services.AddSingleton : Only the first call a new instance of MongoClient is created

namespace MyMusic.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // Configuration for SQL Server
            services.AddDbContext<MyMusicDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("MyMusic.Data")));
            // For every http req, we create insance of UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Configuration for MongoDB
            services.Configure<Settings>(
                options =>
                {
                    options.ConnectionString = Configuration.GetValue<string>("MongoDB:ConnectionString");
                    options.Database = Configuration.GetValue<string>("MongoDB:Database");
                });
            services.AddSingleton<IMongoClient, MongoClient>(
                p => new MongoClient(Configuration.GetValue<string>("MongoDB:ConnectionString")));
            // For every http req, we create a new instance of composerRepository
            services.AddScoped<IComposerRepository, ComposerRepository>();
            services.AddTransient<IDatabaseSettings, DatabaseSettings>();

            // Services
            services.AddTransient<IMusicService, MusicService>();
            services.AddTransient<IArtistService, ArtistService>();
            services.AddTransient<IComposerService, ComposerService>();

            // Swagger to test the API
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Put title here",
                    Description = "DotNet Core Api 3 - with swagger"
                });
            });

            // Automapper
            services.AddAutoMapper(typeof(Startup));

            // Jwt
            services.AddTransient<IUserService, UserService>();
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AppSettings:SecretKey"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        // Everytime we access to IUserService, if 
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            context.Fail("Unautorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Music V1");
            });
        }
    }
}
