//------------------------------------------------------------------------------
// <copyright file="QueuePropertyVariants.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>                                                                
//------------------------------------------------------------------------------

namespace MSMQ.Messaging.Interop
{

  

    internal class QueuePropertyVariants : MessagePropertyVariants
    {

        private const int MaxQueuePropertyIndex = 26;

        public QueuePropertyVariants()
            : base(MaxQueuePropertyIndex, NativeMethods.QUEUE_PROPID_BASE + 1)
        {
        }
    }
}
