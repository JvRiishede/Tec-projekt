using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Data.Interface;
using Model;

namespace WebAPI.Controllers
{
    public class QuickPayController : ApiController
    {
        private readonly IPropertyService _propertyService;

        public QuickPayController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpPost]
        public bool SaveSettings(MailIntervals intervals, string currency, bool disableTestCards)
        {
            bool check;
            check = _propertyService.SetProperty(Properties.InvoiceInterval, (int)intervals);
            check = _propertyService.SetProperty(Properties.Currency, currency);
            check = _propertyService.SetProperty(Properties.DisableTestCards, disableTestCards);
            return check;
        }

        [HttpGet]
        public object GetSettings()
        {
            var interval = _propertyService.GetProperty(Properties.InvoiceInterval, MailIntervals.FirstDayInMonth);
            var currency = _propertyService.GetProperty(Properties.Currency, "tsk");
            var disableTestCards = _propertyService.GetProperty(Properties.DisableTestCards, true);
            return new {interval, currency, disableTestCards};
        }
    }
}
