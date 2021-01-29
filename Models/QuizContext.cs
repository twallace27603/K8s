using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace INE.K8s.Models
{
    public class QuizContext
    {
        private string _connect;
        public QuizContext(string connect)
        {
            _connect = connect;
        }
        public async Task<List<Quiz>> GetQuizzes()
        {
            var connection = new SqlConnection(_connect);
            var cmd = new SqlCommand("uspGetQuizzes", connection);
            var result = new List<Quiz>();
            await connection.OpenAsync();
            try
            {
                var rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    result.Add(new Quiz { QuizID = rdr["QuizID"].ToString(), QuizName = rdr["Name"].ToString() });
                }

            }
            finally
            {
                await connection.CloseAsync();
            }


            return result;

        }

    }
}