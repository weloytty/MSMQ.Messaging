//------------------------------------------------------------------------------
// <copyright file="IPersistStream.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>                                                                
//------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MSMQ.Messaging.Interop {
    
    
    [ComImport(), 
    Guid("00000109-0000-0000-C000-000000000046"), 
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPersistStream {        
        [SuppressUnmanagedCodeSecurity()]
        void GetClassID([Out] out Guid pClassID);

        [SuppressUnmanagedCodeSecurity()]
        int IsDirty();

        [SuppressUnmanagedCodeSecurity()]
        void Load([In, MarshalAs(UnmanagedType.Interface)] IStream pstm);

        [SuppressUnmanagedCodeSecurity()]
        void Save([In, MarshalAs(UnmanagedType.Interface)] IStream pstm,
                  [In, MarshalAs(UnmanagedType.Bool)] bool fClearDirty);

        [SuppressUnmanagedCodeSecurity()] 
        long GetSizeMax();
    }
}
