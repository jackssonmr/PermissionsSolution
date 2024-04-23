using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Permissions.Infrastructure.Kafka
{
    public class ConsumerService : BackgroundService
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly ILogger<ConsumerService> _logger;
        private readonly string _topic;

        public ConsumerService(ConsumerConfig consumerConfig, ILogger<ConsumerService> logger, string topic)
        {
            _consumerConfig = consumerConfig;
            _logger = logger;
            _topic = topic;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                consumer.Subscribe(_topic);

                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(stoppingToken);
                            if (consumeResult != null)
                            {
                                _logger.LogInformation($"Consumed message from topic {_topic}: {consumeResult.Message.Value}");
                                // Aquí puedes procesar el mensaje recibido según tus necesidades
                            }
                        }
                        catch (ConsumeException ex)
                        {
                            _logger.LogError($"Error consuming message: {ex.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // El token de cancelación se activó, terminando la tarea
                }
                finally
                {
                    consumer.Close();
                }
            }
        }
    }
}
