using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Xp.Cert;
using Mono.Unix;

namespace Xp.Cert.Commands
{
    public partial class Update : Command
    {
        private static string[] CA_BUNDLE_LOCATIONS = new string[] {
            "/etc/ssl/certs/ca-certificates.crt",     // Debian/Ubuntu/Gentoo etc.
            "/etc/pki/tls/certs/ca-bundle.crt",       // Fedora/RHEL
            "/etc/ssl/ca-bundle.pem",                 // OpenSUSE
            "/etc/pki/tls/cacert.pem",                // OpenELEC
            "/usr/local/share/certs/ca-root-nss.crt", // FreeBSD/DragonFly
            "/etc/ssl/cert.pem",                      // OpenBSD
            "/etc/openssl/certs/ca-certificates.crt"  // NetBSD
         };

        /// <summary>On Linux and Un*x systems, search locations, stopping at whichever 
        /// comes first. See https://golang.org/src/crypto/x509/root_linux.go</summary>
        public void Unix(FileInfo bundle)
        {
            var location = CA_BUNDLE_LOCATIONS.FirstOrDefault(File.Exists);
            if (null == location)
            {
                throw new NotSupportedException(string.Format(
                    "Cannot find bundle in any of [{1}  {0}{1}]",
                    string.Join(Environment.NewLine + "  ", CA_BUNDLE_LOCATIONS),
                    Environment.NewLine
                ));
            }

            if ((bundle.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
            {
                var link = new UnixSymbolicLinkInfo(bundle.FullName).GetContents().FullName;
                if (link == location)
                {
                    // Nothing to do here
                    Console.WriteLine("> Link exists with up-to-date target {0}", link);
                }
                else
                {
                    bundle.Delete();
                    (new UnixFileInfo(location)).CreateSymbolicLink(bundle.FullName);
                    Console.WriteLine("> Replaced link target {0} with {1}", link, location);
                }
            }
            else if (bundle.Exists)
            {
                bundle.Delete();
                (new UnixFileInfo(location)).CreateSymbolicLink(bundle.FullName);
                Console.WriteLine("> Replaced file with link to {0}", location);
            }
            else
            {
                (new UnixFileInfo(location)).CreateSymbolicLink(bundle.FullName);
                Console.WriteLine("> Linked {0} -> {1}", bundle.Name, location);
            }

            Console.WriteLine("  {0} certificates", CountCertificates(bundle));
            Console.WriteLine();
        }
    }
}