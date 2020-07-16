using System;
using System.Linq.Expressions;
using Examples;

namespace MessagingExamples
{
    public class ExampleRunner
    {
        static void Main(string[] args)
        {


            //If you want to use a specific queue for whatever operation you are going to run
            //pass it in as args[0]. Otherwise, the test will use the queuename defined in the test.



            RunOne(new SendExample(), args);
            RunOne(new ReceiveExample(), args);

            Console.WriteLine("Done");
        }

        private static void RunOne(IExample oneExample, string[] argumentsForExample)
        {
            try {
                oneExample.ExampleMessage += Example_OnExampleMessage;
                oneExample.RunExample(argumentsForExample);
                oneExample.ExampleMessage -= Example_OnExampleMessage;
            }
            catch (Exception e) {
                Console.WriteLine($"{oneExample.GetType().Name} threw exception {e.Message}");
            }

        }

        private static void Example_OnExampleMessage(object sender, ExampleMessageArgs e)
        {
            Console.WriteLine($"{e.ExmpleMessage}");
        }

    }
}