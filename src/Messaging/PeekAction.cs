
using System.Diagnostics.CodeAnalysis;
using MSMQ.Messaging.Interop;

namespace MSMQ.Messaging
{


    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum PeekAction
    {

        Current = NativeMethods.QUEUE_ACTION_PEEK_CURRENT,

        Next = NativeMethods.QUEUE_ACTION_PEEK_NEXT
    }
}
