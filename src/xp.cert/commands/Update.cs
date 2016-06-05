using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Xp.Cert;

namespace Xp.Cert.Commands
{
    public partial class Update : Command
    {
        const string BUNDLE = "ca-bundle.crt";

        const string MACOSX = "macosx";
        const string WINDOWS = "windows";
        const string CYGWIN = "cygwin";
        const string UNIX = "unix";

        const string BEGIN_CERT = "-----BEGIN CERTIFICATE-----";
        const string END_CERT = "-----END CERTIFICATE-----";

        private static Dictionary<string, Action<Update, FileInfo>> update = new Dictionary<string, Action<Update, FileInfo>>()
        {
            { MACOSX, (self, bundle) => self.MacOSX(bundle) },
            { WINDOWS, (self, bundle) => self.Windows(bundle) },
            { CYGWIN, (self, bundle) => self.Cygwin(bundle) },
            { UNIX, (self, bundle) => self.Unix(bundle) }
        };

        /// <summary>Detect OS platform</summary>
        private string CurrentPlatform()
        {
            if (Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                return MACOSX;
            }
            else if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                return CygwinEnvironment.Active ? CYGWIN : WINDOWS;
            }
            else
            {
                return UNIX;
            }
        }

        /// <summary>Count certificates in a given bundle</summary>
        protected int CountCertificates(FileInfo bundle)
        {
            using (var reader = new StreamReader(bundle.OpenRead()))
            {
                int count = 0;
                while (reader.Peek() >= 0)
                {
                    if (reader.ReadLine().StartsWith(BEGIN_CERT))
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        /// <summary>Execute this command</summary>
        public int Execute(string[] args)
        {
            string platform;

            if (args.Length > 0)
            {
                platform = args[0].ToLower();
                Console.WriteLine("@{0} (specified via command line)", platform);
            }
            else
            {
                platform = CurrentPlatform();
                Console.WriteLine("@{0} (detected)", platform);
            }

            Console.WriteLine("Updating certificates");
            Console.WriteLine();
            try
            {
                var bundle = new FileInfo(BUNDLE);
                update[platform](this, bundle);
                Console.WriteLine("Done, {0} updated", bundle.FullName);
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 1;                
            }

        }
    }
}