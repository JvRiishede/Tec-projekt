using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Backend.Viewmodels;
using Data.Interface;
using Model;
using System.Reflection;
using Backend.HelperClasses;

namespace Backend.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IPropertyService _propertyService;

        public OrdersController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        public ActionResult Settings()
        {
            var intervals = new List<SelectListItem>();
            foreach (MailIntervals property in Enum.GetValues(typeof(MailIntervals)))
            {
                intervals.Add(new SelectListItem
                {
                    Text = property.GetDisplayname(),
                    Value = ((int)property).ToString()
                });
            }

            var model = new SettingsViewmodel
            {
                Host = _propertyService.GetProperty(Properties.Host, ""),
                Port = _propertyService.GetProperty(Properties.Port, 0),
                EnableSsl = _propertyService.GetProperty(Properties.EnableSsl, false),
                Email = _propertyService.GetProperty(Properties.Email, ""),
                Password = _propertyService.GetProperty(Properties.Password, ""),
                Displayname = _propertyService.GetProperty(Properties.Displayname, ""),
                Currency = _propertyService.GetProperty(Properties.Currency, "DKK"),
                DisableTestCards = _propertyService.GetProperty(Properties.DisableTestCards, false),
                Subject = _propertyService.GetProperty(Properties.Subject, ""),
                Message = _propertyService.GetProperty(Properties.Message, ""),
                SelectedInterval = (int)_propertyService.GetProperty(Properties.InvoiceInterval, MailIntervals.LastDayInMonth),
                Intervals = intervals
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Settings(SettingsViewmodel model)
        {
            _propertyService.SetProperty(Properties.Host, model.Host);
            _propertyService.SetProperty(Properties.Port, model.Port);
            _propertyService.SetProperty(Properties.EnableSsl, model.EnableSsl);
            _propertyService.SetProperty(Properties.Email, model.Email);
            _propertyService.SetProperty(Properties.Password, model.Password);
            _propertyService.SetProperty(Properties.Displayname, model.Displayname);
            _propertyService.SetProperty(Properties.Currency, model.Currency);
            _propertyService.SetProperty(Properties.DisableTestCards, model.DisableTestCards);
            _propertyService.SetProperty(Properties.Subject, model.Subject);
            _propertyService.SetProperty(Properties.Message, model.Message);
            _propertyService.SetProperty(Properties.InvoiceInterval, model.SelectedInterval);

            return RedirectToAction("Index", "Dashboard");
        }
    }
}