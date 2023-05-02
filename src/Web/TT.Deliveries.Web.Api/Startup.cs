namespace TT.Deliveries.Web.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using TT.Deliveries.Data;
    using TT.Deliveries.Services;
    using TT.Deliveries.Services.Contracts;
    using AutoMapper;
    using System.Text.Json.Serialization;
    using System;
    using TT.Deliveries.Core.Options;
    using TT.Deliveries.Web.Api.Extensions;
    using TT.Deliveries.Auth.Handlers;
    using TT.Deliveries.Auth.Services.Contracts;
    using TT.Deliveries.Auth.Services;
    using System.Reflection;
    using System.IO;

    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Banner.WriteBanner();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TT.Deliveries.Web.Api", Version = "v1" });
                c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authentication header using the Basic scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Basic"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // Include Controller XML Comments in Swagger Doc using reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddDbContext<DeliveryDbContext>(options =>
                options.UseSqlite(Environment.ExpandEnvironmentVariables(Configuration.GetConnectionString("DeliveryDatabase")))
            );

            services.AddAutoMapper(
                x => x.AddProfile<Maps>()
            );
            services.Configure<BasicAuthSchemeOptions>(Configuration.GetSection("Auth"));

            var authOptions = Configuration.GetSection("Auth").Get<BasicAuthSchemeOptions>();
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<BasicAuthSchemeOptions, BasicAuthenticationHandler> 
                ("BasicAuthentication", x => x.Realm = authOptions.Realm);

            services.AddTransient<IDeliveryService,DeliveryService>();
            services.AddTransient<IStateService, StateService>();
            services.AddTransient<IAuthService, AuthService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TT.Deliveries.Web.Api v1"));

                using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
                var context = serviceScope.ServiceProvider.GetRequiredService<DeliveryDbContext>();
                context.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(e => e.MapControllers());
        }
    }
}
