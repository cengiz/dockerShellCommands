using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dockerShellCommands.Models;
using Microsoft.AspNetCore.Hosting;
using Medallion.Shell;

namespace dockerShellCommands.Controllers
{
    public class HomeController : Controller
    {
        private IApplicationLifetime ApplicationLifetime { get; set; }

        public HomeController(IApplicationLifetime appLifetime)
        {
            ApplicationLifetime = appLifetime;
        }

        public void ShutdownSite()
        {
            ApplicationLifetime.StopApplication();
            //return "Done";
        }

        public IActionResult Index()
        {
            //string res = RunSript("ls", "", "");
            //string rez = RunCommand("hostnamectl", string.Empty);
            //string res = Bash("git");
            ViewBag.Data = "";
            return View();
        }

        public string RunSript(string shellname, string workingDir, string arguments = "")
        {
            try
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = shellname,
                        Arguments = arguments,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        ErrorDialog = false,
                        WorkingDirectory = workingDir
                    }
                };

                process.Start();
                string result = process.StandardOutput.ReadToEnd();
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string Bash(string cmd, string param)
        {
            string result = "";
            var escapedArgs = cmd.Replace("\"", "\\\"");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    //FileName = "/bin/bash",
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    //Arguments = param,
                    RedirectStandardOutput = true,
                    WorkingDirectory = param,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            try
            {
                process.Start();
                result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                result = process.StandardError.ReadToEnd();
            }


            return result;
        }

        public IActionResult Command2(string cmd)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ShellModel shellModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            // Bash("cd /ldapwebservice");
            //string res = Bash(shellModel.cmd, shellModel.WorkingDirectory);
            // string res = RunCommand(shellModel.cmd, Convert.ToBoolean(shellModel.UseShellExecute), shellModel.WorkingDirectory);
            // string res = RunCommand(cmd, "");

            //string res = "";

            /*
            var command = Command.Run(shellModel.cmd, shellModel.WorkingDirectory);

          
            command.Wait(); // or...
            var result = command.Result; // or...
            result = await command.Task;

            // inspect the result
            if (!result.Success)
            {
               res = $"command failed with exit code {result.ExitCode}: {result.StandardError}";
            }
            else
            {
                res = result.StandardOutput;
            }*/

            
            //var res = ShellHelper.Bash(shellModel.cmd);
            var res = shellModel.cmd.Bash(shellModel.WorkingDirectory);
            res = res.Replace("\n", "<br>");
            ViewBag.Data = res;
            return View();
        }

        public static string BashCk(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        public string Bash2(string cmd, string args)
        {
            System.Diagnostics.ProcessStartInfo procStartInfo;
            procStartInfo = new System.Diagnostics.ProcessStartInfo("/bin/bash", "-c \""+ cmd+" " +args + " | grep -a 'dump f'\"");
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.RedirectStandardError = true;
            procStartInfo.UseShellExecute = false;

            procStartInfo.CreateNoWindow = true;

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            // Get the output into a string
            string output = proc.StandardOutput.ReadToEnd();
            string error = proc.StandardError.ReadToEnd();

            if (string.IsNullOrEmpty(error)) { return output; }
            else { return error; }
        }

        public string RunCommand(string command, bool shellExecute, string args)
        {
            try
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = command,
                        Arguments = args,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = shellExecute,
                        CreateNoWindow = true,
                        WorkingDirectory = args,
                        //FileName = "/bin/bash",
                        //Arguments = $"-c \"{escapedArgs}\"",
                    }
                };
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (string.IsNullOrEmpty(error)) { return output; }
                else { return error; }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
