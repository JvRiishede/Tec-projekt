using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using Ninject.Web.WebApi;
using Data.Interface;
using Model;
using System.Web.Http.Cors;

namespace WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IDrinkService _drinkService;
        private readonly IVareService _vareService;

        public UsersController(IUserService userService, IOrderService orderService, IDrinkService drinkService, IVareService vareService)
        {
            _userService = userService;
            _orderService = orderService;
            _drinkService = drinkService;
            _vareService = vareService;
        }

        // GET: api/Users
        public List<User> GetUsers()
        {
            return _userService.GetUsers().ToList();
        }

        [HttpGet]
        public object GetUserStats(int userId)
        {
            var ordersCount = new int[12];
            var ordersTotal = new decimal[12];
            var orders = _orderService.GetOrdersYearForUser(DateTime.Now.Year, userId);
            foreach (var item in orders)
            {
                ordersCount[item.Tidsstempel.Month - 1] += 1;
                ordersTotal[item.Tidsstempel.Month - 1] += item.Pris;
            }
            return new
            {
                OrdreTotal = _orderService.GetOrdreTotalForUser(userId),
                Drinks = _drinkService.GetDrinksTotalForUser(userId),
                Varer = _vareService.GetVareTotalForUser(userId),
                UserPlace = _userService.GetUserBuyerPlace(userId),
                OrdreTotalYear = ordersCount,
                OrdrePriceTotalYear = ordersTotal
            };
        }

        [HttpPost]
        public object GetPageduser([FromBody]UserSearchTerms terms)
        {
            return new
            {
                Total=_userService.GetUserTotal(),
                User = _userService.GetPagedUser(terms)
            };
        }
    }
}
