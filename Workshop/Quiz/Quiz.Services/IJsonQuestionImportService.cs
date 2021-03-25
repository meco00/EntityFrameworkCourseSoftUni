using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
   public interface IJsonQuestionImportService
    {
        void Import(string fileName, string quizName);
    }
}
