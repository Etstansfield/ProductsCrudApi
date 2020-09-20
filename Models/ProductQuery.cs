using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCrudApi.Models
{
    public class ProductQuery
    {
        public readonly AppDb Db;

        public ProductQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Product> FindOneAsync(int id)
        {
            var result = await ReadAllAsync(await FindOneCmd(id).ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

		public async Task<IEnumerable<Product>> FindManyAsync(int start, int amount)
        {
			return await ReadAllAsync(await FindManyCmd(start, amount).ExecuteReaderAsync());

		}

		private async Task<List<Product>> ReadAllAsync(DbDataReader reader)
		{
			var posts = new List<Product>();
			using (reader)
			{
				while (await reader.ReadAsync())
				{
					var post = new Product(Db)
					{
						id = await reader.GetFieldValueAsync<int>(0),
						name = await reader.GetFieldValueAsync<string>(1),
						description = await reader.GetFieldValueAsync<string>(2),
						img = await reader.GetFieldValueAsync<string>(3),
						creator = await reader.GetFieldValueAsync<string>(4),
					};
					posts.Add(post);
				}
			}
			return posts;
		}

		private DbCommand FindOneCmd(int id)
		{
			var cmd = Db.Connection.CreateCommand();
			cmd.CommandText = @"SELECT * FROM `products` WHERE `Id` = @id";
			cmd.Parameters.Add(new MySqlParameter
			{
				ParameterName = "@id",
				DbType = DbType.Int32,
				Value = id,
			});
			return cmd as MySqlCommand;
		}

		private DbCommand FindManyCmd(int start, int amount)
        {
			MySqlCommand cmd = Db.Connection.CreateCommand();
			cmd.CommandText = @"SELECT * FROM `products` LIMIT @amount OFFSET @start";
			cmd.Parameters.Add(new MySqlParameter
			{
				ParameterName = "@amount",
				DbType = DbType.Int32,
				Value = amount
			});
			cmd.Parameters.Add(new MySqlParameter
			{
				ParameterName = "@start",
				DbType = DbType.Int32,
				Value = start
			});

			return cmd;
		}

	}
}
