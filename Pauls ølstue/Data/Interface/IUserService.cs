using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace Data.Interface
{
    public interface IUserService
    {
        User FindByRoomAndPassword(int roomnr, string password);
    }
}
