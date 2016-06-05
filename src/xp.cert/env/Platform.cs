using System;
using System.IO;
using Xp.Cert;
using Mono.Unix.Native;

namespace Xp.Cert.Env
{
    static class Platform
    {
        public const string MACOSX  = "macosx";
        public const string WINDOWS = "windows";
        public const string CYGWIN  = "cygwin";
        public const string UNIX    = "unix";

        /// <summary>Returns uname.sysname</summary>
        private static string SysName()
        {
            var name = new Utsname();
            if (0 != Syscall.uname(out name)) return null;
            return name.sysname;
        }

        /// <summary>Detect OS platform</summary>
        public static string Current()
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
                var sysname = SysName();
                if (sysname == "Darwin")
                {
                    return MACOSX;
                }

                return UNIX;
            }
        }
    }
}