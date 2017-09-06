using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Data.Interface;
using Model;
using WebAPI.Classes;
using System.Collections.ObjectModel;

namespace WebAPI.Controllers
{
    [AllowCrossSiteJson]
    public class OrderController : ApiController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public int PlaceOrder(FullOrder orders)
        {
            return _orderService.PlaceOrder(orders);
        }
    }
}
