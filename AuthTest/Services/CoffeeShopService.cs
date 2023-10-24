using AutoMapper;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthTest.Services
{
    public class CoffeeShopService : ICoffeeShopService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CoffeeShopService(
            ApplicationDbContext context,
            IMapper mapper
            )
        {
            this._context = context;
            this._mapper = mapper;

        }

        public async Task<CoffeeShop> Create(CoffeeShopDto coffeeShop)
        {

            CoffeeShop coffeeShopEntity =  _mapper.Map<CoffeeShop>(coffeeShop);


             _context.Coffeeshops.Add(coffeeShopEntity);

            await _context.SaveChangesAsync();

            return coffeeShopEntity;
        }

 

        public async Task<List<CoffeeShop>> GetAll()
        {
            try
            {
                var coffeeShops = await _context.Coffeeshops.ToListAsync();
                return coffeeShops;
            }
            catch( Exception ex )
            {
                return new List<CoffeeShop>();
            }
        }

  
        
    }
}
