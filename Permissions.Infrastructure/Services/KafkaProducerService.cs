using Confluent.Kafka;
using Newtonsoft.Json;

using Permissions.API.DTOs;

namespace Permissions.Infrastructure.Services
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly ProducerConfig _producerConfig;
        private readonly string _topicName;

        public KafkaProducerService(string bootstrapServers, string topicName)
        {
            _producerConfig = new ProducerConfig { BootstrapServers = bootstrapServers };
            _topicName = topicName;
        }

        public async Task ProducePermissionMessageAsync(PermissionDTO permissionDTO, string operation)
        {
            using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {
                try
                {
                    var message = new
                    {
                        Id = Guid.NewGuid(),
                        Operation = operation,
                        Permission = permissionDTO
                    };

                    var messageJson = JsonConvert.SerializeObject(message);
                    var result = await producer.ProduceAsync(_topicName, new Message<Null, string> { Value = messageJson });

                    Console.WriteLine($"Message delivered to topic {result.Topic}, partition {result.Partition}, offset {result.Offset}");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}
