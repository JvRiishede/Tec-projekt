using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using System.Net;
using System.Net.Mail;
using Model;

namespace Data.Service
{
    public class MailService :IMailService
    {
        private readonly IPropertyService _propertyService;

        public MailService(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        public bool SendEmail(string email, string name, string subject, string body)
        {
            
            var fromAddress = new MailAddress(_propertyService.GetProperty(Properties.Email, ""), _propertyService.GetProperty(Properties.Displayname, "Paul"));
            var toAddress = new MailAddress(email, name);
            var fromPassword = _propertyService.GetProperty(Properties.Password, "Paulsdata");

            var smtp = new SmtpClient
            {
                Host = _propertyService.GetProperty(Properties.Host, "smtp.gmail.com"),
                Port = _propertyService.GetProperty(Properties.Port, 587),
                EnableSsl = _propertyService.GetProperty(Properties.EnableSsl, false),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            try
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
                return true;
            }
            catch(Exception e)
            {
                var test = e;
                return false;
            }
        }
    }
}
