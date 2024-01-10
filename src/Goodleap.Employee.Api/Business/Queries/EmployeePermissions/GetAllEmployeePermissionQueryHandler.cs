using Goodleap.Employee.Api.Controllers;
using Goodleap.Employee.Api.DTOs;
using Goodleap.Employee.Api.Services.ElasticSearch;
using MediatR;
using Nest;

namespace Goodleap.Employee.Api.Business.Queries.EmployeePermissions
{
    public class GetAllEmployeePermissionQueryHandler : IRequestHandler<GetAllEmployeePermissionQuery, List<UpdatePermissionDto>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GetAllEmployeePermissionQueryHandler> _logger;

        public GetAllEmployeePermissionQueryHandler(IElasticClient elasticClient, IConfiguration configuration, ILogger<GetAllEmployeePermissionQueryHandler> logger)
        {
            _elasticClient = elasticClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<UpdatePermissionDto>> Handle(GetAllEmployeePermissionQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{this.GetType().Name} - Retrieving information from Elasticsearch");
            var baseUrl = _configuration["ElasticSettings:baseUrl"];
            var connectionSettings = new ConnectionSettings(new Uri(baseUrl))
                .EnableApiVersioningHeader();
            var client = new ElasticClient(connectionSettings);
            var elasticsearch = new ElasticSearch(client, _configuration["ElasticSettings:defaultIndex"]);

            return await elasticsearch.GetAll<UpdatePermissionDto>();
        }
    }
}
