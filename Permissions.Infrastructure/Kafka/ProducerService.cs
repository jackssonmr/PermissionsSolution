using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Permissions.Infrastructure.Models;

namespace Permissions.Infrastructure.Kafka
{
    public class ProducerService
    {
        private readonly ProducerConfig _producerConfig;
        private readonly ILogger<ProducerService> _logger;
        private readonly string _topic;

        public ProducerService(ProducerConfig producerConfig, ILogger<ProducerService> logger, string topic)
        {
            _producerConfig = producerConfig;
            _logger = logger;
            _topic = topic;
        }

        public async Task ProduceAsync<T>(string topic, T message)
        {
            using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {
                try
                {
                    var messageJson = JsonConvert.SerializeObject(message);
                    var deliveryResult = await producer.ProduceAsync(topic, new Message<Null, string> { Value = messageJson });
                    _logger.LogInformation($"Delivered message to {deliveryResult.TopicPartitionOffset}");
                }
                catch (ProduceException<Null, string> ex)
                {
                    _logger.LogError($"Failed to deliver message: {ex.Error.Reason}");
                    throw;
                }
            }
        }
        
        public void SendMessage(Permission permission)
        {
            using (var producer = new ProducerBuilder<string, Permission>(_producerConfig).Build())
            {
                producer.Produce(_topic, new Message<string, Permission> { Key = permission.Id.ToString(), Value = permission });
            }
        }
    }
}
