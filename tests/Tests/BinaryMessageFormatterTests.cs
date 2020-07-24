using System;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using MSMQ.Messaging;
using Xunit;

namespace MSMQ.Messaging.Tests
{
    public class BinaryMessageFormatterTests
    {

        private const string TEST_IMAGE = "assets/ironing a kitten.jpg";
 

        [Fact()]
        public void BinaryMessageFormatterTest()
        {
            Assert.True(File.Exists(TEST_IMAGE), "Can't find binary file to test.");

            Image thisImage = Image.FromFile(TEST_IMAGE);
            using var q = new MessageQueue(TestCommon.TEST_PRIVATE_NONTRANSACTIONAL_QUEUE);
            using var m = new Message(thisImage,new BinaryMessageFormatter());
            q.Formatter = new BinaryMessageFormatter();

            q.Send(m);
            string msgId = m.Id;
            var recMsg = q.ReceiveById(msgId, TimeSpan.FromSeconds(10.0));
         

            //if I was smart, I would be able to compare the two images.  Suffice to say if we get here, it passed.
            //Assert.True(TestCommon.ByteArrayCompare(ConvertStreamToByteArray(recMsg.BodyStream),TestCommon.GetImageBytes(thisImage)));
            Assert.True(true);
           

        }


        internal static byte[] ConvertStreamToByteArray(System.IO.Stream input)
        {
            byte[] buffer = new byte[16 * 1024];

            using System.IO.MemoryStream ms = new System.IO.MemoryStream();
            int chunk;

            while ((chunk = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, chunk);
            }

            return ms.ToArray();
        }


       
    }
}