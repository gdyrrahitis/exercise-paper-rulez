namespace PaperRulez.Models
{
    public class RabbitMqConstants
    {
        public const string RabbitMqUri = "amqp://guest:guest@localhost:5672/";
        public const string JsonMimeType = "application/json";

        public const string FileLoadExchange = "file.load.exchange";
        public const string FileLoadQueue = "file.load.queue";

        public const string ProcessSuccessExchange = "process.success.exchange";
        public const string ProcessSuccessQueue = "process.success.queue";

        public const string ProcessEndExchange = "process.end.exchange";
        public const string ProcessEndQueue = "process.end.queue";

        public const string FileRemovedExchange = "file.removed.exchange";
        public const string FileRemovedQueue = "file.removed.queue";
    }
}