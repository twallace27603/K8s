using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using INE.K8s.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace K8s.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkController : ControllerBase
    {
        IConfiguration _config;
        public WorkController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("quizNames")]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizNames()
        {
            // TODO: Your code here
            var context = new QuizContext(_config.GetValue<string>("SQLConnection"));
            return await context.GetQuizzes();
        }

        [HttpGet("quiz/{id}")]
        public async Task<ActionResult<Quiz>> GetQuizById(string id)
        {
            // TODO: Your code here
            var context = new MongoContext(_config.GetValue<string>("MongoServer"));
            return await context.GetQuiz(id);
        }

        [HttpGet("load")]
       public async Task<ActionResult<int>> load(string id)
        {
            // TODO: Your code here
            var context = new MongoContext(_config.GetValue<string>("MongoServer"));
            return await context.LoadMongo();
        }

        [HttpPost("")]
        public async Task<ActionResult<string>> Poststring(string model)
        {
            // TODO: Your code here
            await Task.Yield();

            return null;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Putstring(int id, string model)
        {
            // TODO: Your code here
            await Task.Yield();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeletestringById(int id)
        {
            // TODO: Your code here
            await Task.Yield();

            return null;
        }
    }
}