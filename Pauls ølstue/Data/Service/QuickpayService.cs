using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Model;
using Quickpay;
using RestSharp;

namespace Data.Service
{
    public class QuickpayService : QuickPayRestClient, IQuickpayService
    {
        private readonly IPropertyService _propertyService;
        private readonly IOrderService _orderService;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;
        public void CreatePaymentLinks()
        {
            var orders = _orderService.GetUnPaiedOrders();
            var groups = orders.GroupBy(a => a.BrugerId);
            foreach (var group in groups)
            {
                var transaction = new Transaction
                {
                    Beløb = group.Sum(a => a.Pris),
                    OrdreId = group.FirstOrDefault().Id,
                    QuickpayGuid = Guid.NewGuid().ToString()
                };
                var payment = CallEndpoint<dynamic>("payments", a =>
                {
                    a.Method = Method.POST;
                    a.AddParameter("currency", _propertyService.GetProperty(Properties.Currency, "DKK"));
                    a.AddParameter("order_id", "OrderId-" + transaction.OrdreId);
                    a.AddParameter("QuickPay-Callback-Url", "http://localhost:52856/api/paymentcallback");
                    a.AddParameter("variables[genericPaymentid]", transaction.QuickpayGuid);
                });
                string id = Convert.ToString(payment["id"]);
                //decimal getbits returnere int[4], hvor det fjerde element indeholder scaling factor og sign bit
                //Getbytes returnere byte[] af fjerde element, bit 16 til 23 er decimals power of 10. Aka plads 3 i byte array'et
                int count = BitConverter.GetBytes(decimal.GetBits(transaction.Beløb)[3])[2];
                transaction.Beløb *= (decimal)Math.Pow(10, count);
                var link = CallEndpoint<dynamic>("payments/" + id + "/link", a =>
                {
                    a.Method = Method.PUT;
                    a.AddParameter("amount", transaction.Beløb.ToString("####"));
                    a.AddParameter("language", "da");

                });
                _orderService.CreateTransactionForOrders(transaction, group.Select(a => a.Id).ToArray());
                var user = _userService.FindById(group.Key);
                var message = _propertyService.GetProperty(Properties.Message, "").Replace("%link%", string.Format(@"<a href=""{0}"">Betal her</a>", link["url"]));
                _mailService.SendEmail(user.Email, user.Fornavn, _propertyService.GetProperty(Properties.Subject, ""), message);
            }
        }

        public QuickpayService(string username, string password, IPropertyService propertyService, IOrderService orderService, IMailService mailService, IUserService userService) : base(username, password)
        {
            _propertyService = propertyService;
            _orderService = orderService;
            _mailService = mailService;
            _userService = userService;
        }

        public QuickpayService(string apikey, IPropertyService propertyService, IOrderService orderService, IMailService mailService, IUserService userService) : base(apikey)
        {
            _propertyService = propertyService;
            _orderService = orderService;
            _mailService = mailService;
            _userService = userService;
        }
    }
}
