using System.Xml.Linq;
using MSMQ.Messaging;
using Xunit;

namespace MSMQ.Messaging.Tests {
    public class MessageTests {

        [Fact()]
        public void MessageTest() {
            using var q = new Message();
            Assert.NotNull(q);
            Assert.IsType<Message>(q);
        }


        [Fact()]
        public void MessageTestBody()
        {
            var messageBody = "Whatever";
            using var q = new Message(messageBody);
            Assert.NotNull(q);
            Assert.IsType<Message>(q);
            Assert.True((string)q.Body == messageBody);
        }

        [Fact()]
        public void MessageTestBodyFormatter() {
            XElement xmlBody = new XElement("Root",
                new XElement("Child1", 1),
                new XElement("Child2", 2),
                new XElement("Child3", 3),
                new XElement("Child4", 4),
                new XElement("Child5", 5),
                new XElement("Child6", 6)
            );
            using var q = new Message(xmlBody, new XmlMessageFormatter());
            Assert.NotNull(q);
            Assert.IsType<Message>(q);
        }
    }
}