using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Model;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;

namespace Data.Service
{
    public class OrderService : IOrderService
    {
        private readonly IConnectionInformationService _connectionInformationService;

        public OrderService( IConnectionInformationService connectionInformationService)
        {
            _connectionInformationService = connectionInformationService;
        }

        public int PlaceOrder(FullOrder order)
        {
            int BrugerId = order.Brugerid;
            decimal FuldPris = order.FuldPris;
            ObservableCollection<OrderItem> orders = order.OrderList;
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "Insert into Ordre (BrugerId, Pris) values (@BrugerId,@FuldPris); select LAST_INSERT_ID()";
                    cmd.Parameters.AddWithValue("@BrugerId", BrugerId);
                    cmd.Parameters.AddWithValue("@FuldPris", FuldPris);
                    int OrdreId = Convert.ToInt32(cmd.ExecuteScalar());
                    
                    cmd.CommandText = "Insert into Ordre_Drink_Vare (OrdreId, DrinkId, VareId) values ";
                    for (int i = 0; i < orders.Count; i++)
                    {
                        for (int j = 0; j < orders[i].Amount; j++)
                        {
                            int DrinkId = 0, VareId = 0;
                            if (orders[i].ErDrink)
                            {
                                DrinkId = orders[i].Id;
                            }
                            else
                                VareId = orders[i].Id;

                            cmd.CommandText += "(" + OrdreId + ", " + DrinkId + ", " + VareId + "), ";
                        }
                    }
                    cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 2);
                    cmd.CommandText += ";";
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int GetOrdreTotal()
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select count(*) as Total from Ordre";
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int GetOrdreTotalForUser(int userId)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select count(*) as Total from Ordre where BrugerId = @userId";
                    cmd.Parameters.AddWithValue("@userId", userId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public List<Order> GetOrdersForYear(int year)
        {
            var result = new List<Order>();
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select Id, BrugerId, Pris, Tidsstempel from Ordre where Tidsstempel between @startdate and @enddate";
                    cmd.Parameters.AddWithValue("@startdate", new DateTime(year, 1, 1));
                    cmd.Parameters.AddWithValue("@enddate", new DateTime(year, 12, 31));
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            result.Add(new Order
                            {
                                Id = (int)dr["Id"],
                                BrugerId = (int)dr["BrugerId"],
                                Pris = (decimal)dr["Pris"],
                                Tidsstempel = (DateTime)dr["Tidsstempel"]
                            });
                        }
                    }
                }
            }
            return result;
        }

        public List<Order> GetOrdersYearForUser(int year, int userId)
        {
            var result = new List<Order>();
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select Id, BrugerId, Pris, Tidsstempel from Ordre where Tidsstempel between @startdate and @enddate and BrugerId = @userId";
                    cmd.Parameters.AddWithValue("@startdate", new DateTime(year, 1, 1));
                    cmd.Parameters.AddWithValue("@enddate", new DateTime(year, 12, 31));
                    cmd.Parameters.AddWithValue("@userId", userId);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            result.Add(new Order
                            {
                                Id = (int)dr["Id"],
                                BrugerId = (int)dr["BrugerId"],
                                Pris = (decimal)dr["Pris"],
                                Tidsstempel = (DateTime)dr["Tidsstempel"]
                            });
                        }
                    }
                }
            }
            return result;
        }
    }
}
