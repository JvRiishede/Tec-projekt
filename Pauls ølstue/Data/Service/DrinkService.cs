using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Model;
using MySql.Data.MySqlClient;

namespace Data.Service
{
    public class DrinkService : IDrinkService
    {
        private readonly IVareService _vareService;
        private readonly IConnectionInformationService _connectionInformationService;

        public DrinkService(IVareService vareService, IConnectionInformationService connectionInformationService)
        {
            _vareService = vareService;
            _connectionInformationService = connectionInformationService;
        }

        public Drink GetDrink(int id)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = @"select Drink.Id DrinkId, Drink.Navn DrinkNavn, Drink.Tidsstempel DrinkStempel, Vare.Id VareId, Vare.Navn VareNavn, Vare.Pris VarePris, Vare.Tidsstempel VareStempel from Drink
                    left join Vare_Drink on Drink.Id = Vare_Drink.DrinkId
                    left join Vare on Vare_Drink.VareId = Vare.Id
                    where Drink.Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var dr = cmd.ExecuteReader())
                    {
                        var drink = new Drink();
                        drink.Ingrediense = new List<Vare>();
                        while (dr.Read())
                        {
                            if (drink.Id <= 0)
                            {
                                drink.Id = (int) dr["DrinkId"];
                                drink.Navn = (string) dr["DrinkNavn"];
                                drink.Tidsstempel = (DateTime) dr["DrinkStempel"];
                                drink.Ingrediense.Add(new Vare
                                {
                                    Id = (int) dr["VareId"],
                                    Navn = (string) dr["VareNavn"],
                                    Pris = (decimal) dr["VarePris"],
                                    Tidsstempel = (DateTime) dr["VareStempel"],
                                });
                            }
                            else
                            {
                                drink.Ingrediense.Add(new Vare
                                {
                                    Id = (int)dr["VareId"],
                                    Navn = (string)dr["VareNavn"],
                                    Pris = (decimal)dr["VarePris"],
                                    Tidsstempel = (DateTime)dr["VareStempel"],
                                });
                            }
                        }
                        return drink;
                    }
                }
            }
        }

        public List<Drink> GetDrinks()
        {
            var result = new List<Drink>();
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = @"select Drink.Id DrinkId, Drink.Navn DrinkNavn, Drink.Tidsstempel DrinkStempel, Vare.Id VareId, Vare.Navn VareNavn, Vare.Pris VarePris, Vare.Tidsstempel VareStempel from Drink
                    left join Vare_Drink on Drink.Id = Vare_Drink.DrinkId
                    left join Vare on Vare_Drink.VareId = Vare.Id";
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            var drinkId = (int) dr["DrinkId"];
                            var drinkNavn = (string)dr["DrinkNavn"];
                            var drinkStempel = (DateTime)dr["DrinkStempel"];
                            var vareId = (int) dr["VareId"];
                            var vareNavn = (string) dr["VareNavn"];
                            var varePris = (decimal) dr["VarePris"];
                            var vareStempel = (DateTime) dr["VareStempel"];


                            var drink = result.FirstOrDefault(a => a.Id == drinkId);

                            if (drink == null)
                            {
                                drink = new Drink
                                {
                                    Id = drinkId,
                                    Navn = drinkNavn,
                                    Ingrediense = new List<Vare>
                                    {
                                        new Vare
                                        {
                                            Id = vareId,
                                            Navn = vareNavn,
                                            Pris = varePris,
                                            Tidsstempel = vareStempel
                                        }
                                    },
                                    Tidsstempel = drinkStempel

                                };
                                result.Add(drink);
                            }
                            else
                            {
                                drink.Ingrediense.Add(new Vare
                                {
                                    Id = vareId,
                                    Navn = vareNavn,
                                    Pris = varePris,
                                    Tidsstempel = vareStempel
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public int CreateDrink(string navn)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "insert into Vare(Navn) values(@Navn); select LAST_INSERT_ID()";
                    cmd.Parameters.AddWithValue("@Navn", navn);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public bool UpdateDrink(Drink drink)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "update Drink set Navn = @Navn where Id = @Id";
                    cmd.Parameters.AddWithValue("@Navn", drink.Navn);
                    cmd.Parameters.AddWithValue("@Id", drink.Id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteDrink(int id)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "delete from Drink where Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
