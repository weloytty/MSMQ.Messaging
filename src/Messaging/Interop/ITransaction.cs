

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MSMQ.Messaging.Interop
{
    
    using UnmanagedType = UnmanagedType;

    [ComImport, Guid("0FB15084-AF41-11CE-BD2B-204C4F4F5020"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ITransaction
    {

        [return: MarshalAs(UnmanagedType.I4)]
        [SuppressUnmanagedCodeSecurity()]
        [PreserveSig]
        int Commit(
           [In, MarshalAs(UnmanagedType.I4)] 
             int fRetaining,
           [In, MarshalAs(UnmanagedType.U4)] 
             int grfTC,
           [In, MarshalAs(UnmanagedType.U4)] 
             int grfRM);

        [return: MarshalAs(UnmanagedType.I4)]
        [SuppressUnmanagedCodeSecurity()]
        [PreserveSig]
        int Abort(
           [In, MarshalAs(UnmanagedType.U4)]
              int pboidReason,
           [In, MarshalAs(UnmanagedType.I4)] 
             int fRetaining,
           [In, MarshalAs(UnmanagedType.I4)] 
             int fAsync);

        [return: MarshalAs(UnmanagedType.I4)]
        [SuppressUnmanagedCodeSecurity()]
        [PreserveSig]
        int GetTransactionInfo(
           [In, Out]
            IntPtr /* XACTTRANSINFO */ pinfo);
    }
}
