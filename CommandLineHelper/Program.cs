using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CommandLineHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            //Run your commands here
            //Here is an example by running the ls command to list all files in the directory that the program is running
            Console.WriteLine(ExecCommand("ls"));
        }
        public static String ExecCommand(String command)
        {
            //First lets check what os we are using and based on that choose which file to execute and how to execute the command
            // as an arguement
            string shellFile;
            string args;
            //escape the arguements so that we can execute commands with arguments
            string escapedArgs = command.Replace("\"", "\\\"");
            //determine how to run the command based on the host OS
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                shellFile = "cmd.exe";
                args = $"/c \"{escapedArgs}\"";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                shellFile = "/bin/bash";
                args = $"-c \"{escapedArgs}\"";
            }
            else
            {
                //not a supported os, print message and exit
                Console.WriteLine("Error, Unsupported os, exiting");
                return "Error: Unsupported os";
            }
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = shellFile,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            //return the output of the task
            return result;
        }
    }
}
