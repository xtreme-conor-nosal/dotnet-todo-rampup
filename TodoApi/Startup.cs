﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Logging.CloudFoundry;
using TodoApi.Controllers;
using TodoApi.Models;

namespace TodoApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; } 
        
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            ReadConfiguration(env);
            AddLoggingProviders(loggerFactory);
        }

        protected virtual void AddLoggingProviders(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddCloudFoundry(Configuration.GetSection("Logging"));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureEntityFramework(services);
            services.AddMvc();
            services.AddTransient(typeof(TodoService));
            services.AddTransient(typeof(ITodoContext), typeof(TodoContext));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
        }

        protected virtual void ReadConfiguration(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
           
            Configuration = builder.Build();
        }

        protected virtual void ConfigureEntityFramework(IServiceCollection services)
        {
            
            services.AddDbContext<TodoContext>(opt =>
            {
                var hostname = $@"Server={Configuration["DB_HOST"]}";
                var name = $@"database={Configuration["DB_NAME"]}";
                var username = $@"uid={Configuration["DB_USER"]}";
                var password = Configuration["DB_PASS"] == null ? "" : $@"pwd={Configuration["DB_PASS"]}";
                var connection = $@"{hostname};{name};{username};{password};";
                opt.UseMySql($@"{connection}");
            });
        }
    }
}