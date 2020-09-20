using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsCrudApi.Models;

namespace ProductsCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public AppDb Db { get; }
        public ProductsController(AppDb db)
        {
            Db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetMany([FromBody] GetMany body)
        {
            try
            {
                await Db.Connection.OpenAsync();
                ProductQuery query = new ProductQuery(Db);

                IEnumerable<Product> results = await query.FindManyAsync(body.start, body.amount);

                return new OkObjectResult(results);

            } catch (Exception e)
            {
                Debug.WriteLine(e);
                return new StatusCodeResult(500);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            ProductQuery query = new ProductQuery(Db);

            Product result = await query.FindOneAsync(id);

            if (result is null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result);
        }

        // POST api/products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] Product body)
        {
            await Db.Connection.OpenAsync();
            var query = new ProductQuery(Db);
            var result = await query.FindOneAsync(id);

            if (result is null)
            {
                return new NotFoundResult();
            }
                
            result.description = body.description;
            result.img = body.img;
            result.name = body.name;
            result.creator = body.creator;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            ProductQuery query = new ProductQuery(Db);
            Product result = await query.FindOneAsync(id);

            if (result is null)
            {
                return new NotFoundResult();
            }

            await result.DeleteAsync();
            return new OkResult();
        }
    }
}
