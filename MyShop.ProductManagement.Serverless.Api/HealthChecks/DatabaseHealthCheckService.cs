﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyShop.ProductManagement.Application.Requests;

namespace MyShop.ProductManagement.Serverless.Api.HealthChecks
{
    public class DatabaseHealthCheckService : IHealthCheck
    {
        private readonly IMediator _mediator;

        public DatabaseHealthCheckService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var databaseHealthCheckRequest = new DatabaseHealthCheckRequest("HEALTH_CHECK");

            var operation = await _mediator.Send(databaseHealthCheckRequest, cancellationToken);

            if (operation.Status)
            {
                return HealthCheckResult.Healthy();
            }

            var errorData = operation.Validation.Errors.ToDictionary(x => x.ErrorCode, x => (object) x.ErrorMessage);

            return HealthCheckResult.Unhealthy("Accessing product data", data: errorData);
        }
    }
}