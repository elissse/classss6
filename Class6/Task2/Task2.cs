using Microsoft.Extensions.Logging;

namespace Task2
{
    public class Task2
    {
        private static readonly ILogger<Task2> Logger =
            LoggerFactory.Create(builder => { builder.AddConsole(); }).CreateLogger<Task2>();

        public static void Main(string[] args)
        {
            Logger.LogInformation("program started");
            TODO();
            Logger.LogInformation("program completed");
        }

        internal static void TODO()
        {
            throw new NotImplementedException();
        }

        private static T TODO<T>()
        {
            throw new NotImplementedException();
        }
    }
}