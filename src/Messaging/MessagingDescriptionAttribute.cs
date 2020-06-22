

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Security.Permissions;

namespace MSMQ.Messaging
{



    /// <include file='..\..\doc\MessagingDescriptionAttribute.uex' path='docs/doc[@for="MessagingDescriptionAttribute"]/*' />
    /// <devdoc>
    ///     DescriptionAttribute marks a property, event, or extender with a
    ///     description. Visual designers can display this description when referencing
    ///     the member.
    /// </devdoc>
    [AttributeUsage(AttributeTargets.All)]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes")]
    public class MessagingDescriptionAttribute : DescriptionAttribute
    {

        private bool replaced = false;

        /// <include file='..\..\doc\MessagingDescriptionAttribute.uex' path='docs/doc[@for="MessagingDescriptionAttribute.MessagingDescriptionAttribute"]/*' />
        /// <devdoc>
        ///     Constructs a new sys description.
        /// </devdoc>
        public MessagingDescriptionAttribute(string description)
            : base(description)
        {
        }

        /// <include file='..\..\doc\MessagingDescriptionAttribute.uex' path='docs/doc[@for="MessagingDescriptionAttribute.Description"]/*' />
        /// <devdoc>
        ///     Retrieves the description text.
        /// </devdoc>
        public override string Description
        {
            [HostProtection(SharedState = true)] // DescriptionAttribute uses SharedState=true. We should not change base's behavior
            get
            {
                if (!replaced)
                {
                    replaced = true;
                    DescriptionValue = Res.GetString(base.Description);
                }
                return base.Description;
            }
        }
    }
}
