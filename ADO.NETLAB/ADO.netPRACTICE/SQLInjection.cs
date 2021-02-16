using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADO.netPRACTICE
{
    public class SQLInjection
    {
     

        public void Run()
        {
            SqlInjectionPrevent();

        }

        private static void SqlInjectionPrevent()
        {
            var dbconnection = new SqlConnection("Server=.;Database=Demo;Integrated Security=true");
            using (dbconnection)
            {
                dbconnection.Open();

                Console.WriteLine("Please Enter Username");
                var inputUsername = Console.ReadLine();

                Console.WriteLine("Please Enter Password");
                var inputPassword = Console.ReadLine();



                var command = new SqlCommand(
                   @$" SELECT COUNT(*) FROM
                      Customers
                      WHERE FirstName=@FirstName AND PaymentNumber=@PaymentNumber; ", dbconnection);


                //With this method Sql Injection Is  prevented 

                command.Parameters.AddWithValue("@FirstName", inputUsername);
                command.Parameters.AddWithValue("@PaymentNumber", inputPassword);


                int IsValid = (int)command.ExecuteScalar();

                if (IsValid > 0)
                {
                    Console.WriteLine("Access granted");

                }
                else
                {
                    Console.WriteLine("Access denied");
                }









            }
        }
    }
}

