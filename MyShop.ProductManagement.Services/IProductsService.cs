﻿using System.Threading.Tasks;
using MyShop.ProductManagement.Core;
using MyShop.ProductManagement.DataAccess.Models;
using MyShop.ProductManagement.Services.Requests;

namespace MyShop.ProductManagement.Api.Services
{
    public interface IProductsService
    {
        Task<Result<ProductDataModel>> UpsertProductAsync(UpsertProductRequest request);
        Task<Result<ProductDataModel>> GetProductAsync(GetProductRequest request);
    }
}