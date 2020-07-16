using System;
using System.Linq;
using MessagingExamples;
using MSMQ.Messaging;

namespace Examples
{
    internal class SendExample : IExample
    {
     
        public event EventHandler<ExampleMessageArgs> ExampleMessage;
        private const string DESTINATION_QUEUE = @"FORMATNAME:DIRECT=OS:SERVERNAME\PRIVATE$\QUEUENAME";

        public bool RunExample(string[] args)
        {

            var thisQueue = args.Any() ? args[0] : DESTINATION_QUEUE;

            var messageText = "Hello, World";

            using var mq = new MSMQ.Messaging.MessageQueue(thisQueue);
            using var msg = new Message(messageText);
            using var msgTransaction = new MessageQueueTransaction();

            if (mq.Transactional) {
                msgTransaction.Begin();
                mq.Send(msg, MessageQueueTransactionType.Single);
                msgTransaction.Commit();
            }
            else {
                mq.Send(messageText);
            }

            OnExampleMessage(new ExampleMessageArgs($"Sent {messageText} to {thisQueue}"));
            return true;

        }
        
        public void OnExampleMessage(ExampleMessageArgs e) {
            ExampleMessage?.Invoke(this,e);
        }
    }
}
