using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Xp.Cert;

namespace Xp.Cert.Commands
{
    public partial class Update : Command
    {

        /// <summary>Export a given certificate</summary>
      private void ExportToPEM(X509Certificate cert, TextWriter target)
      {
          target.WriteLine(BEGIN_CERT);
          target.WriteLine(Convert.ToBase64String(
              cert.Export(X509ContentType.Cert),
              Base64FormattingOptions.InsertLineBreaks)
          );
          target.WriteLine(END_CERT);
      }

        /// <summary>Export a given certificate store</summary>
      private void ExportStore(StoreName name, TextWriter target)
      {
          var store = new X509Store(name, StoreLocation.CurrentUser);
          store.Open(OpenFlags.OpenExistingOnly);

          Console.Write("> From {0}: [", name);

          foreach (var cert in store.Certificates)
          {
              ExportToPEM(cert, target);
              Console.Write('.');
          }

          Console.WriteLine("]");
          Console.WriteLine("  {0} certificates", store.Certificates.Count);
          Console.WriteLine();
          store.Close();
      }

        /// <summary>On Windows, export certificate store to</summary>
        public void Windows(FileInfo bundle)
        {
            using (var writer = new StreamWriter(bundle.Open(FileMode.Create)))
            {
                ExportStore(StoreName.Root, writer);                  // trusted root certificate authorities
                ExportStore(StoreName.AuthRoot, writer);              // third-party certificate authorities
                ExportStore(StoreName.CertificateAuthority, writer);  // intermediate certificate authorities
            }
        }
    }
}