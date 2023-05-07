using GenteFitOdoo;
using System.Diagnostics;
/*using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;*/

/*namespace MyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        *//*public static IHost InitApp(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .Build();
        }*//*

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
           .UseStartup<Startup>();
    }
}*/
// Creamos la app
var app = Startup.InitApp(args);

// Preparamos el inico de la API de Python
StartPythonApi();

// Ejecutamos la app
app.Run();

static void StartPythonApi()
{
    var process = new Process()
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = "API.py",
            WorkingDirectory = "./Resources/OddoRepositories/",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        }
    };

    process.OutputDataReceived += (sender, e) =>
    {
        Console.WriteLine(e.Data);
    };

    process.ErrorDataReceived += (sender, e) =>
    {
        Console.WriteLine(e.Data);
    };

    process.Start();
    process.BeginOutputReadLine();
    process.BeginErrorReadLine();
}