using Microsoft.Data.SqlClient;
using System;
using System.IO;

namespace ADO.netPRACTICE
{
    class Program
    {
        static void Main(string[] args)
        {
            var practiceWith13ExamFebOnMSSQL = new PracticeExamWithADO("zaza");

            practiceWith13ExamFebOnMSSQL.Run();


            var practiceInjection = new SQLInjection();
            practiceInjection.Run();


         

        }

       
        }
    }

