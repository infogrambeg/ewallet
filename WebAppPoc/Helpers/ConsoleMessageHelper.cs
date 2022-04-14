using System;

namespace WebAppPoc.Helpers
{
    public class ConsoleMessageHelper
    {
        private static readonly object _MessageLock = new object();
        public static void WriteMessageError(string message)
        {
            lock (_MessageLock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public static void WriteMessageSuccess(string message)
        {
            lock (_MessageLock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}
