using Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model;

namespace WebAPI.Controllers
{
    public class VarerController : ApiController
    {
        private readonly IVareService _vareService;

        public VarerController(IVareService vareService)
        {
            _vareService = vareService;
        }

        public Vare GetProduct(int id)
        {
            return new Vare();
        }

        public List<Vare> GetProducts()
        {
            return null;
        }
    }
}
