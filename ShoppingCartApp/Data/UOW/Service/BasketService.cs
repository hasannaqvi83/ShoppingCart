using ShoppingCart.Data.Entities;
using ShoppingCart.Data.UOW.Interfaces;
using ShoppingCart.Identity.User;
using System.Threading.Tasks;


namespace UOW.Service.Service
{
    public class BasketService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IMapper _mapper;
        private IUserIdentity _user;

        public BasketService(
            IUnitOfWork unitOfWork,
            IUserIdentity user
            //IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            //_mapper = mapper;
            _user = user;
        }

        public async Task<Basket> CreateBasketAsync(string userId)
        {
            var basket = new Basket { BuyerId = userId };
            await _unitOfWork.Baskets.AddAsync(basket);
            return basket;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
