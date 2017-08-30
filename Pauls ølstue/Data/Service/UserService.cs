using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private User BindUser(MySqlDataReader dr, bool bindPicture = false)
        {
            var user = new User();
            if (dr != null)
            {
                user.Id = (int) dr["Id"];
                user.Fornavn = (string)dr["Fornavn"];
                user.Efternavn = (string) dr["Efternavn"];
                user.Email = (string) dr["Email"];
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
