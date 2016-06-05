using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Xp.Cert;
using Xp.Cert.Env;

namespace Xp.Cert.Commands
{
    public partial class Update : Command
    {
        const string BUNDLE = "ca-bundle.crt";

        const string BEGIN_CERT = "-----BEGIN CERTIFICATE-----";
        const string END_CERT = "-----END CERTIFICATE-----";

        private static Dictionary<string, Action<Update, FileInfo>> update = new Dictionary<string, Action<Update, FileInfo>>()
        {
            { Platform.MACOSX, (self, bundle) => self.MacOSX(bundle) },
            { Platform.WINDOWS, (self, bundle) => self.Windows(bundle) },
            { Platform.CYGWIN, (self, bundle) => self.Cygwin(bundle) },
            { Platform.UNIX, (self, bundle) => self.Unix(bundle) }
        };

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
                platform = Platform.Current();
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