using GenteFit.Models.Repositories;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace GenteFit
{
    /* Esta clase permite crear la configuración y los servicios que implementará nuestro backend. 
     La usamso tambi;en para especificar la conexión a la BD y para crear la relación entre el frontend y 
    el backend cuando generemos nuestra aplicación en producción. En un entorno de desarrollo creamos las dos
    conexiones por separado, usamos dotnet run par ejecutar el backend y ng serve para ejecutar el frontende,
    igualmente, la conexión entre ambar también se establece en esta clase. */  
    public class Startup
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

        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddMvc();

            // Conectamos con la app de Angular
            builder.Services.AddCors(options =>
            {
                // Definimos una política de seguridad que permita cualquier origen, método y cabecera
                options.AddPolicy("AllowAll", builder =>
                builder.AllowAnyOrigin().
                AllowAnyMethod().
                AllowAnyHeader());
            });


            // Añadimos swagger que nos permitirá realizar peticiones a la API para testarla
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
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

            // Este servicio permite que los controladores de la app devuelvan vistas Razor, lo dejo
            // porque lo usamos en la app de test del producto anterior.
            builder.Services.AddControllersWithViews();

            // AddEndPoints sirve para que swagger o la app de frontend puedan encontrar los endpoints de la API
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
                // Mediante CORS habilitamos el envío de las peticiones desde el front-end
                app.UseCors(builder => builder.WithOrigins("*")
                    .AllowAnyMethod()
                    .AllowAnyHeader());

                // Si la excepción se produce en un entorno de producción, mostramos una página de error
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            } else
            {
                // Mapeamos los endpoint para swagger y añadimos swagger  a la app.
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "GenteFit V1");
                });

                // Configuramos una trazabilidad total para las excepciones que se den en entornos de desarrollo
                DeveloperExceptionPageOptions d = new()
                {
                    SourceCodeLineCount = 1,
                };

                app.UseDeveloperExceptionPage(d);
            }

            // Permitimos que el front-end pueda acceder a la API
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            // Habilitamos cualquier tipo de petición a nuestra ApiRest
            app.UseCors("AllowAll");

            // Configuramos los endpoints para que el frontend pueda acceder al backend
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // Declaramos el entry point de la app de Angular
                endpoints.MapFallbackToFile("ClientApp/dist/index.html");
            });


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("ClientApp/dist/index.html"); ;
        }


    }
}
