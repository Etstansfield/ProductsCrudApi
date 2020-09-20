using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;

namespace ProductsCrudApi.Models
{
    public class RatingQuery
    {
        public readonly AppDb Db;

        public RatingQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<IEnumerable<Rating>> FindManyAsync(int productId)
        {
            return await ReadAllAsync(await FindManyCmd(productId).ExecuteReaderAsync());
        }

        public async Task<Rating> FindOneAsync(int id)
        {
            var result = await ReadAllAsync(await FindOneCmd(id).ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        private async Task<List<Rating>> ReadAllAsync(DbDataReader reader)
        {
            var ratings = new List<Rating>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var rating = new Rating(Db)
                    {
                        id = await reader.GetFieldValueAsync<int>(0),
                        productId = await reader.GetFieldValueAsync<int>(1),
                        rating = await reader.GetFieldValueAsync<int>(2)
                    };
                    ratings.Add(rating);
                }
            }
            return ratings;
        }

        private DbCommand FindManyCmd(int productId)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `ratings` WHERE `productId` = @productId";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@productId",
                DbType = System.Data.DbType.Int32,
                Value = productId
            });

            return cmd;
        }

        private DbCommand FindOneCmd(int id)
        {
            MySqlCommand cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `ratings` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = System.Data.DbType.Int32,
                Value = id
            });

            return cmd;
        }
    }
}
