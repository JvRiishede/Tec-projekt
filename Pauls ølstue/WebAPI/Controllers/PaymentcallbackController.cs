using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Data.Interface;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    public class PaymentcallbackController : ApiController
    {
        private readonly IOrderService _orderService;

        public PaymentcallbackController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public HttpResponseMessage Post()
        {
            var checkSum = HttpContext.Current.Request.Headers["QuickPay-Checksum-Sha256"];
            var resourcetype = HttpContext.Current.Request.Headers["QuickPay-Resource-Type"];
            var accountid = HttpContext.Current.Request.Headers["QuickPay-Account-ID"];
            var apiversion = HttpContext.Current.Request.Headers["QuickPay-API-Version"];

            var bytes = new byte[HttpContext.Current.Request.InputStream.Length];
            HttpContext.Current.Request.InputStream.Read(bytes, 0, bytes.Length);
            HttpContext.Current.Request.InputStream.Position = 0;
            var content = System.Text.Encoding.UTF8.GetString(bytes);

            var obj = JsonConvert.DeserializeObject<dynamic>(content);

            _orderService.SetOrdersToPaid(obj.variables.genericPaymentid);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
