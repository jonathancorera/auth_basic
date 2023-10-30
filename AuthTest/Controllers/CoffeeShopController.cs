using AuthTest.Services;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AuthTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoffeeShopController : ControllerBase
    {

        private readonly ICoffeeShopService _coffeeShopService;

        public CoffeeShopController(ICoffeeShopService coffeeShopService)
        {
            this._coffeeShopService = coffeeShopService;
        }



        [HttpGet]
        public async Task<IActionResult> List ()
        {
           var coffeeshops =  await _coffeeShopService.GetAll();

            return Ok(coffeeshops);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CoffeeShopDto coffeeShop)
        {
            try
            {
                return Ok(await _coffeeShopService.Create(coffeeShop));

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
