
using MSMQ.Messaging.Interop;

namespace MSMQ.Messaging
{
   

    /// <include file='..\..\doc\MessageQueueTransactionType.uex' path='docs/doc[@for="MessageQueueTransactionType"]/*' />
    /// <devdoc>
    ///    <para>[To be supplied.]</para>
    /// </devdoc>
    public enum MessageQueueTransactionType
    {
        /// <include file='..\..\doc\MessageQueueTransactionType.uex' path='docs/doc[@for="MessageQueueTransactionType.None"]/*' />
        /// <devdoc>
        ///    <para>[To be supplied.]</para>
        /// </devdoc>
        None = NativeMethods.QUEUE_TRANSACTION_NONE,
        /// <include file='..\..\doc\MessageQueueTransactionType.uex' path='docs/doc[@for="MessageQueueTransactionType.Automatic"]/*' />
        /// <devdoc>
        ///    <para>[To be supplied.]</para>
        /// </devdoc>
        Automatic = NativeMethods.QUEUE_TRANSACTION_MTS,
        /// <include file='..\..\doc\MessageQueueTransactionType.uex' path='docs/doc[@for="MessageQueueTransactionType.Single"]/*' />
        /// <devdoc>
        ///    <para>[To be supplied.]</para>
        /// </devdoc>
        Single = NativeMethods.QUEUE_TRANSACTION_SINGLE,
    }
}
