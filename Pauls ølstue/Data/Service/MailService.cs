using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using System.Net;
using System.Net.Mail;

namespace Data.Service
{
    public class MailService :IMailService
    {
        public bool SendEmail(string email, string name)
        {
            var fromAddress = new MailAddress("paulsoelbar@gmail.com", "Paul");
            var toAddress = new MailAddress(email, name);
            const string fromPassword = "Paulsdata";
            const string subject = "Betal";
            const string body = "Betal";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            try
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
