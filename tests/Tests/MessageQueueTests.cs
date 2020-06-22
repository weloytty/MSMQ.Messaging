using System;
using MSMQ.Messaging;
using Xunit;

namespace MSMQ.Messaging.Tests
{
    public class MessageQueueTests
    {

     

        public MessageQueueTests()
        {
            if (!MessageQueue.Exists(TestCommon.TEST_PRIVATE_TRANSACTIONAL_QUEUE))
            {
                using var mq = MessageQueue.Create(TestCommon.TEST_PRIVATE_TRANSACTIONAL_QUEUE,true);

            }

            if (!MessageQueue.Exists(TestCommon.TEST_PRIVATE_NONTRANSACTIONAL_QUEUE))
            {
                using var mq = MessageQueue.Create(TestCommon.TEST_PRIVATE_NONTRANSACTIONAL_QUEUE);
            }

            if (!MessageQueue.Exists(TestCommon.TEST_ADMIN_QUEUE))
            {
                using var mq = MessageQueue.Create(TestCommon.TEST_ADMIN_QUEUE);
            }
        }
        [Fact()]
        public void CreateTest()
        {
            var queueName = $".\\private$\\{Guid.NewGuid():N}";
            Assert.False(MessageQueue.Exists(queueName));

            using MessageQueue mq = MessageQueue.Create(queueName);
            Assert.NotNull(mq);
            Assert.IsType<MessageQueue>(mq);
            Assert.True(MessageQueue.Exists(queueName));
            MessageQueue.Delete(queueName);
            Assert.False(MessageQueue.Exists(queueName));
        }


        [Fact()]
        public void ReceiveTest()
        {
            SendTest();
            using var mq = new MessageQueue(TestCommon.TEST_PRIVATE_TRANSACTIONAL_QUEUE)
            {
                Formatter = new XmlMessageFormatter(new Type[] { typeof(string) })
            };
            var msg = mq.Receive();
            Assert.Equal(msg.Body, TestCommon.TEST_MESSAGE_BODY);
        }


        [Fact()]
        public void ReceiveByCorrelationIdTest()
        {

           
            MessageQueue queue = new MessageQueue(TestCommon.TEST_PRIVATE_NONTRANSACTIONAL_QUEUE);
            Message msg = new Message("Example Message Body");

            queue.Send(msg, "Example Message Label");

            string id = msg.Id;

            msg = queue.ReceiveById(id, TimeSpan.FromSeconds(10.0));
            MessageQueue transQueue = new MessageQueue(TestCommon.TEST_PRIVATE_TRANSACTIONAL_QUEUE);
            Message responseMsg = new Message("Example Response Message Body") {CorrelationId = id};

            transQueue.Send(responseMsg, "Example Response Message Label",
                MessageQueueTransactionType.Single);

            // Set the transactional queue's MessageReadPropertyFilter property to
            // ensure that the response message includes the desired properties.
            transQueue.MessageReadPropertyFilter.CorrelationId = true;

            // Receive the response message from the transactional queue.
            responseMsg = transQueue.ReceiveByCorrelationId(id,
                TimeSpan.FromSeconds(10.0), MessageQueueTransactionType.Single);

            // Display the response message's property values.
            Assert.True(responseMsg.CorrelationId == msg.Id);

        }

        //[Fact()]
        //public void ReceiveByLookupIdTest()
        //{
        //    Assert.True(false, "This test needs an implementation");
        //}

        [Fact()]
        public void SendTest()
        {
            using var mq = new MessageQueue(TestCommon.TEST_PRIVATE_TRANSACTIONAL_QUEUE );
            if (mq.Transactional)
            {
                using var msgTransaction = new MessageQueueTransaction();
                msgTransaction.Begin();
                mq.Send(TestCommon.TEST_MESSAGE_BODY, msgTransaction);
                msgTransaction.Commit();
            }
            else
            {
                mq.Send(TestCommon.TEST_MESSAGE_BODY);
            }
        }




        //[Fact()]
        //public void SetPermissionsTest()
        //{
        //    Assert.True(false, "This test needs an implementation");
        //}
    }
}