using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsCrudApi.Models;

namespace ProductsCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {

        public AppDb Db;

        public RatingsController(AppDb db)
        {
            Db = db;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(int productId)
        {
            // find all ratings by their productId
            await Db.Connection.OpenAsync();
            RatingQuery query = new RatingQuery(Db);

            IEnumerable<Rating> results = await query.FindManyAsync(productId);

            if (!results.Any())
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(results);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Rating rating)
        {
            // then check that the product exists
            await Db.Connection.OpenAsync();
            ProductQuery query = new ProductQuery(Db);

            Product result = await query.FindOneAsync(rating.productId);

            if (result is null)
            {
                return new NotFoundResult();
            }

            // assuming those pass, insert the rating
            
            rating.Db = Db;
            await rating.InsertAsync();

            return new OkObjectResult(rating);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // then check that the product exists
            await Db.Connection.OpenAsync();
            RatingQuery query = new RatingQuery(Db);

            Rating rating = await query.FindOneAsync(id);

            if (rating is null)
            {
                return new NotFoundResult();
            }

            // assuming those pass, insert the rating

            rating.Db = Db;
            await rating.DeleteAsync();

            return new OkObjectResult(rating);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Rating rating)
        {
            // check that the rating exists
            await Db.Connection.OpenAsync();
            RatingQuery query = new RatingQuery(Db);

            Rating ogRating = await query.FindOneAsync(id);

            if (ogRating is null)
            {
                return new NotFoundResult();
            }

            // now update the ogRating

            ogRating.rating = rating.rating;

            await ogRating.UpdateAsync();

            return new OkObjectResult(ogRating);
        }
    }
}
