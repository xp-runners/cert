using System;
using System.IO;
using Xp.Cert;

namespace Xp.Cert.Commands
{
    public partial class Update : Command
    {
        const string CYGWIN_BUNDLE_LOCATION = "/etc/pki/tls/certs/ca-bundle.crt";

        /// <summary>On Cygwin, symlink well-known bundle location</summary>
        public void Cygwin(FileInfo bundle)
        {
            if (bundle.Exists)
            {
                var target = CygwinEnvironment.ReadLink(bundle);
                if (target == CYGWIN_BUNDLE_LOCATION)
                {
                    // Nothing to do here
                    Console.WriteLine("> Link exists with up-to-date target {0}", target);
                }
                else if (null == target)
                {
                    CygwinEnvironment.ReplaceSymlink(bundle, CYGWIN_BUNDLE_LOCATION);
                    Console.WriteLine("> Replaced file with link to {1}", target, CYGWIN_BUNDLE_LOCATION);
                }
                else
                {
                    CygwinEnvironment.ReplaceSymlink(bundle, CYGWIN_BUNDLE_LOCATION);
                    Console.WriteLine("> Replaced outdated link target {0} with {1}", target, CYGWIN_BUNDLE_LOCATION);
                }
            }
            else
            {
                CygwinEnvironment.CreateSymlink(bundle, CYGWIN_BUNDLE_LOCATION);
                Console.WriteLine("> Linked {0} -> {1}", bundle, CYGWIN_BUNDLE_LOCATION);
            }

            Console.WriteLine("  {0} certificates", CountCertificates(
                CygwinEnvironment.Resolve(CYGWIN_BUNDLE_LOCATION)
            ));
            Console.WriteLine();
        }
    }
}