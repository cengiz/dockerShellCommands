using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public static class ShellHelper
{
    public static string Bash(this string cmd, string _WorkingDirectory)
    {
        var escapedArgs = cmd.Replace("\"", "\\\"");
        string eOut = null;
        string result = "";
        try
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = _WorkingDirectory
                }
            };
            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
            { eOut += e.Data; });

            process.Start();
            string standart = process.StandardOutput.ReadToEnd();
            //string error = process.StandardError.ReadToEnd();
           
            process.WaitForExit();

            StreamReader reader = process.StandardError;
            string error_output = reader.ReadToEnd();

            if (!string.IsNullOrEmpty(eOut))
            {
                result = eOut;
            }
            if (!string.IsNullOrEmpty(error_output))
            {
                result = error_output;
            }
            else
            {
                result = standart;
            }

            return result;
        }
        catch (Exception e)
        {
            return "Aplication error : " + e.Message;
        }
    }
}