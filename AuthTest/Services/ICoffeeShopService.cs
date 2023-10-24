using DataAccess.Entities;

namespace AuthTest.Services
{
    public interface ICoffeeShopService
    {

        Task<List<CoffeeShop>>  GetAll();

        Task<CoffeeShop> Create(CoffeeShopDto coffeeShop);

    }
}
