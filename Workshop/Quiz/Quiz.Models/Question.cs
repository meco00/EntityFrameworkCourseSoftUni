
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
  public class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
            UserAnswers = new HashSet<UserAnswer>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int QuizId { get; set; }

        public virtual Quiz Quiz { get; set; }
        
        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
