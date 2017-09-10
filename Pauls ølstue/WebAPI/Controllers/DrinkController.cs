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

        public class EditProductData
        {
            public int DrinkId { get; set; }
            public string Navn { get; set; }
            public int[] VareIds { get; set; }
        }

        [HttpPost]
        public bool EditProduct([FromBody] EditProductData editProduct)
        {
            var drink = new Drink
            {
                Id = editProduct.DrinkId,
                Navn = editProduct.Navn
            };
            _drinkService.UpdateDrink(drink);
            _drinkService.RemoveVareFromDrink(editProduct.DrinkId, editProduct.VareIds);
            var exceptList = _drinkService.GetDrink(editProduct.DrinkId).Ingrediense.Select(a => a.Id);
            return _drinkService.AddVareToDrink(editProduct.DrinkId, editProduct.VareIds.Except(exceptList).ToArray());
        }

        [HttpPost]
        public bool DeleteProduct(int id)
        {
            return _drinkService.DeleteDrink(id);
        }

    }
}