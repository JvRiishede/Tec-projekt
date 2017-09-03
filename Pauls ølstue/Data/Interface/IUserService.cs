using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes;
using Model;
namespace Data.Interface
{
    public interface IUserService
    {
        int UserId { get; }
        User FindByRoomAndPassword(int roomnr, string password);
        bool SaveProfile(int id, string firstname, string lastname, string email, int roleId);
        bool SaveCredentials(int roomnr, string password, int id);
        bool SaveImage(byte[] file, int id);
        byte[] GetUserImage(int userid);
        User FindById(int id, bool bindPicture = false);
        IEnumerable<User> GetUsers();
        IEnumerable<Role> GetRoles();
        bool CreateUser(User user, string password);
        bool DeleteUser(int id);

        IEnumerable<User> SearchUsers(string searchText, UserSort sort);

    }
}
