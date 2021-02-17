using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ADO.NETExercise
{
    class Program
    {
        private static SqlConnection connection;
        private static SqlCommand cmd;

        static void Main(string[] args)
        {
            CreateMinionsDB();

            connection.Open();

            using (connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated security=true"))
            {

                ExecuteNonQuery(InfoForCreatingTables());

                ExecuteNonQuery(InfoForInsertingRecordsIntoTable());


                Problem02();

                Problem03();


                Problem04();

                Problem05();

                Problem06();


                Problem07();

                Problem08();

                Problem09();



            }


        }

        private static void Problem09()
        {
            cmd = new SqlCommand(@"CREATE PROC usp_GetOlder @id INT
                                        AS
                                        UPDATE Minions
                                           SET Age += 1
                                         WHERE Id = @id
                                         SELECT Name, Age FROM Minions WHERE Id = @Id", connection);

            cmd.ExecuteNonQuery();


            cmd = new SqlCommand("EXEC dbo.usp_GetOlder @Id", connection);

            var inputUserId = Console.ReadLine();

            cmd.Parameters.AddWithValue("@Id", inputUserId);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"{reader[0]} – {reader[1]} years old");
            }
        }

        private static void Problem08()
        {
            var input = Console.ReadLine().Split().ToArray();

            for (int i = 0; i < input.Length; i++)
            {
                var currentId = input[i];

                cmd = new SqlCommand(@"UPDATE Minions
                                    SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                    WHERE Id = @Id", connection);

                cmd.Parameters.AddWithValue("@Id", currentId);
                cmd.ExecuteNonQuery();

            }

            cmd = new SqlCommand("SELECT Name, Age FROM Minions", connection);

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader[0]}  {reader[1]}");

            }
            reader.Close();
        }

        private static void Problem07()
        {
            cmd = new SqlCommand("SELECT Name FROM Minions", connection);

            var reader = cmd.ExecuteReader();

            var list = new List<string>();

            while (reader.Read())
            {
                list.Add(reader[0].ToString());

            }

            reader.Close();

            var list2 = ConstructValidSequence(list.ToArray());

            Console.WriteLine(String.Join(Environment.NewLine, list2));
        }

        private static List<string> ConstructValidSequence(string[] list)
        {
            var list2 = new string[list.Length];

            int m = 0;
            for (int i = 0; i < list.Length - 1; i++)
            {
                list2[i] = list[m++];
                list2[++i] = list[list.Length - m];
            }

            if (list2.Length % 2 != 0)
            {
                list2[list2.Length - 1] = list[list.Length / 2];

            }

            return list2.ToList();
        }

        private static void Problem06()
        {
            var villainId = Console.ReadLine();


            cmd = new SqlCommand("SELECT Name FROM Villains WHERE Id = @villainId", connection);
            cmd.Parameters.AddWithValue("@villainId", villainId);

            var villainName = (string)cmd.ExecuteScalar();

            if (villainName is null)
            {
                Console.WriteLine("No such villain was found.");
                return;

            }

            cmd = new SqlCommand(@"DELETE FROM MinionsVillains 
                                         WHERE VillainId = @villainId", connection);

            cmd.Parameters.AddWithValue("@villainId", villainId);

            var affectedRows = cmd.ExecuteNonQuery();

            cmd = new SqlCommand(@"DELETE FROM Villains
                                        WHERE Id = @villainId", connection);

            cmd.Parameters.AddWithValue("@villainId", villainId);

            cmd.ExecuteNonQuery();

            Console.WriteLine($"{villainName} was deleted.");
            Console.WriteLine($"{affectedRows} minions were released");
        }

        private static void Problem05()
        {
           

            var countryName = Console.ReadLine();


            cmd = new SqlCommand(@"UPDATE Towns
                                    SET Name = UPPER(Name)
                                    WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)", connection);
            cmd.Parameters.AddWithValue("@countryName", countryName);

            int affectedRows = (int)cmd.ExecuteNonQuery();

            if (affectedRows == 0)
            {
                Console.WriteLine("No town names were affected.");

            }
            else
            {
                Console.WriteLine($"{affectedRows} town names were affected.");

                cmd = new SqlCommand(@"SELECT t.Name 
                                            FROM Towns as t
                                            JOIN Countries AS c ON c.Id = t.CountryCode
                                             WHERE c.Name = @countryName", connection);

                cmd.Parameters.AddWithValue("@countryName", countryName);

                List<string> List = new List<string>();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    List.Add(reader[0].ToString());

                }
                reader.Close();

                Console.WriteLine($"[{String.Join(", ", List)}]");

            }
        }

        private static void Problem04()
        {
            var transaction = connection.BeginTransaction();
            try
            {
                string[] minionInfo = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
                string[] villainInfo = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();

                cmd.Transaction = transaction;
                int townId = ValidateTown(minionInfo[2]);

                int minionId = ValidateMinion(minionInfo[0], minionInfo[1], townId);

                int villainId = ValidateVillain(villainInfo[0]);

                cmd = new SqlCommand("INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)", connection);
                cmd.Parameters.AddWithValue("@villainId", minionId);
                cmd.Parameters.AddWithValue("@minionId", villainId);

                try
                {
                    cmd.ExecuteNonQuery();

                    Console.WriteLine($"Successfully added {minionInfo[0]} to be minion of {villainInfo[0]}.");
                }
                catch (SqlException)
                {

                    Console.WriteLine($"{minionInfo[0]} is already minion of {villainInfo[0]}");
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                try
                {

                }
                catch (Exception)
                {

                    
                }
               
            }

         
        }

        private static int ValidateVillain(string villainName)
        {


         

            cmd = new SqlCommand("SELECT Id FROM Villains WHERE Name = @Name", connection);
            cmd.Parameters.AddWithValue("@Name", villainName);

            int? villainId = (int?)cmd.ExecuteScalar();

            if (villainId is null)
            {
                cmd = new SqlCommand("INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)", connection);
                cmd.Parameters.AddWithValue("@villainName", villainName);

                cmd.ExecuteNonQuery();
                Console.WriteLine($"Villain {villainName} was added to the database.");

                cmd = new SqlCommand("SELECT Id FROM Villains WHERE Name = @Name", connection);
                cmd.Parameters.AddWithValue("@Name", villainName);

                villainId = (int)cmd.ExecuteScalar();

            }

            return (int)villainId;
        }

        private static int ValidateMinion(string minionName, string minionAge, int townId)
        {
            

            cmd = new SqlCommand("SELECT Id FROM Minions WHERE Name = @Name", connection);
            cmd.Parameters.AddWithValue("@Name", minionName);

            int? minionId = (int?)cmd.ExecuteScalar();

            if (minionId is null)
            {
                cmd = new SqlCommand("INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)", connection);
                cmd.Parameters.AddWithValue("@nam", minionName);
                cmd.Parameters.AddWithValue("@age", minionAge);
                cmd.Parameters.AddWithValue("@townId", townId);

                cmd.ExecuteNonQuery();


                cmd = new SqlCommand("SELECT Id FROM Minions WHERE Name = @Name", connection);
                cmd.Parameters.AddWithValue("@Name", minionName);

                minionId = (int)cmd.ExecuteScalar();

            }

            return (int)minionId;
        }

        private static int ValidateTown(string minionInfo)
        {
            var townName = minionInfo;

            cmd = new SqlCommand("SELECT Id FROM Towns WHERE Name = @townName", connection);
            cmd.Parameters.AddWithValue("@townName", townName);

            int? townId = (int?)cmd.ExecuteScalar();

            if (townId is null)
            {
                cmd = new SqlCommand("INSERT INTO Towns (Name) VALUES (@townName)", connection);
                cmd.Parameters.AddWithValue("@townName", townName);

                cmd.ExecuteNonQuery();

                Console.WriteLine($"Town {townName} was added to the database.");


                cmd = new SqlCommand("SELECT Id FROM Towns WHERE Name = @townName", connection);
                cmd.Parameters.AddWithValue("@townName", townName);

                townId = (int)cmd.ExecuteScalar();



            }

            return (int)townId;
        }

        private static void Problem03()
        {
            cmd = new SqlCommand(@"SELECT Name FROM Villains WHERE Id = @Id", connection);

            int inputId = int.Parse(Console.ReadLine());

            cmd.Parameters.AddWithValue("@Id", inputId);

            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine($"No villain with ID {inputId} exists in the database.");
            }
            else
            {
                reader.Read();

                Console.WriteLine($"Villain : {reader[0]}");
                reader.Close();


                cmd = new SqlCommand(@"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name,
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name", connection);

                cmd.Parameters.AddWithValue("@Id", inputId);

                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    Console.WriteLine("(no minions)");

                }
                else
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader[0]}. {reader[1]} {reader[2]}");

                    }
                }





            }

            reader.Close();
        }

        private static void Problem02()
        {
            var transaction = connection.BeginTransaction();
            try
            {
                cmd = new SqlCommand(@"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                     FROM Villains AS v
                                     JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                                      GROUP BY v.Id, v.Name
                                        HAVING COUNT(mv.VillainId) > 3
                                      ORDER BY COUNT(mv.VillainId)", connection);

                cmd.Transaction = transaction;

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader[0]} - {reader[1]}");

                }
                reader.Close();

                transaction.Commit();

            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);

                try
                {
                    transaction.Rollback();
                }
                catch (Exception e2)
                {

                    Console.WriteLine(e2.Message);
                }

            }
          
        }

        private static void ExecuteNonQuery(string[] info)
        {
           


            foreach (var item in info)
            {
                cmd = new SqlCommand(item, connection);
                cmd.ExecuteNonQuery();

            }
        }


        private static string[] InfoForInsertingRecordsIntoTable()
        {
            return new string[]
            {
                    "INSERT INTO Countries ([Name]) VALUES('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')",

                    "INSERT INTO Towns([Name], CountryCode) VALUES('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)",

                    "INSERT INTO Minions(Name, Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)",

                    "INSERT INTO EvilnessFactors(Name) VALUES('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')",

                    "INSERT INTO Villains(Name, EvilnessFactorId) VALUES('Gru', 2),('Victor', 1),('Jilly', 3),('Miro', 4),('Rosen', 5),('Dimityr', 1),('Dobromir', 2)",

                    "INSERT INTO MinionsVillains(MinionId, VillainId) VALUES(4, 2),(1, 1),(5, 7),(3, 5),(2, 6),(11, 5),(8, 4),(9, 7),(7, 1),(1, 3),(7, 3),(5, 3),(4, 3),(1, 2),(2, 1),(2, 7)",

            };
        }


        private static void CreateMinionsDB()
        {
            using (connection = new SqlConnection("Server=.;Database=master;Integrated security=true"))
            {
                connection.Open();
                cmd = new SqlCommand("CREATE DATABASE MinionsDB", connection);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Created MinionsDB");



            }
        }

        private static string[] InfoForCreatingTables()
        {
          return new string[]
            {
                    "CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))",

                    "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))",

                    "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(30), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))",

                    "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))",

                    "CREATE TABLE Villains (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))",

                    "CREATE TABLE MinionsVillains (MinionId INT FOREIGN KEY REFERENCES Minions(Id),VillainId INT FOREIGN KEY REFERENCES Villains(Id),CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))"

            };
        }
    }
}
