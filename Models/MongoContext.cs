using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Net.Http;
using Newtonsoft.Json;
namespace INE.K8s.Models
{
    public class MongoContext
    {
        private string _server;
        public MongoContext(string server)
        {
            _server = server;
        }
        public async Task<Quiz> GetQuiz(string id)
        {
            var client = new MongoClient($"mongodb://{_server}:27017");
            var database = client.GetDatabase("Trivia");
            var collection = database.GetCollection<Quiz>("Quizzes");
            var quiz = (await collection.FindAsync<Quiz>(q => q.QuizID == id)).FirstOrDefault();
            return quiz;
        }

        public async Task<int> LoadMongo()
        {
            string[] urls = { "https://inedemoassets.blob.core.windows.net/mongo/QuizAzureConcepts.json", "https://inedemoassets.blob.core.windows.net/mongo/QuizAzureServices.json" };
            int count = 0;


            HttpClient httpClient = new HttpClient();

            foreach (string url in urls)
            {
                string result = await httpClient.GetStringAsync(url);
                Quiz quiz = JsonConvert.DeserializeObject<Quiz>(result);
                var client = new MongoClient($"mongodb://{_server}:27017");
                var database = client.GetDatabase("Trivia");
                var collection = database.GetCollection<Quiz>("Quizzes");
                var q = (await collection.FindAsync<Quiz>(q => q.QuizID == quiz.QuizID)).FirstOrDefault();
                if (q == null)
                {
                    await collection.InsertOneAsync(quiz);
                    count++;
                }                
            }
            return count;


        }
    }
}