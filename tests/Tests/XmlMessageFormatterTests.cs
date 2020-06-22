using System;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using MSMQ.Messaging;
using Xunit;

namespace MSMQ.Messaging.Tests
{
    public class XmlMessageFormatterTests
    {

        public XmlMessageFormatterTests()
        {
            if (!MessageQueue.Exists(TestCommon.TEST_PRIVATE_TRANSACTIONAL_QUEUE))
            {
                MessageQueue.Create(TestCommon.TEST_PRIVATE_TRANSACTIONAL_QUEUE, true);}

            if (!MessageQueue.Exists(TestCommon.TEST_PRIVATE_NONTRANSACTIONAL_QUEUE))
            {
                MessageQueue.Create(TestCommon.TEST_PRIVATE_NONTRANSACTIONAL_QUEUE);
            }
        }

        [Fact()]
        public string XmlMessageFormatterSendTest()
        {
            Address addy = new Address(){StreetAddress = "123 Main Street",City="Anytown",StateOrRegion = "NC",PostalCode = "12345"};
            Person p = new Person(){FirstName = "Test",LastName = "User",Address = addy,MaritalStatus = MaritalStatus.Married};
            
            using var q = new MessageQueue(TestCommon.TEST_PRIVATE_NONTRANSACTIONAL_QUEUE);
            using var m = new Message(p);
            q.Formatter = new XmlMessageFormatter(new Type[] {typeof(Person)});
            q.Send(m);
            Assert.True(true);
            return m.Id;
        }




        [Fact()]
        public void XmlMessageFormatterSendReceiveTest()
        {

            Assert.True(MessageQueue.Exists(TestCommon.TEST_PRIVATE_NONTRANSACTIONAL_QUEUE));


            Address addy = new Address(){StreetAddress = "123 Main Street",City="Anytown",StateOrRegion = "NC",PostalCode = "12345"};
            Person p = new Person(){FirstName = "Test",LastName = "User",Address = addy,MaritalStatus = MaritalStatus.Married};
            
            using var q = new MessageQueue(TestCommon.TEST_PRIVATE_NONTRANSACTIONAL_QUEUE);
            using var m = new Message(p);
            q.Formatter = new XmlMessageFormatter(new Type[] {typeof(Person)});
            q.Send(m);
            string msgId = m.Id;
            var recMsg = q.ReceiveById(msgId, TimeSpan.FromSeconds(10.0));
            Assert.True(p == (Person)recMsg.Body);
        }

        
    }
}