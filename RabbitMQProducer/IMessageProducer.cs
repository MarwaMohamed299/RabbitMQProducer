namespace RabbitMQProducer
{
    public interface IMessageProducer
    {
        public void SendMessages<T>(T message);
    }
}
