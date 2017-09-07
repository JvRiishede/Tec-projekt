using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Data.Classes;
using Model;
using MySql.Data.MySqlClient;

namespace Data.Service
{
    public class HelpService :IHelpService
    {
        private readonly IConnectionInformationService _connectionInformationService;

        public HelpService(IConnectionInformationService connectionInformationService)
        {
            _connectionInformationService = connectionInformationService;
        }
        public List<Help> GetAllHelp()
        {
            var result = new List<Help>();

            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select Id, Navn, Tekst from Help";
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            result.Add(new Help
                            {
                                Id = (int)dr["Id"],
                                Navn = (string)dr["Navn"],
                                Text = (string)dr["Tekst"]
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
