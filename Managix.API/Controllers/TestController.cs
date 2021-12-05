using Managix.Redis.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Managix.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TestController : ControllerBase
    {
        // GET: api/<TestController>
        [HttpGet]
        public async Task<string> Get([FromServices] IRedisDatabase _redis)
        {
            try
            {
                string appInfoDtos = await _redis.GetOrCreateAsync<string>($"GetAppList:",
                   async () => await Task.FromResult(Guid.NewGuid().ToString()));


                //var ss = await _redis.AddAsync("test1", Guid.NewGuid().ToString());
                var str = await _redis.GetAsync<string>("test1");
                var str2 = await _redis.SearchKeysAsync("*");
                return DateTime.Now.ToString()+":"+ str;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TestController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
