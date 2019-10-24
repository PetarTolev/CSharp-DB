using System;
using System.Data.SqlClient;

namespace _02._Villain_Names
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var connection =
                new SqlConnection("Server=DESKTOP-IN4GT0T\\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;");

            connection.Open();

            var sqlQuery = @"SELECT v.[Name],
	                         COUNT(mv.MinionId) AS 'MinionsCount'
                             FROM MinionsVillains mv
                             FROM MinionsVillains mv
                             JOIN Villains v
                             ON mv.VillainId = v.Id
                             GROUP BY v.[Name]
                             HAVING COUNT(mv.MinionId) > 3
                             ORDER BY COUNT(mv.MinionId) DESC";

            var command = new SqlCommand(sqlQuery, connection);

            using (command)
            {
                var reader = command.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        var name = (string) reader["Name"];
                        var minionsCount = (int) reader["MinionsCount"];

                        Console.WriteLine($"{name} - {minionsCount}");
                    }
                }
            }
        }
    }
}