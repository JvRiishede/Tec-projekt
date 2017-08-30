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
        bool SaveProfile(string firstname, string lastname, string email, int id);
        bool SaveCredentials(int roomnr, string password, int id);
        bool SaveImage(byte[] file, int id);
    }
}
