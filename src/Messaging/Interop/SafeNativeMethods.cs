
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;

namespace MSMQ.Messaging.Interop
{
    

    [ComVisible(false),
    SuppressUnmanagedCodeSecurity()]
    internal static class SafeNativeMethods
    {
        public unsafe delegate void ReceiveCallback(int result, IntPtr handle, int timeout, int action, IntPtr propertiesPointer, NativeOverlapped* overlappedPointer, IntPtr cursorHandle);

        [DllImport(ExternDll.Mqrt, EntryPoint = "MQBeginTransaction", CharSet = CharSet.Unicode)]
        public static extern int IntMQBeginTransaction(out ITransaction refTransaction);
        public static int MQBeginTransaction(out ITransaction refTransaction)
        {
            try
            {
                return IntMQBeginTransaction(out refTransaction);
            }
            catch (DllNotFoundException)
            {
                throw new InvalidOperationException(Res.GetString(Res.MSMQNotInstalled));
            }
        }

        [DllImport(ExternDll.Mqrt, CharSet = CharSet.Unicode)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern int MQCloseQueue(IntPtr handle);

        [DllImport(ExternDll.Mqrt, EntryPoint = "MQPathNameToFormatName", CharSet = CharSet.Unicode)]
        private static extern int IntMQPathNameToFormatName(string pathName, StringBuilder formatName, ref int count);
        public static int MQPathNameToFormatName(string pathName, StringBuilder formatName, ref int count)
        {
            try
            {
                return IntMQPathNameToFormatName(pathName, formatName, ref count);
            }
            catch (DllNotFoundException)
            {
                throw new InvalidOperationException(Res.GetString(Res.MSMQNotInstalled));
            }
        }

        [DllImport(ExternDll.Mqrt, EntryPoint = "MQInstanceToFormatName", CharSet = CharSet.Unicode)]
        public static extern int IntMQInstanceToFormatName(byte[] id, StringBuilder formatName, ref int count);
        public static int MQInstanceToFormatName(byte[] id, StringBuilder formatName, ref int count)
        {
            try
            {
                return IntMQInstanceToFormatName(id, formatName, ref count);
            }
            catch (DllNotFoundException)
            {
                throw new InvalidOperationException(Res.GetString(Res.MSMQNotInstalled));
            }
        }

        [DllImport(ExternDll.Mqrt, CharSet = CharSet.Unicode)]
        public static extern int MQCreateCursor(MessageQueueHandle handle, out CursorHandle cursorHandle);

        [DllImport(ExternDll.Mqrt, CharSet = CharSet.Unicode)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern int MQCloseCursor(IntPtr cursorHandle);

        [DllImport(ExternDll.Mqrt, CharSet = CharSet.Unicode)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern void MQFreeSecurityContext(IntPtr handle);

        [DllImport(ExternDll.Mqrt, CharSet = CharSet.Unicode)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern int MQLocateEnd(IntPtr enumHandle);

        [DllImport(ExternDll.Mqrt, CharSet = CharSet.Unicode)]
        public static extern int MQLocateNext(LocatorHandle enumHandle, ref int propertyCount, [Out] MQPROPVARIANTS[] variantArray);

        [DllImport(ExternDll.Mqrt, CharSet = CharSet.Unicode)]
        public static extern void MQFreeMemory(IntPtr memory);

        [DllImport(ExternDll.Kernel32, ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetHandleInformation(SafeHandle handle, out int handleInformation);

        [DllImport(ExternDll.Kernel32)]
        public static extern IntPtr LocalFree(IntPtr hMem);

        [DllImport(ExternDll.Advapi32)]
        public static extern int SetEntriesInAclW(int count,
            //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0, ArraySubType = ExplicitAccess)]
            //ExplicitAccess[] entries,
                                                  IntPtr entries,
                                                  IntPtr oldacl,
                                                  out IntPtr newAcl);

        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Unicode)]
        public static extern bool GetComputerName(StringBuilder lpBuffer, int[] nSize);

        public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100,
            FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200,
            FORMAT_MESSAGE_FROM_STRING = 0x00000400,
            FORMAT_MESSAGE_FROM_HMODULE = 0x00000800,
            FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000,
            FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000,
            FORMAT_MESSAGE_MAX_WIDTH_MASK = 0x000000FF;

        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Unicode)]
        public static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId,
                                                int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr arguments);

    }
}
