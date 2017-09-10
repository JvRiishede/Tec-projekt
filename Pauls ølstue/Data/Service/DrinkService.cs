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
                                if (dr["VareId"] != DBNull.Value)
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
                            var vareId = dr["VareId"] != DBNull.Value ? (int)dr["VareId"] : 0;
                            var vareNavn = dr["VareNavn"] != DBNull.Value ? (string)dr["VareNavn"] : "";
                            var varePris = dr["VarePris"] != DBNull.Value ? (decimal)dr["VarePris"] : 0;
                            var vareStempel = dr["VareStempel"] != DBNull.Value ? (DateTime)dr["VareStempel"] : DateTime.MinValue;


                            var drink = result.FirstOrDefault(a => a.Id == drinkId);

                            if (drink == null)
                            {
                                drink = new Drink
                                {
                                    Id = drinkId,
                                    Navn = drinkNavn,
                                    Tidsstempel = drinkStempel

                                };
                                if (vareId != 0)
                                {
                                    drink.Ingrediense = new List<Vare>
                                    {
                                        new Vare
                                        {
                                            Id = vareId,
                                            Navn = vareNavn,
                                            Pris = varePris,
                                            Tidsstempel = vareStempel
                                        }
                                    };
                                }
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
                    cmd.CommandText = "insert into Drink(Navn) values(@Navn); select LAST_INSERT_ID()";
                    cmd.Parameters.AddWithValue("@Navn", navn);
                    return Convert.ToInt32(cmd.ExecuteScalar());
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

        public bool AddVareToDrink(int drinkId, int[] ids)
        {
            var check = false;
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    foreach (var id in ids)
                    {
                        cmd.CommandText = "insert into Vare_Drink(VareId, DrinkId) values(@vareid, @drinkid)";
                        cmd.Parameters.AddWithValue("@vareid", id);
                        cmd.Parameters.AddWithValue("@drinkid", drinkId);
                        check = cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            return check;
        }

        public bool RemoveVareFromDrink(int drinkId, int[] ids)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "delete from Vare_Drink where drinkid = @drinkid and VareId not in ([IDS])";
                    cmd.CommandText = cmd.CommandText.Replace("[IDS]", string.Join(",", ids));
                    cmd.Parameters.AddWithValue("@drinkid", drinkId);

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

        public List<Drink> GetPagedDrinks(ProductSearchTerms terms)
        {
            var result = new List<Drink>();
            var sql = @"select Drink.Id DrinkId, Drink.Navn DrinkNavn, Drink.Tidsstempel DrinkStempel, Vare.Id VareId, Vare.Navn VareNavn, Vare.Pris VarePris, Vare.Tidsstempel VareStempel from (select * from Drink where Navn like @search [SORT] limit @offset, @pageSize) as Drink
                    left join Vare_Drink on Drink.Id = Vare_Drink.DrinkId
                    left join Vare on Vare_Drink.VareId = Vare.Id";

            switch (terms.Sort)
            {
                case SortProducts.AscNavn:
                    sql = sql.Replace("[SORT]", "order by Navn asc");
                    break;
                case SortProducts.DescNavn:
                    sql = sql.Replace("[SORT]", "order by Navn desc");
                    break;
                case SortProducts.AscPris:
                    sql = sql.Replace("[SORT]", "order by Navn asc");
                    break;
                case SortProducts.DescPris:
                    sql = sql.Replace("[SORT]", "order by Navn desc");
                    break;
                case SortProducts.AscTime:
                    sql = sql.Replace("[SORT]", "order by Tidsstempel asc");
                    break;
                case SortProducts.DescTime:
                    sql = sql.Replace("[SORT]", "order by Tidsstempel desc");
                    break;
            }
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@offset", terms.PageSize * terms.Page);
                    cmd.Parameters.AddWithValue("@pageSize", terms.PageSize);
                    cmd.Parameters.AddWithValue("@search", "%" + (terms.SearchText ?? "") + "%");
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            var drinkId = (int)dr["DrinkId"];
                            var drinkNavn = (string)dr["DrinkNavn"];
                            var drinkStempel = (DateTime)dr["DrinkStempel"];
                            var vareId = dr["VareId"] != DBNull.Value ? (int)dr["VareId"] : 0;
                            var vareNavn = dr["VareNavn"] != DBNull.Value ? (string)dr["VareNavn"] : "";
                            var varePris = dr["VarePris"] != DBNull.Value ? (decimal)dr["VarePris"] : 0;
                            var vareStempel = dr["VareStempel"] != DBNull.Value ? (DateTime)dr["VareStempel"] : DateTime.MinValue;


                            var drink = result.FirstOrDefault(a => a.Id == drinkId);

                            if (drink == null)
                            {
                                drink = new Drink
                                {
                                    Id = drinkId,
                                    Navn = drinkNavn,
                                    Tidsstempel = drinkStempel

                                };
                                if (vareId != 0)
                                {
                                    drink.Ingrediense = new List<Vare>
                                    {
                                        new Vare
                                        {
                                            Id = vareId,
                                            Navn = vareNavn,
                                            Pris = varePris,
                                            Tidsstempel = vareStempel
                                        }
                                    };
                                }
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

        public int GetDrinksTotal(string searchText = "")
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select count(*) as Total from Drink where Navn like @search";
                    cmd.Parameters.AddWithValue("@search", $"%{searchText}%");
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
