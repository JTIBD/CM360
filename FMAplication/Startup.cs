using System;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using FMAplication.Core;
using FMAplication.Domain;
using FMAplication.Filters;
using FMAplication.Helpers;
using FMAplication.HostedService;
using FMAplication.Hubs;
using FMAplication.Middlewares;
using FMAplication.Repositories;
using FMAplication.Services.Notification;
using FMAplication.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetCore.AutoRegisterDi;
using OfficeOpenXml;

namespace FMAplication
{
    public class Startup
    {
        private const string CorsPolicy = "CORSPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy(CorsPolicy, build =>
            {
                build.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }));

            services.Configure<ApplicationConfig>(Configuration.GetSection("ApplicationConfig"));
            Configuration.GetSection("AzureSettings").Get<AzureSettings>();
          

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString(nameof(ApplicationDbContext))));

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                .AddJwtBearer(cfg =>
                {
                    //cfg.Authority = "https://login.microsoftonline.com/0a495c7a-8a56-41c6-bf09-f14ccaf97dfe";
                    //cfg.Audience = "api://f0f7eef6-41ab-4c71-8b55-79cc2f1871fc"; // Set this to the App ID URL for the web API, which you created when you registered the web API with Azure AD.

                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:key"])),
                        ValidateIssuerSigningKey = false,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                    };


                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IHttpContextStorageContainer<>), typeof(HttpContextStorageContainer<>));
            services.AddScoped<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddScoped<IUserRequest, UserRequest>();

            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUnitOfWork, UnitOfWorks.UnitOfWork>();

            services.RegisterAssemblyPublicNonGenericClasses(Assembly.GetAssembly(typeof(Startup)))
                    .Where(c => c.Name.EndsWith("Repository"))
                    .AsPublicImplementedInterfaces();

            services.RegisterAssemblyPublicNonGenericClasses(Assembly.GetAssembly(typeof(Startup)))
                    .Where(c => c.Name.EndsWith("Service"))
                    .AsPublicImplementedInterfaces();

            //cache
            services.AddMemoryCache();

            //filter
            //services.AddScoped<AuthorizeFilter>();
            //services.AddScoped<ValidationFilter>();
            //services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(GlobalActionFilter));
            });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                //o.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });
            //services.AddSignalR();


            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers().AddNewtonsoftJson(options =>
                { 
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }
            );    // Enable Swagger   
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "JTI FM APPLICATION",
                    Description = "FM Web API v1"
                });
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}

                    }
                });
            });
            services.AddMvc(options => options.MaxModelValidationErrors = 999999);
            services.AddSignalR();


            services.AddCronJob<PosmLedgerCronJob>(c =>
            {
                var timeZoneInfo = TimeZoneInfo.Utc;
                try
                {
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
                }
                catch (Exception)
                {
                    Console.WriteLine("Cannot resolve Bangladesh time zone");
                }
                c.TimeZoneInfo = timeZoneInfo;
                c.CronExpression = @"1 0 * * *";
            });
        }


        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            HttpHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            app.UseCors(CorsPolicy);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FM Application V1");
                c.RoutePrefix = "fm";
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseRequestLocalization();
            app.UseMiddleware<ItemInHttpContextMiddleware>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");   
            });

            app.UseEndpoints(routes =>
            {
                routes.MapHub<NotificationHub>("/NotificationHub");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}

