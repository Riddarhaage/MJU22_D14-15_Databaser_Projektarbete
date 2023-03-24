using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using static Gym_Booking_Manager.LocalStorage;

namespace Gym_Booking_Manager
{
    public class DBStorage : IDatabase
    {
        //<----- Hidden connection string
        private string connectionString = "Server=localhost;Port=5432;Database=GymBookingManager;User Id=postgres;Password=Riddarhaage1;";

        public bool Create<T>(T entity)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var type = entity.GetType();
                var properties = type.GetProperties();
                var tableName = type.Name;
                var columns = new List<string>();
                var values = new List<string>();
                foreach (var property in properties)
                {
                    var value = property.GetValue(entity);
                    if (value != null)
                    {
                        columns.Add(property.Name);
                        values.Add(value.ToString());
                    }
                }
                var sql = $"INSERT INTO {tableName} ({string.Join(", ", columns.Select(c => $"\"{c}\""))}) VALUES ({string.Join(", ", values.Select(v => $"'{v}'"))})";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        public bool Delete<T>(T entity)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var type = entity.GetType();
                var properties = type.GetProperties();
                var tableName = type.Name;
                var columns = new List<string>();
                var values = new List<string>();
                foreach (var property in properties)
                {
                    var value = property.GetValue(entity);
                    if (value != null)
                    {
                        columns.Add(property.Name);
                        values.Add(value.ToString());
                    }
                }
                var sql = $"DELETE FROM {tableName} WHERE {columns[0]} = '{values[0]}'";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        public List<T> Read<T>(string? field, string? value)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var type = typeof(T);
                var tableName = type.Name;
                var sql = $"SELECT * FROM {tableName}";
                if (field != null && value != null)
                {
                    sql += $" WHERE \"{field}\" = '{value}'";
                }
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var entities = new List<T>();
                        while (reader.Read())
                        {
                            var entity = Activator.CreateInstance<T>();
                            var properties = type.GetProperties();
                            foreach (var property in properties)
                            {
                               var value2 = reader[property.Name];
                                if (value2 != DBNull.Value)
                                {
                                    property.SetValue(entity, value2);
                                }
                            }
                            entities.Add(entity);
                        }
                        return entities;
                    }
                }
            }
        }
        public bool Update<T>(T newEntity, T oldEntity)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var type = newEntity.GetType();
                var properties = type.GetProperties();
                var tableName = type.Name;
                var columns = new List<string>();
                var values = new List<string>();
                var oldValues = new List<string>();
                foreach (var property in properties)
                {
                    var value = property.GetValue(newEntity);
                    var oldValue = property.GetValue(oldEntity);
                    if (value != null)
                    {
                        columns.Add(property.Name);
                        values.Add(value.ToString());
                        oldValues.Add(oldValue.ToString());
                    }
                }
                for (int i = 0; i < columns.Count; i++)
                {
                    if (values[i] != oldValues[i])
                    {
                        var sql = $"UPDATE {tableName} SET {columns[i]} = '{values[i]}' WHERE {columns[i]} = '{oldValues[i]}'";
                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

            }
            return true;
        }

    }
}
