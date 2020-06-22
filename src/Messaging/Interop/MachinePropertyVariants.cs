

namespace MSMQ.Messaging.Interop
{

   
    internal class MachinePropertyVariants : MessagePropertyVariants
    {
        public MachinePropertyVariants()
            : base(5, NativeMethods.MACHINE_BASE + 1)
        {
        }
    }
}
