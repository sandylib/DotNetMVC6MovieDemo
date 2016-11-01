using AutoMapper;
using DotNetCoreDemo.Helper;
using DotNetCoreDemo.Services;
using DotNetCoreDemo.ViewModal;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoviesLibrary;
using Newtonsoft.Json.Serialization;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace DotNetCoreDemo
{
    public class Startup
    {
       public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .AddEnvironmentVariables();

           
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<IMapper, Mapper>();
            services.AddCaching();
            services.AddMvc()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ContractResolver =
                            new CamelCasePropertyNamesContractResolver();
                    });
            
            services.AddInstance(Configuration);
            services.AddSingleton<MovieDataSource, MovieDataSource>();
            services.AddSingleton<IDataRepository, DataRepository>();
            services.AddSingleton<CacheHelper, CacheHelper>();


        }

       
        public void Configure(IApplicationBuilder app, 
                                IHostingEnvironment env, 
                                ILoggerFactory loggerFactory,
                                IMemoryCache memoryCache)
        {
            
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            Mapper.Initialize(config =>
            {
                config.CreateMap<MovieData, MovieDataViewModal>().ReverseMap();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseFileServer();

            app.UseIISPlatformHandler();

            app.UseMvc();

        }
        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
