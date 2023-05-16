using GenteFit.Models.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace GenteFitBackup
{
    /* El archivo Startup es un archivo de configuración que se ejecuta al inicio de la aplicación.
    * En este archivo se configuran los servicios que se van a utilizar en la aplicación.
    * En este caso, se configura el servicio de Swagger para documentar la API. */
    public static class Startup
    {
        public static WebApplication InitApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();
            Configure(app);

            return app;
        }

        private static void ConfigureServices(WebApplicationBuilder builder) {
            builder.Services.AddCors();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.WebHost.UseKestrel();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GenteFit API", Version = "v1" });
            });

            // Configuración de la API de Python
            builder.Services.AddScoped<HttpClient>(s =>
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:5005")
                };
                return client;
            });

            // Configuración de Mongo
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
            builder.Services.AddScoped<DataConnection>();
            builder.Services.AddSingleton<DataConnection>();
        }
        public static void Configure(WebApplication app) { 
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json",
                                               "GenteFit API v1");
                });
            }
            app.UseCors(options =>
            {
                options.WithOrigins("http://localhost:4200", "https://danielboj.github.io/UOC-Persistencia-Angular");
                options.AllowAnyHeader();
                options.AllowAnyMethod();
            } );
            app.UseRouting();
            app.MapControllers();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
