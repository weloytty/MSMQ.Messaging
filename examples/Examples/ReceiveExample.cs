using System;
using System.Linq;
using MessagingExamples;
using MSMQ.Messaging;

namespace Examples
{
    public class ReceiveExample : IExample
    {
        public event EventHandler<ExampleMessageArgs> ExampleMessage;
        private const string DESTINATION_QUEUE = @"FORMATNAME:DIRECT=OS:SERVERNAME\PRIVATE$\QUEUENAME";

        public bool RunExample(string[] args) {

            var thisQueue = args.Any() ? args[0] : DESTINATION_QUEUE;

            using var mq = new MessageQueue(thisQueue)
            {
                Formatter = new XmlMessageFormatter(new Type[] { typeof(string) })
            };
            var msg = mq.Receive();
            OnExampleMessage(new ExampleMessageArgs($"Recieved {msg.Body} from {thisQueue}"));

            return true;

        }
        public void OnExampleMessage(ExampleMessageArgs e)
        {
            ExampleMessage?.Invoke(this, e);
        }

    }
}