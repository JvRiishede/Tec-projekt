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
    public class PropertyService : IPropertyService
    {
        private readonly IConnectionInformationService _connectionInformationService;

        public PropertyService(IConnectionInformationService connectionInformationService)
        {
            _connectionInformationService = connectionInformationService;
        }

        public T GetProperty<T>(Properties property, T defaultValue)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select Value from Settings where Property = @property";
                    cmd.Parameters.AddWithValue("@property", property.ToString());
                    var value = cmd.ExecuteScalar();

                    if (defaultValue.GetType().IsEnum)
                    {
                        try
                        {
                            return (T)Enum.Parse(typeof(T), (string)value);
                        }
                        catch (FormatException)
                        {
                            return defaultValue;
                        }
                        catch (InvalidCastException)
                        {
                            return defaultValue;
                        }
                    }

                    try
                    {
                        return (T) Convert.ChangeType(value, typeof(T));
                    }
                    catch (FormatException)
                    {
                        return defaultValue;
                    }
                    catch (InvalidCastException)
                    {
                        return defaultValue;
                    }
                }
            }
        }

        public bool SetProperty<T>(Properties property, T defaultValue)
        {
            using (var con = new MySqlConnection(_connectionInformationService.ConnectionString))
            {
                con.Open();
                int id;
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select Id from Settings where Property = @property";
                    cmd.Parameters.AddWithValue("@property", property.ToString());
                    id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                using (var cmd = con.CreateCommand())
                {
                    if (id > 0)
                    {
                        cmd.CommandText = "update Settings set Value = @value where Id = @id";
                        cmd.Parameters.AddWithValue("@Id", id);
                    }
                    else
                    {
                        cmd.CommandText = "insert into Settings(Property, Value) values(@property, @value)";
                        cmd.Parameters.AddWithValue("@property", property.ToString());
                    }
                    cmd.Parameters.AddWithValue("@value", defaultValue.ToString());
                    return cmd.ExecuteNonQuery() > 0;
                    
                }
            }
        }
    }
}
