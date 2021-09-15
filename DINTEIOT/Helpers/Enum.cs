using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DINTEIOT.Helpers
{
    public enum ExitCodes
    {
        NotAuthorize =-1,
        Success = 1,
        Error = 0,
        SignToolNotInPath = 2,
        AssemblyDirectoryBad = 3,
        PFXFilePathBad = 4,
        PasswordMissing = 8,
        SignFailed = 16,
        UnknownError = 32
    }
    public enum MethodType
    {
        THUCONG = 1,
        TUDONG = 2
    }
}
