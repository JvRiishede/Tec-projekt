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
    }
}
