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
using System.Web.Http.Cors;

namespace WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    [AuthorizeApi]
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

        [HttpGet]
        public object GetOrderOverview()
        {
            var ordersCount = new int[12];
            var ordersTotal = new decimal[12];
            var orders = _orderService.GetOrdersForYear(DateTime.Now.Year);
            foreach (var item in orders)
            {
                ordersCount[item.Tidsstempel.Month - 1] += 1;
                ordersTotal[item.Tidsstempel.Month - 1] += item.Pris;
            }

            return new {ordersCount, ordersTotal};
        }
    }
}
