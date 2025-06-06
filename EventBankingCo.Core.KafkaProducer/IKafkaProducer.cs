namespace EventBankingCo.Core.KafkaProducer
{
    public interface IKafkaProducer<TKey, TValue>
    {
        /// <summary>
        /// Asynchronously produces a message to the Kafka topic with the specified key and value.
        /// </summary>
        /// <param name="key">The key to use for the message sent to the Kafka Topic.</param>
        /// <param name="value">The value to use for the message sent tothe Kafka Topic</param>
        Task ProduceAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    }
}
