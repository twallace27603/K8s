using System.Collections.Generic;
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace INE.K8s.Models
{
       public class Quiz
    {
        public Quiz()
        {
            QuizID = Guid.NewGuid().ToString();
            Topics = new List<Topic>();
            QuizName = "New Quiz";
        }
                [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public String QuizID { get; set; }
        public string QuizName { get; set; }
        public List<Topic> Topics { get; set; }

    }


    public class Topic
    {
        public Topic()
        {
            TopicID = Guid.NewGuid();
            Questions = new List<Question>();
        }
        public Guid TopicID { get; set; }
        public string TopicName { get; set; }
        public List<Question> Questions { get; set; }
    }

    public class Question
    {
        public Question()
        {
            QuestionID = Guid.NewGuid();
            Answers = new List<Answer>();
        }
        public Guid QuestionID { get; set; }
        public string QuestionText { get; set; }
        public string Image { get; set; }
        public List<Answer> Answers { get; set; }

    }
    public class Answer
    {
        public int AnswerID { get; set; }
        public string AnswerText { get; set; }
        public bool Correct { get; set; }

    }
}