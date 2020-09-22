using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using CodeDemo.API.DataValidation; 
using CodeDemo.API.DelegatingHandlers; 
using CodeDemo.API.ProcessPayment.DataValidation;
using CodeDemo.API.ProcessPayment.DataService;
using CodeDemo.API.AppDatabase.PaymentProcessRepository;
using AutoMapper;
using CodeDemo.API.AutoMapper; 
using Microsoft.OpenApi.Models;
using CodeDemo.API.Models;
using System.Text; 
using Microsoft.IdentityModel.Tokens;
using CodeDemo.API.AuthenticationService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Prometheus; 
using CodeDemo.API.Cryptography;
using CodeDemo.API.AppMetrics;
using Varghese_PaymentGateway.Filters.API;
using CodeDemo.API.AppDatabase.DatabaseContext;
using CodeDemo.API.AppDatabase.UsersRepository;

namespace CodeDemo.API
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// To read the key for authentication
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; 
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param> 
        public void ConfigureServices(IServiceCollection services)
        { 
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PaymentProcessProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
            services.AddSingleton<IProcessValidation, ProcessValidation>().
                     AddSingleton<IDataService, DataService>().
                     AddSingleton<IDataValidationControl, DataValidationControl>().
                     AddSingleton<IPaymentRepository, PaymentRepository>().
                     AddSingleton<IAuthService, AuthService>().
                     AddSingleton<ICryptography, Cryptographi>().
                     AddSingleton<IApplicationMetrics, ApplicationMetrics>().
                     AddSingleton<IRepositoryConnection, RepositoryConnection>().
                     AddSingleton<IUsersRepository, UsersRepository>();

            services.AddOcelot().AddDelegatingHandler<GatewayHandler>(true);  

            services.AddControllers();

            services.AddDataProtection();

            services.AddSwaggerGen(c =>
            { 
                c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Payment Gateway API",
                    Description = "Assignment",  
                    Contact = new OpenApiContact
                    {
                        Name = "Varghese Chandy",
                        Email = "varghese.chandy@hotmail.com"
                    } ,
                    
                }); 
                c.DocumentFilter<HideControllersFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT token with Bearer into field; For e.g. Value = Bearer 'Token string'",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                  });
                c.IncludeXmlComments(System.AppDomain.CurrentDomain.BaseDirectory + @"CodeDemo.API.xml"); 
            });

            //JWT Authentication
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Key); 

            services.AddAuthentication(au =>
            {
                au.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                au.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false 
                     
                };
            }); 
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param> 
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMetricServer("/metrics");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            app.UseSwagger();     
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Client From Varghese"); });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            await app.UseOcelot();
        }
    }
}
