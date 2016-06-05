using System;
using System.IO;
using System.Diagnostics;
using Xp.Cert;

namespace Xp.Cert.Commands
{
    public partial class Update : Command
    {
        const string SECURITY_EXECUTABLE = "/usr/bin/security";
        const string SECURITY_ARGUMENTS  = "find-certificate -a -p";
        const string SECURITY_KEYCHAIN   = "/System/Library/Keychains/SystemRootCertificates.keychain";

        /// <summary>Execute this command</summary>
        public void MacOSX(FileInfo bundle)
        {
            var proc = new Process();
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.FileName = SECURITY_EXECUTABLE;
            proc.StartInfo.Arguments = SECURITY_ARGUMENTS + " " + SECURITY_KEYCHAIN;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = false;

            try {
                Console.Write("> From {0}: [", SECURITY_KEYCHAIN);
                proc.Start();

                using (var writer = new StreamWriter(bundle.Open(FileMode.Create)))
                {
                    var count = 0;
                    proc.OutputDataReceived += (sender, e) => {
                      if (e.Data.StartsWith(BEGIN_CERT))
                      {
                          count++;
                          Console.Write('.');
                      }
                      writer.WriteLine(e.Data);
                    };

                    proc.BeginOutputReadLine();
                    proc.WaitForExit();

                    Console.WriteLine("]");
                    Console.WriteLine("  {0} certificates", count);
                    Console.WriteLine();
                }
            }
            finally
            {
                proc.Close();
            }
        }
    }
}