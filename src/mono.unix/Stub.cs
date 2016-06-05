using System;

namespace Mono.Unix
{
    /// <summary>NOOP Stub for Windows C# compiler</summary>
    public class UnixFileInfo
    {
        public string FullName {
          get { throw new NotImplementedException("FullName"); }
        }

        public UnixFileInfo(string name) {
            throw new NotImplementedException("UnixFileInfo");
        }

        public UnixSymbolicLinkInfo CreateSymbolicLink(string target)
        {
            throw new NotImplementedException("CreateSymbolicLink");
        }
    }

    /// <summary>NOOP Stub for Windows C# compiler</summary>
    public class UnixSymbolicLinkInfo
    {
        public UnixSymbolicLinkInfo(string name) {
            throw new NotImplementedException("UnixSymbolicLinkInfo");
        }

        public UnixFileInfo GetContents()
        {
            throw new NotImplementedException("GetContents");
        }
    }
}
