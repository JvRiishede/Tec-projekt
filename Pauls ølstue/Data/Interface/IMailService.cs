using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interface
{
    public interface IMailService
    {
        bool SendEmail(string email, string name, string subject, string body);
    }
}
