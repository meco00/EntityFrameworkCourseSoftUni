using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services.Models
{
    public class QuestionInputModel
    {
        public string Question { get; set; }

        public ICollection<AnswerInputModel> Answers { get; set; }

    }
}
