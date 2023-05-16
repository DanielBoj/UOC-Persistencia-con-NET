using Docker.DotNet;
using Docker.DotNet.Models;
using GenteFitBackup;
using System.Diagnostics;

var app = Startup.InitApp(args);
// Iniciamos el contenedor de Odoo
StartOdooContainer();

// Preparamos el inico de la API de Python
InstallPythonDependencies();
StartPythonApi();

app.Run();

static void StartOdooContainer()
{
    // Ejecutamos el contenedor con Odoo
    DockerClient client = new DockerClientConfiguration(
    new Uri("npipe://./pipe/docker_engine"))
     .CreateClient();

    // Ejecutar el comando "docker-compose up" para iniciar el contenedor de Odoo
    var process = new System.Diagnostics.Process()
    {
        StartInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker-compose",
            Arguments = "up -d",
            WorkingDirectory = "./"
        }
    };
    process.Start();
    process.WaitForExit();
}

static void InstallPythonDependencies()
{
    var process = new Process()
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = $"-m pip install -r requirements.txt",
            WorkingDirectory = "./Resources/OddoRepositories/PythonLibs/",
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

    process.WaitForExit();
}

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