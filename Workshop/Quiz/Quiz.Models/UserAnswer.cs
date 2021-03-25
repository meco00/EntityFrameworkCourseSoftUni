﻿
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
   public class UserAnswer
    {
        public int Id { get; set; }

        public string IdentityUserId { get; set; }

        public virtual IdentityUser IdentityUser { get; set; }

        public int? AnswerId { get; set; }

        public virtual Answer Answer { get; set; }

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }

    }
}