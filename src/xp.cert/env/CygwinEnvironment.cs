using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Xp.Cert.Env
{
    static class CygwinEnvironment
    {
        private const string CYGDRIVE_PATH = "/cygdrive/";
        private const string INSTALLATIONS = @"Software\Cygwin\Installations";

        private static byte[] SYMLINK_COOKIE = new byte[] { 33, 60, 115, 121, 109, 108, 105, 110, 107, 62 };

        /// <summary>Determine whether we're runnning inside Cygwin</summary>
        public static bool Active { get { return null != Environment.GetEnvironmentVariable("SHELL"); } }

        /// <summary>Determine Cygwin installation directories. Caches information</summary>
        public static IEnumerable<string> Installations()
        {
            var installed = Registry.CurrentUser.OpenSubKey(INSTALLATIONS) ?? Registry.LocalMachine.OpenSubKey(INSTALLATIONS);
            if (null == installed)
            {
                throw new NotSupportedException("Cannot determine Cygwin path via registry [" + INSTALLATIONS + "]");
            }

            return installed.GetValueNames()
                .Select(key => installed.GetValue(key) as string)
                .Select(path => path.Replace(@"\??\", "").TrimEnd(Path.DirectorySeparatorChar))
            ;
        }

        /// <summary>Resolve directory. Supports /cygdrive and absolute paths</summary>
        public static FileInfo Resolve(string path)
        {
            if (path.StartsWith(CYGDRIVE_PATH))
            {
                return new FileInfo(path[CYGDRIVE_PATH.Length] + ":" + path.Substring(CYGDRIVE_PATH.Length + 1));
            }

            var absolute = path.Replace("/", Path.DirectorySeparatorChar.ToString());
            return ResolveSymlinks(new FileInfo(Installations()
                .Where(Directory.Exists)
                .Select(root => root + absolute)
                .First()
            ));
        }

        /// <summary>Resolve symlinks in path</summary>
        public static FileInfo ResolveSymlinks(FileInfo info)
        {
            if (-1 == (int)info.Attributes)     // Does not exist, one of the parent directories is a symlink!
            {
                var directory = info.Directory;
                while (!directory.Exists)
                {
                    directory = directory.Parent;
                }

                var path = info.FullName.Substring(directory.FullName.Length + 1).Split(new char[] { Path.DirectorySeparatorChar }, 2);
                var link = new FileInfo(directory.FullName + Path.DirectorySeparatorChar + path[0]);

                return ResolveSymlinks(new FileInfo(
                    Resolve(ReadLink(link)).FullName +
                    Path.DirectorySeparatorChar +
                    path[1]
                ));
            }
            else if ((info.Attributes & FileAttributes.System) == FileAttributes.System)
            {
                var target = ReadLink(info);
                while (null != target)
                {
                    info = Resolve(target);
                    target = ReadLink(info);
                }
                return info;
            }
            else
            {
                return info;
            }
        }

        /// <summary>Checks whether a file is a cygwin symlink file, and resolves it. See the
        /// Cygwin docs, https://cygwin.com/cygwin-ug-net/using.html#pathnames-symlinks</summary>
        public static string ReadLink(FileInfo info)
        {
            if ((info.Attributes & FileAttributes.System) != FileAttributes.System) return null;

            using (var stream = info.OpenRead())
            {
                var cookie = new byte[SYMLINK_COOKIE.Length];
                stream.Read(cookie, 0, SYMLINK_COOKIE.Length);
                if (!cookie.SequenceEqual(SYMLINK_COOKIE)) return null;

                using (var text = new StreamReader(stream, true))
                {
                    return text.ReadToEnd().TrimEnd('\0');
                }
            }
        }

        /// <summary>Creates a symbolic link to a given target</summary>
        public static void CreateSymlink(FileInfo info, string target)
        {
            Symlink(FileMode.CreateNew, info, target);
        }

        /// <summary>Creates a symbolic link to a given target</summary>
        public static void ReplaceSymlink(FileInfo info, string target)
        {
            Symlink(FileMode.Truncate, info, target);
        }

        /// <summary>Creates a symbolic link in the given mode</summary>
        private static void Symlink(FileMode mode, FileInfo info, string target)
        {
            using (var stream = info.Open(mode))
            {
                stream.Write(SYMLINK_COOKIE, 0, SYMLINK_COOKIE.Length);
                stream.WriteByte(255);
                stream.WriteByte(254);
                stream.Write(Encoding.Unicode.GetBytes(target), 0, Encoding.Unicode.GetByteCount(target));
                stream.WriteByte(0);
            }

            info.Attributes |= FileAttributes.System;
        }
    }
}