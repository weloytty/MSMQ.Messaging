using System.Diagnostics.CodeAnalysis;

namespace MSMQ
{
    
    [SuppressMessage("Style", "IDE1006:Naming Styles")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    internal static class ExternDll
    {
        public const string Advapi32 = "advapi32.dll";
        public const string Kernel32 = "kernel32.dll";
        public const string Mqrt = "mqrt.dll";
        public const string Ole32 = "ole32.dll";
    }
}