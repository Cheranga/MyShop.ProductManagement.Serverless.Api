using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Core;
using MyShop.ProductManagement.DataAccess.Models;
using MyShop.ProductManagement.DataAccess.Queries;

namespace MyShop.ProductManagement.DataAccess.CommandHandlers
{
    public class UpsertProductCommandHandler : IRequestHandler<UpsertProductCommand, Result<ProductDataModel>>
    {
        private const string InsertCommand = "insert into Products (ProductCode, ProductName) " +
                                             "output inserted.Id, inserted.ProductCode, inserted.ProductName " +
                                             "values (@ProductCode, @ProductName)";


        private const string UpdateCommand = "update Products set ProductCode=@ProductCode, ProductName=@ProductName " +
                                             "output inserted.Id, inserted.ProductCode, inserted.ProductName " +
                                             "where ProductCode=@ProductCode";

        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IMediator _mediator;
        private readonly ILogger<UpsertProductCommandHandler> _logger;


        public UpsertProductCommandHandler(IDbConnectionFactory dbConnectionFactory, IMediator mediator, ILogger<UpsertProductCommandHandler> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result<ProductDataModel>> Handle(UpsertProductCommand request, CancellationToken cancellationToken)
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
                    return Result<ProductDataModel>.Failure("", "Error when getting the product information.");
                }

                var command = getProductOperation.Data == null ? InsertCommand : UpdateCommand;

                using (var connection = _dbConnectionFactory.GetConnection())
                {
                    var upsertedProducts = await connection.QueryAsync<ProductDataModel>(command, request);
                    var upsertedProduct = upsertedProducts.FirstOrDefault();
                    if (upsertedProduct == null)
                    {
                        _logger.LogError("Error when upserting product {command}", request);
                        return Result<ProductDataModel>.Failure("", "Error occured when upserting product.");
                    }

                    return Result<ProductDataModel>.Success(upsertedProduct);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error occured when upserting product {command}", request);
            }

            return Result<ProductDataModel>.Failure("", "Error occured when upserting product.");
        }
    }
}