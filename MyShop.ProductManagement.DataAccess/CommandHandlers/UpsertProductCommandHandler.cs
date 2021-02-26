using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.DataAccess.Queries;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.DataAccess.CommandHandlers
{
    public class UpsertProductCommandHandler : IRequestHandler<UpsertProductCommand, Result<Product>>
    {
        private const string InsertCommand = "insert into Products (ProductCode, ProductName) " +
                                             "output inserted.Id, inserted.ProductCode, inserted.ProductName " +
                                             "values (@ProductCode, @ProductName)";


        private const string UpdateCommand = "update Products set ProductCode=@ProductCode, ProductName=@ProductName " +
                                             "output inserted.Id, inserted.ProductCode, inserted.ProductName " +
                                             "where ProductCode=@ProductCode";

        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<UpsertProductCommandHandler> _logger;
        private readonly IMediator _mediator;


        public UpsertProductCommandHandler(IDbConnectionFactory dbConnectionFactory, IMediator mediator, ILogger<UpsertProductCommandHandler> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result<Product>> Handle(UpsertProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var getProductOperation = await _mediator.Send(new GetProductQuery(command.ProductCode), cancellationToken);
                if (!getProductOperation.Status)
                {
                    _logger.LogError("Error when getting the product information.");
                    return Result<Product>.Failure("", "Error when getting the product information.");
                }

                var dataCommand = getProductOperation.Data == null ? InsertCommand : UpdateCommand;

                using (var connection = _dbConnectionFactory.GetConnection())
                {
                    var upsertedProducts = await connection.QueryAsync<Product>(dataCommand, command);
                    var upsertedProduct = upsertedProducts.FirstOrDefault();
                    if (upsertedProduct == null)
                    {
                        _logger.LogError("Error when upserting product {command}", command);
                        return Result<Product>.Failure("", "Error occured when upserting product.");
                    }

                    return Result<Product>.Success(upsertedProduct);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error occured when upserting product {command}", command);
            }

            return Result<Product>.Failure("", "Error occured when upserting product.");
        }
    }
}