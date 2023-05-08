using System;
using System.Diagnostics;
using System.IO;

namespace Webrick.Test.CommandLine
{
    internal class Program
    {
        public static void MakeSureChromeDriveDoesNotExist()
        {
            try
            {
                Process[] chromeDriverProcesses = Process.GetProcessesByName("chromedriver");
                if (chromeDriverProcesses != null)
                {
                    foreach (Process chromeDriver in chromeDriverProcesses)
                    {
                        chromeDriver.Kill();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        static void Main(string[] args)
        {
            string path = Path.GetFullPath(Path.Combine(".", "lastRun.txt"));
            Console.Out.WriteLine(path);
            File.WriteAllText(path, DateTime.Now.ToString());
            MakeSureChromeDriveDoesNotExist();
        }
    }
}
