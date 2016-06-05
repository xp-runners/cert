using System;

/// <summary>NOOP Stubs for Windows C# compiler</summary>
namespace Mono.Unix
{
    public class UnixFileInfo
    {
        public string FullName {
          get { throw new NotImplementedException("FullName"); }
        }

        public UnixFileInfo(string name)
        {
            throw new NotImplementedException("UnixFileInfo");
        }

        public UnixSymbolicLinkInfo CreateSymbolicLink(string target)
        {
            throw new NotImplementedException("CreateSymbolicLink");
        }
    }

    public class UnixSymbolicLinkInfo
    {
        public UnixSymbolicLinkInfo(string name)
        {
            throw new NotImplementedException("UnixSymbolicLinkInfo");
        }

        public UnixFileInfo GetContents()
        {
            throw new NotImplementedException("GetContents");
        }
    }
}

namespace Mono.Unix.Native
{
    public class Utsname
    {
        public string sysname;

        public Utsname()
        {
            throw new NotImplementedException("Utsname");
        }
    }

    public class Syscall
    {
        public static int uname(out Utsname name)
        {
            throw new NotImplementedException("uname");
        }
    }
}
