using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using INE.K8s.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading;


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

        [HttpGet("info")]
        public ActionResult<Info> info()
        {
            string[] restricted = { "AZURE_CLIENT_ID","AZURE_CLIENT_SECRET","AZURE_TENANT_ID"};
            // TODO: Your code here
            var info = new Info();
            foreach (var hdr in HttpContext.Request.Headers)
            {
                info.headers.Add(hdr.Key, hdr.Value);
            }
            foreach (var route in HttpContext.Request.RouteValues)
            {
                info.routeValues.Add(route.Key, route.Value.ToString());
            }
            var envVars = Environment.GetEnvironmentVariables();
            foreach (var key in envVars.Keys)
            {
                if (!restricted.Contains(key))
                {
                    info.envVariables.Add(key.ToString(), envVars[key].ToString());
                }
            }
            return info;
        }

        [HttpGet("processor/{seconds}")]
        public ActionResult<string> processor(int seconds)
        {
            Program.SetLoadData(true, (double)seconds);
            return $"Set a processor load to run for {seconds} seconds";
        }

       [HttpGet]
        [Route("resultpayload/{size}/{generateErrors}/{delay}")]
        public async Task<ActionResult<string>> ResponsePayload(int size, bool generateErrors, bool delay)
        {
            byte[] output = new byte[size];
            var rand = new Random();
            rand.NextBytes(output);
            await Task.Run(() => { if (delay) Thread.Sleep(rand.Next(3000)); });

            if (generateErrors)
            {
                switch (rand.Next(9))
                {
                    case 1:
                        return new NotFoundResult();
                    case 2:
                        throw new Exception("Sample exception");
                }
            }

            return System.Text.Encoding.Default.GetString(output);
        }



    }
}