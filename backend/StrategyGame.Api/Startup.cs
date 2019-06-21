using AutoMapper;
using Hangfire;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.CodeGeneration.TypeScript;
using StrategyGame.Bll;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Services.Buildings;
using StrategyGame.Bll.Services.Commands;
using StrategyGame.Bll.Services.Country;
using StrategyGame.Bll.Services.Researches;
using StrategyGame.Bll.Services.TurnHandling;
using StrategyGame.Bll.Services.Units;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.IO;

namespace StrategyGame.Api
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
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            });

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<UnderSeaDatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<User>()
                .AddEntityFrameworkStores<UnderSeaDatabaseContext>();

            services.AddIdentityServer()
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerDocument(options =>
            {
                options.PostProcess = document =>
                {
                    var settings = new TypeScriptClientGeneratorSettings
                    {
                        ClassName = "{controller}Client"
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

            services.AddSingleton(new ModifierParserContainer(new AbstractEffectModifierParser[]
                {
                    new BarrackSpaceEffectParser(),
                    new CoralProductionEffectParser(),
                    new HarvestModifierEffectParser(),
                    new PopulationEffectParser(),
                    new TaxModifierEffectParser(),
                    new UnitDefenseEffectParser(),
                    new UnitAttackEffectParser()
                }));

            services.AddTransient<ITurnHandlingService, TurnHandlingService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IResearchService, ResearchService>();
            services.AddTransient<IBuildingService, BuildingService>();
            services.AddTransient<IUnitService, UnitService>();
            services.AddTransient<ICommandService, CommandService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseIdentityServer();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            app.UseMvc();
        }
    }
}
