using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Data.Interface;
using Model;
using WebAPI.Classes;

namespace WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    [AuthorizeApi]
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
        public object GetPagedProducts([FromBody] ProductSearchTerms terms)
        {
            var drinks = _drinkService.GetPagedDrinks(terms);
            if ((drinks == null || drinks.Count == 0) && terms.Page != 0)
            {
                terms.Page--;
                drinks = _drinkService.GetPagedDrinks(terms);
            }
            return new
            {
                Total = _drinkService.GetDrinksTotal(terms.SearchText),
                Drinks = drinks
            };
        }

        [HttpPost]
        public Drink CreateProduct(string navn)
        {
            var id = _drinkService.CreateDrink(navn);
            return _drinkService.GetDrink(id);
        }

        [HttpPost]
        public Drink EditProduct([FromBody] Drink drink)
        {
            if (drink.Id > 0)
            {
                var tempDrink = _drinkService.GetDrink(drink.Id);
                tempDrink.Navn = drink.Navn;
                tempDrink.Ingrediense = drink.Ingrediense;
                _drinkService.UpdateDrink(tempDrink);
                return tempDrink;
            }
            return null;
        }

        [HttpPost]
        public bool DeleteProduct(int id)
        {
            return _drinkService.DeleteDrink(id);
        }
    }
}