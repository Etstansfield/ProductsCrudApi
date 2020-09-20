using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using System.Data;

namespace ProductsCrudApi.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string img { get; set; }
        public string creator { get; set; }

        internal AppDb Db { get; set; }

        public Product() { }

        internal Product(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `products` (`name`, `description`, `img`, `creator`) VALUES (@name, @description, @img, @creator);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            id = (int)cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `products` SET `name` = @name, `description` = @description, `img` = @img, `creator` = @creator  WHERE `Id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
            // id = (int)cmd.LastInsertedId;
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `products` WHERE `Id` = @id";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@name",
                DbType = DbType.String,
                Value = name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@description",
                DbType = DbType.String,
                Value = description,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@img",
                DbType = DbType.String,
                Value = img,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@creator",
                DbType = DbType.String,
                Value = creator,
            });
        }
    }
}
