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
        public Vare CreateProduct(string navn, decimal pris)
        {
            var id = _vareService.CreateVare(navn, pris);
            return _vareService.GetVare(id);
        }

        [HttpPost]
        
        public object GetPagedProducts([FromBody]ProductSearchTerms terms)
        {

            return new
            {
                Total = _vareService.GetVareTotal(),
                Varer = _vareService.GetPagedVare(terms)
            };
        }
    }
}
