﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.DataAccess.CommandHandlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<Product>>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        private const string UpdateCommand = "update Products set ProductCode=@ProductCode, ProductName=@ProductName " +
                                             "output inserted.Id, inserted.ProductCode, inserted.ProductName " +
                                             "where ProductCode=@ProductCode";

        public UpdateProductCommandHandler(IDbConnectionFactory dbConnectionFactory, ILogger<UpdateProductCommandHandler> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }


        public async Task<Result<Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using (var connection = _dbConnectionFactory.GetConnection())
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var updatedProduct = await connection.QuerySingleOrDefaultAsync<Product>(UpdateCommand, request, transaction);
                            return Result<Product>.Success(updatedProduct);
                        }
                        catch (Exception exception)
                        {
                            _logger.LogError(exception, "Error occured when updating product. {@product}", request);
                            transaction.Rollback();
                        }
                        finally
                        {
                            transaction.Commit();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error occured when operating on the Products table.");
            }

            return Result<Product>.Failure("", "Error occured when updating product.");
        }
    }
}