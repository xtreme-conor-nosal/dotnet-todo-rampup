using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TodoApi.Models;

namespace TodoApi
{
    public class TestStartup : Startup
    {
        
        protected override void ReadConfiguration(IHostingEnvironment env)
        {
            
        }

        protected override void ConfigureEntityFramework(IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("Test"));
        }
    }
}