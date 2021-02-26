﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.DataAccess.Queries
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<Product>>
    {
        private const string query = "select * from Products where productcode=@productCode";
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<GetProductQueryHandler> _logger;

        public GetProductQueryHandler(IDbConnectionFactory dbConnectionFactory, ILogger<GetProductQueryHandler> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<Result<Product>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                using (var connection = _dbConnectionFactory.GetConnection())
                {
                    var commandDefinition = new CommandDefinition(query, new {productCode = request.ProductCode});
                    var product = await connection.QueryFirstOrDefaultAsync<Product>(commandDefinition);

                    return Result<Product>.Success(product);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error occured when getting the product {query}", request);
            }

            return Result<Product>.Failure("", "Error occured when getting the product.");
        }
    }
}