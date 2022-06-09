using ShoppingCart.Data.Entities;
using System.Threading.Tasks;

namespace ShoppingCart.Data.UOW.Interfaces
{
    public interface IBasketItemRepository : IGenericRepository<BasketItem>
    {
        Task AddBasketItemAsync(Basket basket, int productId, int quantity);
        Task UpdateItemAsync(int basketId, int productId, int quantity);
        Task RemoveItemAsync(int basketId, int productId);
    }
}