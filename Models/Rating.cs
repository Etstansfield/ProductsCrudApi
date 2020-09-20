using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCrudApi.Models
{
    public class Rating
    {
        public int id { get; set; }
        public int productId { get; set; }
        public int rating { get; set; }

        internal AppDb Db { get; set; }

        public Rating() { }

        internal Rating(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `ratings` (`productId`, `rating` ) VALUES (@productId, @rating);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            id = (int)cmd.LastInsertedId;
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `ratings` WHERE `Id` = @id";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `ratings` SET `productId` = @productId, `rating` = @rating  WHERE `Id` = @id;";
            BindParams(cmd);
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
                ParameterName = "@productId",
                DbType = DbType.Int32,
                Value = productId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rating",
                DbType = DbType.Int32,
                Value = rating,
            });

        }

    }
}
