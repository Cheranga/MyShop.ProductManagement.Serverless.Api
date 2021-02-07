using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
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

        public async Task<Result<Product>> Handle(UpsertProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //
                // TODO: Validate the command here
                //

                var getProductOperation = await _mediator.Send(new GetProductQuery(request.ProductCode), cancellationToken);
                if (!getProductOperation.Status)
                {
                    _logger.LogError("Error when getting the product information.");
                    return Result<Product>.Failure("", "Error when getting the product information.");
                }

                var command = getProductOperation.Data == null ? InsertCommand : UpdateCommand;

                using (var connection = _dbConnectionFactory.GetConnection())
                {
                    var upsertedProducts = await connection.QueryAsync<Product>(command, request);
                    var upsertedProduct = upsertedProducts.FirstOrDefault();
                    if (upsertedProduct == null)
                    {
                        _logger.LogError("Error when upserting product {command}", request);
                        return Result<Product>.Failure("", "Error occured when upserting product.");
                    }

                    return Result<Product>.Success(upsertedProduct);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error occured when upserting product {command}", request);
            }

            return Result<Product>.Failure("", "Error occured when upserting product.");
        }
    }
}