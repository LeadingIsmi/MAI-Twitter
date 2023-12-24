using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Nest;

namespace Twitter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Получение строки подключения к MongoDB из конфигурации
            var connectionString = Configuration.GetConnectionString("MongoDB");
            // Регистрация IMongoClient и передача ему строки подключения
            services.AddSingleton(new MongoClient(connectionString));
            // Регистрация сервиса PostService
            services.AddScoped<IPostService, PostService>();
            // Регистрация сервиса UserService
            services.AddScoped<IUserService, UserService>();
            // Другие сервисы и настройки
            services.AddControllers();



            services.Configure<ElasticsearchOptions>(Configuration.GetSection("Elasticsearch"));

            var elasticsearchConfig = Configuration.GetSection("Elasticsearch").Get<ElasticsearchOptions>();

            var settings = new ConnectionSettings(new Uri(elasticsearchConfig.Url))
                .DefaultIndex(elasticsearchConfig.DefaultIndex);

            var elasticClient = new ElasticClient(settings);

            services.AddSingleton<IElasticsearchService>(new ElasticsearchService(elasticClient));
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
            app.UseAuthorization();

            /*
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            */

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello world!\n");
 
                });
            });


        }
    }
}
