using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Application.Constants;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.DataAccess.CommandHandlers
{
    public class InsertProductCommandHandler : IRequestHandler<InsertProductCommand, Result<Product>>
    {
        private const string InsertCommand = "insert into Products (ProductCode, ProductName) " +
                                             "output inserted.Id, inserted.ProductCode, inserted.ProductName " +
                                             "values (@ProductCode, @ProductName)";

        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<InsertProductCommandHandler> _logger;

        public InsertProductCommandHandler(IDbConnectionFactory dbConnectionFactory, ILogger<InsertProductCommandHandler> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }


        public async Task<Result<Product>> Handle(InsertProductCommand request, CancellationToken cancellationToken)
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
                            var updatedProduct = await connection.QuerySingleOrDefaultAsync<Product>(InsertCommand, request, transaction);
                            transaction.Commit();
                            return Result<Product>.Success(updatedProduct);
                        }
                        catch (Exception exception)
                        {
                            _logger.LogError(exception, "Error occured when inserting product. {@product}", request);
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error occured when operating on the Products table.");
            }

            return Result<Product>.Failure(ErrorCodes.DataAccessError, "Error occured when inserting product.");
        }
    }
}