

using System;
using MSMQ.Messaging.Interop;

namespace MSMQ.Messaging
{



    public sealed class SecurityContext : IDisposable
    {

        SecurityContextHandle handle;
        bool disposed;

        internal SecurityContext(SecurityContextHandle securityContext)
        {
            handle = securityContext;
        }

        internal SecurityContextHandle Handle
        {
            get
            {
                if (disposed)
                    throw new ObjectDisposedException(GetType().Name);

                return handle;
            }
        }


        public void Dispose()
        {
            handle.Close();
            disposed = true;
        }
    }
}
