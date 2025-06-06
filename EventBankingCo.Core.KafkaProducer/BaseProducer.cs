using Confluent.Kafka;
using EventBankingCo.Core.Logging.Abstraction;

namespace EventBankingCo.Core.KafkaProducer
{
    public abstract class BaseProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
    {
        #region Protected Constants

        protected const int MinimumAttemptsToProduceMessage = 1;

        protected const int MinimumRetryDelayInMilliseconds = 500;

        #endregion

        #region Private Fields

        private readonly IProducer<TKey, TValue> _producer;

        private readonly ICoreLogger _logger;

        private readonly TopicPartition _topicPartition;

        private int _maxAttemptsToProduceMessage = MinimumAttemptsToProduceMessage;

        private int _retryDelayInMilliseconds = MinimumRetryDelayInMilliseconds;

        #endregion

        #region Protected Fields

        protected string Topic => _topicPartition.Topic;

        protected Partition Partition => _topicPartition.Partition;

        protected int MaxAttemptsToProduceMessage
        {
            get => _maxAttemptsToProduceMessage;
            set => _maxAttemptsToProduceMessage = value >= MinimumAttemptsToProduceMessage ? value
                : throw new ArgumentOutOfRangeException(nameof(MaxAttemptsToProduceMessage), 
                    $"MaxAttemptsToProduceMessage must be greater than or equal to {MinimumAttemptsToProduceMessage}. New Value: {value}");
        }

        protected int RetryDelayInMilliseconds
        {
            get => _retryDelayInMilliseconds;
            set => _retryDelayInMilliseconds = value >= MinimumRetryDelayInMilliseconds ? value
                : throw new ArgumentOutOfRangeException(nameof(RetryDelayInMilliseconds), 
                    $"RetryDelayInMilliseconds must be greater than or equal to {MinimumRetryDelayInMilliseconds}. New Value: {value}");
        }

        #endregion

        #region Constructor

        protected BaseProducer(IProducer<TKey, TValue> producer, ICoreLogger logger, string topicName, Partition? partition = null)
        {
            _producer = producer ?? throw new ArgumentNullException(nameof(producer), "Producer cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");

            if (string.IsNullOrWhiteSpace(topicName))
            {
                throw new ArgumentException("Topic name cannot be null or empty.", nameof(topicName));
            }

            _topicPartition = new TopicPartition(topicName, partition ?? Partition.Any);
        }

        #endregion

        public async Task ProduceAsync(TKey key, TValue value, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Producing message to topic '{Topic}' on partition {Partition} with key '{key}' and value '{value}'.");

            var message = new Message<TKey, TValue>()
            {
                Key = key,
                Value = value,
            };

            for (int attempt = 1; attempt <= MaxAttemptsToProduceMessage; attempt++)
            {
                try
                {
                    _logger.LogDebug($"Attempt {attempt} to produce message to topic '{Topic}' on partition {Partition} with key '{key}' and value '{value}'.");

                    var result = await _producer.ProduceAsync(_topicPartition, message, cancellationToken);

                    if (IsMessageDeliverySuccessful(result, message))
                    {
                        _logger.LogDebug($"Message successfully produced to topic '{result.Topic}' on partition {result.Partition} with key '{result.Message.Key}' at offset {result.Offset} on attempt {attempt}.");

                        return;
                    }
                    else
                    {
                        if (attempt == MaxAttemptsToProduceMessage)
                        {
                            await HandleFinalMessageDeliveryFailureAsync(result, message, attempt, cancellationToken);
                        }
                        else
                        {
                            _logger.LogWarning($"Message delivery not successful for topic {Topic} on Partition {Partition} with Key {message.Key} on attempt {attempt}. Last known status: {result?.Status} Retrying in {RetryDelayInMilliseconds}");

                            await Task.Delay(RetryDelayInMilliseconds, cancellationToken);
                        }
                    }
                }
                catch (KafkaRetriableException e)
                {
                    if (attempt == MaxAttemptsToProduceMessage)
                    {
                        await HandleFinalRetriableExceptionAsync(e, message, attempt, cancellationToken);

                        if (IsFinalRetriableExceptionThrowable(e))
                        {
                            throw;
                        }
                    }
                    else
                    {
                        await HandleExceptionBeforeRetryAsync(e, message, attempt, cancellationToken);

                        await Task.Delay(RetryDelayInMilliseconds, cancellationToken);
                    }
                }
                catch (Exception e)
                {
                    await HandleNonRetriableExceptionAsync(e, message, attempt, cancellationToken);

                    if (IsNonRetriableExceptionThrowable(e))
                    {
                        throw;
                    }

                    if (StopRetryingOnNonRetriableException(e))
                    {
                        _logger.LogWarning($"Stopping retries for message with key '{message.Key}' on topic '{Topic}' and partition {Partition} due to non-retriable exception.", e);

                        return;
                    }
                    else
                    {
                        _logger.LogWarning($"Continuing retries for message with key '{message.Key}' on topic '{Topic}' and partition {Partition} despite non-retriable exception.", e);
                    }
                }
            }
        }

        #region Protected Virtual Methods - Message Delivery Evaluation And Failure Handling

        protected bool IsMessageDeliverySuccessful(DeliveryResult<TKey, TValue> result, Message<TKey, TValue> message)
        {
            switch (result?.Status)
            {
                case PersistenceStatus.Persisted:
                    _logger.LogInformation($"Message with key '{result.Message.Key}' successfully delivered to topic '{result.Topic}' on partition {result.Partition} at offset {result.Offset}.");
                    return true;

                case PersistenceStatus.PossiblyPersisted:
                    _logger.LogWarning($"Message with key '{result.Message.Key}' possibly delivered to topic '{result.Topic}' on partition {result.Partition} at offset {result.Offset}. This may require further verification.");
                    return false;

                case PersistenceStatus.NotPersisted:
                    _logger.LogError($"Message with key '{result.Message.Key}' not delivered to topic '{result.Topic}' on partition {result.Partition}. Offset: {result.Offset}. This may require retrying.");
                    return false;

                default:
                    _logger.LogError($"Unexpected persistence status '{result?.Status}' for message with key '{message.Key}' on topic '{Topic}' and partition {Partition}.");
                    return false;
            }
        }

        protected Task HandleFinalMessageDeliveryFailureAsync(DeliveryResult<TKey, TValue> result, Message<TKey, TValue> message, int attempt, CancellationToken cancellationToken)
        {
            _logger.LogError($"Final attempt to deliver message not successful for topic {Topic} on Partition {Partition} with Key {message.Key} after {attempt} attempts. Last known status: {result?.Status}.");

            return Task.CompletedTask;
        }

        #endregion

        #region Protected Virtual Methods - Retriable Exception Handling

        protected Task HandleExceptionBeforeRetryAsync(KafkaRetriableException e, Message<TKey, TValue> message, int attempt, CancellationToken cancellationToken)
        {
            _logger.LogWarning($"Exception occurred while producing message to topic '{Topic}' on partition {Partition} with key '{message.Key}' on attempt {attempt}. Retrying in {RetryDelayInMilliseconds} milliseconds.", e);

            return Task.CompletedTask;
        }

        protected Task HandleFinalRetriableExceptionAsync(KafkaRetriableException e, Message<TKey, TValue> message, int attempt, CancellationToken cancellationToken)
        {
            _logger.LogError($"Exception occurred on final attempt ({attempt}) to produce message to topic '{Topic}' on partition {Partition} with key '{message.Key}'", e);

            return Task.CompletedTask;
        }

        protected bool IsFinalRetriableExceptionThrowable(KafkaRetriableException e) => true;

        #endregion

        #region Protected Virtual Methods - Non-Retriable Exception Handling

        protected Task HandleNonRetriableExceptionAsync(Exception e, Message<TKey, TValue> message, int attempt, CancellationToken cancellationToken)
        {
            _logger.LogError($"Non-retriable exception occurred while producing message to topic '{Topic}' on partition {Partition} with key '{message.Key}' on attempt {attempt}. Message: {message.Value}", e);

            return Task.CompletedTask;
        }

        protected bool IsNonRetriableExceptionThrowable(Exception e) => true;

        protected bool StopRetryingOnNonRetriableException(Exception e) => true;

        #endregion
    }
}
