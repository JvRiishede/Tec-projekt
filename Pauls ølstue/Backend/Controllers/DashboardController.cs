using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Backend.HelperClasses;
using Backend.Viewmodels;
using Data.Interface;
using Model;

namespace Backend.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IVareService _vareService;
        private readonly IDrinkService _drinkService;
        private readonly IOrderService _orderService;
        public DashboardController(IUserService userService, IVareService vareService, IDrinkService drinkService, IOrderService orderService)
        {
            _userService = userService;
            _vareService = vareService;
            _drinkService = drinkService;
            _orderService = orderService;
        }
        // GET: Dashboard
        public ActionResult Index()
        {
            var model = new DashboardViewmodel
            {
                UserTotal = _userService.GetUserTotal(),
                DrinksTotal = _drinkService.GetDrinksTotal(),
                VarerTotal = _vareService.GetVareTotal(),
                OrdreTotal = _orderService.GetOrdreTotal(),
                TopDrink = _drinkService.GetTopMostSold(),
                TopVare = _vareService.GetTopMostSold(),
                TopBruger = _userService.GetTopBuyers()
            };
            return View(model);
        }
    }
}