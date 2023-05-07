﻿using GenteFit.Models.Repositories;
using GenteFitOdoo.Models.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.Diagnostics;

namespace GenteFitOdoo
{
    /* Esta clase permite crear la configuración y los servicios que implementará nuestro backend. 
     La usamso tambi;en para especificar la conexión a la BD y para crear la relación entre el frontend y 
    el backend cuando generemos nuestra aplicación en producción. En un entorno de desarrollo creamos las dos
    conexiones por separado, usamos dotnet run par ejecutar el backend y ng serve para ejecutar el frontende,
    igualmente, la conexión entre ambar también se establece en esta clase. */
    public class Startup
    {
        /*public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }*/

        public static WebApplication InitApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var startUp = new Startup();

            // Configuramos los servicios
            startUp.ConfigureServices(builder.Services, builder.Configuration);

            // Construimos la app -> Sobre ella configuraremos los Middlewares
            var app = builder.Build();

            // Llamamos al método para configurar los Middlewares
            startUp.Configure(app);

            // Retornamos la app configurada
            return app;
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Configuración de MongoDB
            services.Configure<DataConnectionSettings>(configuration.GetSection("GenteFitDataBase"));
            services.AddSingleton<DataConnection>();

            // Configuración de servicios de la aplicación
            services.AddControllersWithViews();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyApp API", Version = "v1" });
            });

            // Configuración de CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // Configuración de la API de Python
            services.AddScoped<HttpClient>( s =>
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:5005");
                return client;
            });
            /*services.Configure<PythonApiConnection>(configuration.GetSection("PythonApi"));
            services.AddControllers();
            services.AddSingleton<PythonApiConnection>();*/

            // Configuración de la capa de negocio
            // services.AddScoped<IMyService, MyService>();
        }

        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "GenteFit V1");
                });
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Lanzamos la API de Python al iniciar el servidor.
            /*var processInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "./Resources/OddoRepositories/API.py",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            var pythonApi = new Process { StartInfo = processInfo };
            pythonApi.Start();
            pythonApi.WaitForExit();*/
        }
    }
}