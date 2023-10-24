using AutoMapper;
using DataAccess.Entities;

namespace AuthTest.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CoffeeShop, CoffeeShopDto>();
            CreateMap<CoffeeShopDto, CoffeeShop>();
        }
    }
}
