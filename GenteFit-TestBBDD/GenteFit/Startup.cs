using GenteFit.Models.Repositories;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace GenteFit
{
    public static class Startup
    {
        public static WebApplication InitApp(string[] args)
        {
            // Permite configurar los servicios
            var builder = WebApplication.CreateBuilder(args); 

            // Llamamos al método para configurar los servicios
            ConfigureServices(builder);

            // Construimos la app -> Sobre ella configuraremos los Middlewares
            var app = builder.Build();

            // Llamamos al método para configurar los Middlewares
            Configure(app);

            // Retornamos la app configurada
            return app;
        }
        
        // Métodos de configuración de los servicios
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddMvc();
            // Añadimos swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "GenteFit", 
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Grupo Hyperion",
                        Email = "",
                        Url = new Uri("https://github.com/DanielBoj/UOC-Persistencia-con-NET")
                    },
                    Description = "APP para la gestión de centros de fitness"
                });
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddEndpointsApiExplorer();

            // Inyección de los valores de configuración: Las clase DataConnectionSettings irá a buscar los valores
            // al archivo de configuración.
            builder.Services.Configure<DataConnectionSettings>(builder.Configuration.GetSection("GenteFitDataBase"));
            builder.Services.AddSingleton<DataConnection>();
        }

        // Configuramos los Middlewares
        private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // Configuramos una trazabilidad total para las excepciones que se den en entornos de desarrollo
                DeveloperExceptionPageOptions d = new()
                {
                    SourceCodeLineCount = 1,
                };

                app.UseDeveloperExceptionPage(d);      
            } else if (app.Environment.IsProduction()) {

                // Si la excepción se produce en un entorno de producción, mostramos una página de error
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Mapeamos los endpoint para swagger y añadimos swagger  a la app.
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "GenteFit V1");
            });

            // Hasta que no tengamos un certificado SSL no podemos usar el protocolo HTTPS
            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
