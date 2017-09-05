using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Data.Interface;
using Model;
using WebAPI.Classes;

namespace WebAPI.Controllers
{
    [AllowCrossSiteJson]
    public class DrinkController : ApiController
    {
        private readonly IDrinkService _drinkService;

        public DrinkController(IDrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        public Drink GetProduct(int id)
        {
            return _drinkService.GetDrink(id);
        }

        public List<Drink> GetProducts()
        {
            return _drinkService.GetDrinks();
        }

        [HttpPost]
        public Drink CreateProduct(string navn)
        {
            var id = _drinkService.CreateDrink(navn);
            return _drinkService.GetDrink(id);
        }
    }
}
