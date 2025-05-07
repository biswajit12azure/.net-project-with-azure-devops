using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WGL.Account.Controllers.BaseController;


namespace WGL.Account.Controllers.CustomerAccount
{
    public class CustomerAccountController : BaseApiController
    {
        // GET: api/<CustomerAccountController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CustomerAccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CustomerAccountController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CustomerAccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomerAccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
