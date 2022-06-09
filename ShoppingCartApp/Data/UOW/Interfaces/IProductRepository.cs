﻿using ShoppingCart.Data.Entities;
using System.Threading.Tasks;

namespace ShoppingCart.Data.UOW.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
    }
}