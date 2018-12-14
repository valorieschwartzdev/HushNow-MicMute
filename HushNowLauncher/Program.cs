using System.Diagnostics;
using System.IO;

namespace HushNowLauncher
{
    /*
     * The purpose of using the launcher is that it enables the task icon (on the bottom Windows bar) to be changed at runtime.
     */
    
    internal static class Program
    {
        public static void Main()
        {
            var path = Path.GetFullPath("./HushNow.exe");
            Process.Start(path);
        }
    }
}