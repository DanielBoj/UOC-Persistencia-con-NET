using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GenteFit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OdooController : ControllerBase
    {
        [HttpPost]
        public IActionResult EnvioPythonScript(int op)
        {
            var psi = new ProcessStartInfo();
            psi.FileName = @"";
            var script = "";

            switch (op)
            {
                case 0:
                    script = @"test.py";
                    break;
                case 1:
                    break;
            }

            psi.Arguments = $"\"{script}\"";
            Process process = new Process();
            process.StartInfo = psi;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            process.StartInfo.RedirectStandardOutput = true;
            string output = "";

            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                output += line;
            }

            return Ok(new { message = output });
        }
    }
}