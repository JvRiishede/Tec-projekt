using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes;
using Model;

namespace Data.Interface
{
    public interface IHelpService
    {
        List<Help> GetAllHelp();
    }
}
