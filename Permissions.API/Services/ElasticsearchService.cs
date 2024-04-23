using Microsoft.Extensions.Options;
using Nest;
using System;
using Permissions.API.DTOs;
using Permissions.API.Models;
using Permissions.API.Configurations;

namespace Permissions.API.Services
{
    public class ElasticsearchService : IElasticsearchService
    {
        private readonly ElasticClient _client;
        private readonly ElasticsearchConfiguration _elasticsearchConfig;

        public ElasticsearchService(IOptions<ElasticsearchConfiguration> elasticsearchConfig)
        {
            _elasticsearchConfig = elasticsearchConfig.Value;

            var settings = new ConnectionSettings(new Uri(_elasticsearchConfig.Url))
                .DefaultIndex(_elasticsearchConfig.DefaultIndex);

            _client = new ElasticClient(settings);
        }

        public void LogOperation(string operationName, PermissionDTO permissionDto)
        {
            // Crear un registro de operación
            var operationLog = new OperationLog
            {
                Id = Guid.NewGuid(),
                OperationName = operationName,
                PermissionId = permissionDto.Id,
                EmployeeForename = permissionDto.EmployeeForename,
                EmployeeSurname = permissionDto.EmployeeSurname,
                PermissionType = permissionDto.PermissionTypeId,
                PermissionDate = permissionDto.PermissionDate
            };

            // Enviar el registro a Elasticsearch
            _client.IndexDocument(operationLog);
        }
    }
}
