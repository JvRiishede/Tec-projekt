﻿using System;
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
    public class VareService : IVareService
    {
        private readonly IConnectionInformationService _connectionInformationService;

        public VareService(IConnectionInformationService connectionInformationService)
        {
            _connectionInformationService = connectionInformationService;
        }

        public List<Vare> GetAllVare()
        {
            var result = new List<Vare>();
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select Id, Navn, Pris, Tidsstempel from Vare";
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            result.Add(new Vare
                            {
                                Id = (int)dr["Id"],
                                Navn = (string)dr["Navn"],
                                Pris = (decimal)dr["Pris"],
                                Tidsstempel = (DateTime)dr["Tidsstempel"]
                            });
                        }
                    }
                }
            }
            return result;
        }

        public Vare GetVare(int id)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select Id, Navn, Pris, Tidsstempel from Vare where Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            return new Vare
                            {
                                Id = (int)dr["Id"],
                                Navn = (string)dr["Navn"],
                                Pris = (decimal)dr["Pris"],
                                Tidsstempel = (DateTime)dr["Tidsstempel"]
                            };
                        }
                    }
                }
            }
            return null;
        }

        public int CreateVare(string navn, decimal pris)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "insert into Vare(Navn, Pris) values(@Navn, @Pris); select LAST_INSERT_ID()";
                    cmd.Parameters.AddWithValue("@Navn", navn);
                    cmd.Parameters.AddWithValue("@Pris", pris);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public bool UpdateVare(Vare vare)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "update Vare set Navn = @Navn, Pris = @Pris where Id = @Id";
                    cmd.Parameters.AddWithValue("@Navn", vare.Navn);
                    cmd.Parameters.AddWithValue("@Pris", vare.Pris);
                    cmd.Parameters.AddWithValue("@Id", vare.Id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteVare(int id)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "delete from Vare where Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Vare> GetPagedVare(ProductSearchTerms terms)
        {
            var result = new List<Vare>();
            var sql = @"select Id, Navn, Tidsstempel, Pris from Vare where Navn like @search [SORT] limit @offset, @pageSize";

            switch (terms.Sort)
            {
                case SortProducts.AscNavn:
                    sql = sql.Replace("[SORT]", "order by Navn asc");
                    break;
                case SortProducts.DescNavn:
                    sql = sql.Replace("[SORT]", "order by Navn desc");
                    break;
                case SortProducts.AscPris:
                    sql = sql.Replace("[SORT]", "order by Pris asc");
                    break;
                case SortProducts.DescPris:
                    sql = sql.Replace("[SORT]", "order by Pris desc");
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
                            result.Add(new Vare()
                            {
                                Id = (int)dr["Id"],
                                Navn = (string)dr["Navn"],
                                Pris = (decimal)dr["Pris"],
                                Tidsstempel = (DateTime)dr["Tidsstempel"]
                            });
                        }
                    }
                }
            }
            return result;
        }
        public int GetVareTotal()
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select count(*) as Total from Vare";
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
