using System;

namespace MessagingExamples {

    public class ExampleMessageArgs : EventArgs {
        public string ExmpleMessage;
        public ExampleMessageArgs(string message) {
            ExmpleMessage = message;
        }
    }


    public interface IExample {
        bool RunExample(string[] args);
        event EventHandler<ExampleMessageArgs> ExampleMessage;
    }
}