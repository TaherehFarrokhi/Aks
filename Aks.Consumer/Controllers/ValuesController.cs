using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aks.ServiceClient;
using Microsoft.AspNetCore.Mvc;

namespace Aks.Consumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IBookClient _bookClient;

        public ValuesController(IBookClient bookClient)
        {
            _bookClient = bookClient;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var books =  await _bookClient.Get();
            return Ok(books.Select(b => b.BookName).ToList());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
