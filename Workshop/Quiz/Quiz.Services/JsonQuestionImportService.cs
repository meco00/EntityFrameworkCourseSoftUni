using Newtonsoft.Json;
using Quiz.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public class JsonQuestionImportService : IJsonQuestionImportService
    {
        private readonly IQuizService quizService;
        private readonly IQuestionService questionService;
        private readonly IAnswerService answerService;

        public JsonQuestionImportService(IQuizService quizService,
            IQuestionService questionService,
            IAnswerService answerService)
        {
            this.quizService = quizService;
            this.questionService = questionService;
            this.answerService = answerService;

        }
        public void Import(string fileName, string quizName)
        {
            var json = File.ReadAllText(fileName);
            var questions = JsonConvert.DeserializeObject<IEnumerable<QuestionInputModel>>(json);

            var quizId = quizService.Add(quizName);
            foreach (var question in questions)
            {
                var questionId = questionService.Add(question.Question, quizId);
                foreach (var answer in question.Answers)
                {
                    answerService.Add(answer.Answer, answer.Correct ? 1 : 0, answer.Correct, questionId);
                }
            }

        }
    }
}
