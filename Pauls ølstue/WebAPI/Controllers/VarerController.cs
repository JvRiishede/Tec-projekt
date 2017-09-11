using Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Web.Http.Cors;
using WebAPI.Classes;
using Vare = Model.Vare;

namespace WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class VarerController : ApiController
    {
        private readonly IVareService _vareService;

        public VarerController(IVareService vareService)
        {
            _vareService = vareService;
        }

        public Vare GetProduct(int id)
        {
            return _vareService.GetVare(id);
        }

        public List<Vare> GetProducts()
        {
            return _vareService.GetAllVare();
        }

        

        [HttpPost]
        public Vare EditProduct(string navn, decimal pris, int id = 0)
        {

            if (id > 0)
            {
                var vare = _vareService.GetVare(id);
                vare.Navn = navn;
                vare.Pris = pris;
                _vareService.UpdateVare(vare);
                return vare;
            }
            var newId = _vareService.CreateVare(navn, pris);
            return _vareService.GetVare(newId);
        }

        [HttpPost]
        public bool DeleteProduct(int id)
        {
            return _vareService.DeleteVare(id);
        }

        [HttpPost]
        public object GetPagedProducts([FromBody] ProductSearchTerms terms)
        {
            var varer = _vareService.GetPagedVare(terms);
            if (varer == null || varer.Count == 0 && terms.Page != 0)
            {
                terms.Page--;
                varer = _vareService.GetPagedVare(terms);
            }
            return new
            {
                Total = _vareService.GetVareTotal(terms.SearchText),
                Varer = varer
            };
        }

        [HttpGet]
        public object GetStats(int id)
        {
            var solgtTotal = new int[12];
            var solgtPrisTotal = new decimal[12];
            var orders = _vareService.GetVarerForYear(DateTime.Now.Year, id);
            foreach (var item in orders)
            {
                //Item1 = tidsstempel, Item2 = Sum af vare priser
                solgtTotal[item.Item1.Month - 1] += 1;
                solgtPrisTotal[item.Item1.Month - 1] += item.Item2;
            }
            return new
            {
                SolgtTotal = _vareService.GetTotalSold(id),
                SolgtTotalYear = solgtTotal,
                SolgtPriceTotalYear = solgtPrisTotal
            };
        }
    }
}