using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nito.AsyncEx;
using NSwag.CodeGeneration.TypeScript;
using StrategyGame.Api.Hubs;
using StrategyGame.Api.Middlewares;
using StrategyGame.Bll;
using StrategyGame.Bll.DTO.Received;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Services.Buildings;
using StrategyGame.Bll.Services.Commands;
using StrategyGame.Bll.Services.Country;
using StrategyGame.Bll.Services.Logger;
using StrategyGame.Bll.Services.Reports;
using StrategyGame.Bll.Services.Researches;
using StrategyGame.Bll.Services.TurnHandling;
using StrategyGame.Bll.Services.Units;
using StrategyGame.Bll.Services.UserTracker;
using StrategyGame.Bll.Services.Validators;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System.Collections.Generic;
using System.IO;

namespace StrategyGame.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PasswordHasherOptions>(options => options.IterationCount = 100000);
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            });

           // services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<UnderSeaDatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<User>()
                .AddEntityFrameworkStores<UnderSeaDatabaseContext>();

            services.AddIdentityServer()
                .AddCorsPolicyService<IdentityServerCorsPolicyService>()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources(Configuration))
                .AddInMemoryClients(IdentityServerConfig.GetClients(Configuration))
                .AddAspNetIdentity<User>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Authority"];
                    options.Audience = Configuration["ApiName"];
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name
                    };
                });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<PurchaseValidator>());

            services.AddSignalR();

            services.AddSwaggerDocument(options =>
            {
                options.Title = "Undersea API";
                options.Version = "1.0";
                options.Description = "API for the game called Under sea, which is a turn based online multiplayer strategy game.";

                options.PostProcess = document =>
                {
                    var settings = new TypeScriptClientGeneratorSettings
                    {
                        ClassName = "{controller}Client",
                        Template = TypeScriptTemplate.Axios
                    };

                    var generator = new TypeScriptClientGenerator(document, settings);
                    var code = generator.GenerateFile();

                    var path = Directory.GetCurrentDirectory();
                    var directory = new DirectoryInfo(path + @"\TypeScript");
                    if (!directory.Exists)
                    {
                        directory.Create();
                    }

                    var filePath = path + @"\TypeScript\Client.ts";
                    File.WriteAllText(filePath, code);
                };
            });

            services.AddAutoMapper(typeof(KnownValues).Assembly);

            services.AddSingleton(ModifierParserContainer.CreateDefault());
            services.AddSingleton<IUserTracker, UserTracker>();
            services.AddSingleton<AsyncReaderWriterLock>();

            services.AddTransient<ITurnHandlingService, TurnHandlingService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IResearchService, ResearchService>();
            services.AddTransient<IBuildingService, BuildingService>();
            services.AddTransient<IUnitService, UnitService>();
            services.AddTransient<ICommandService, CommandService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IDbLogger, DbLogger>();

            // User ID provider for SignalR Hub
            services.AddTransient<IUserIdProvider, UserIdProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<RequestLoggerMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseIdentityServer();

            app.UseOpenApi();
            app.UseSwaggerUi3();

           // app.UseHangfireServer();
           // app.UseHangfireDashboard();

            app.UseCors();
            app.UseMvc();

            app.UseSignalR(route => route.MapHub<UnderSeaHub>("/hub"));
            
            app.UseStaticFiles();

          //  RecurringJob.AddOrUpdate<TurnEndingJob>(x => x.EndTurnAsync(), Cron.Hourly);
        }
    }
}