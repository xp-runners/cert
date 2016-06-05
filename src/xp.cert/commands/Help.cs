using System;
using Xp.Cert;

namespace Xp.Cert.Commands
{
    public class Help : Command
    {
        /// <summary>Execute this command</summary>
        public int Execute(string[] args)
        {
            Console.WriteLine("Certificate management");
            Console.WriteLine("════════════════════════════════════════════════════════════════════════");
            Console.WriteLine();
            Console.WriteLine("> Update certificates from operating system environment");
            Console.WriteLine();
            Console.WriteLine("  cert up");
            Console.WriteLine();

            return 1;
        }
    }
}