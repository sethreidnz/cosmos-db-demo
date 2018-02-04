using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CosmosDbDemo.Server.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CosmosDbDemo.Server
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();

      if (env.IsDevelopment())
      {
          builder.AddUserSecrets<Startup>();
      }

      Configuration = builder.Build();
      HostingEnvironment = env;
    }

    public IHostingEnvironment HostingEnvironment { get; private set; }

    public IConfigurationRoot Configuration { get; }

    public IContainer ApplicationContainer { get; private set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      // create CORS policy for development
      services.AddCors(options => options.AddPolicy(
              "AllowAll",
              p => p.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()));

      services.AddMvc();

      services.Configure<CosmosDbOptions>(Configuration.GetSection("CosmosDb"));

      var builder = new ContainerBuilder();
      builder.Populate(services);
      builder.RegisterModule(new AutofacModule());
      ApplicationContainer = builder.Build();
      return new AutofacServiceProvider(ApplicationContainer);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment())
      {
        app.UseCors("AllowAll");
      }

      app.UseMvc();
    }
  }
}
