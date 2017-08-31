using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Data.Interface;
using MySql.Data;
using Model;
using Data.Classes;
using MySql.Data.MySqlClient;


namespace Data.Service
{
    public class UserService:IUserService
    {
        private readonly IConnectionInformationService _connectionInformationService;

        public UserService(IConnectionInformationService connectionInformationService)
        {
            _connectionInformationService = connectionInformationService;
        }

        public int UserId
        {
            get
            {
                var requestCookie = HttpContext.Current.Request
                    .Cookies[FormsAuthentication.FormsCookieName];
                if (requestCookie != null)
                {
                    var authTicket = FormsAuthentication.Decrypt(requestCookie.Value);
                    if (authTicket != null)
                    {
                        return int.Parse(authTicket.UserData.Split(':')[0]);
                    }
                }

                return -1;
            }
        }

        public User FindByRoomAndPassword(int roomnr, string password)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select Id, Fornavn, Efternavn, VærelseNr, Email, KodeHash, Billede, Type from Bruger where VærelseNr = @roomnr";
                    cmd.Parameters.AddWithValue("@roomnr", roomnr);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (PasswordHash.Validate(password, (string)dr["KodeHash"]))
                            {
                                return BindUser(dr);
                            }
                        }
                    }

                }
            }
            return null;
        }

        public bool SaveProfile(string firstname, string lastname, string email, int id)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "update Bruger set Fornavn = @firstname, Efternavn = @lastname, Email = @email where id = @id";
                    cmd.Parameters.AddWithValue("@firstname", firstname);
                    cmd.Parameters.AddWithValue("@lastname", lastname);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@id", id);
                    var count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        public bool SaveCredentials(int roomnr, string password, int id)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "update Bruger set VærelseNr = @roomnr, KodeHash = @password  where id = @id";
                    cmd.Parameters.AddWithValue("@roomnr", roomnr);
                    cmd.Parameters.AddWithValue("@password", PasswordHash.Hash(password));
                    cmd.Parameters.AddWithValue("@id", id);
                    var count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        return true;
                    }

                }
            }
            return false;
        }


        public bool SaveImage(byte[] file, int id)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "update Bruger set Billede = @image  where id = @id";
                    cmd.Parameters.AddWithValue("@image", file);
                    cmd.Parameters.AddWithValue("@id", id);
                    var count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        public User FindById(int id, bool bindPicture = false)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select Id, Fornavn, Efternavn, VærelseNr, Email, KodeHash, Billede, Type from Bruger where Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            return BindUser(dr, bindPicture);
                        }
                    }

                }
            }
            return null;
        }

        public IEnumerable<User> GetUsers()
        {
            var result = new List<User>();
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select Id, Fornavn, Efternavn, VærelseNr, Email, KodeHash, Billede, Type from Bruger";
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                             result.Add(BindUser(dr));
                        }
                    }

                }
            }
            return result;
        }

        public byte[] GetUserImage(int userid)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select Billede from Bruger where Id = @id";
                    cmd.Parameters.AddWithValue("@id", userid);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            return (byte[]) dr["Billede"];
                        }
                    }

                }
            }
            return new byte[0];
        }

        private User BindUser(MySqlDataReader dr, bool bindPicture = false)
        {
            var user = new User();
            if (dr != null)
            {
                user.Id = (int) dr["Id"];
                user.Fornavn = (string)dr["Fornavn"];
                user.Efternavn = (string) dr["Efternavn"];
                user.Email = (string) dr["Email"];
                user.VærelseNr = (int) dr["VærelseNr"];
                if (bindPicture)
                {
                    user.Billede = (byte[]) dr["Billede"];
                }
                user.Role = dr["Type"] == DBNull.Value ? Role.Bartender : (Role)dr["Type"];
            }
            return user;
        }


    }
}
