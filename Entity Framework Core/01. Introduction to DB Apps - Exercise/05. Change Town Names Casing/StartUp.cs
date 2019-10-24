namespace _05._Change_Town_Names_Casing
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            string countryName = Console.ReadLine();

            var connection = new SqlConnection("Server=DESKTOP-IN4GT0T\\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;");

            using (connection)
            {
                connection.Open();

                List<string> towns = GetTowns(connection, countryName);

                if (towns.Count > 0)
                {
                    SetTownsUpperCase(connection, countryName);

                    towns.Select(t => t.ToUpper());

                    Console.WriteLine($"{towns.Count} town names were affected.");
                    Console.WriteLine($"[{string.Join(", ", towns)}]");
                }
                else
                {
                    Console.WriteLine("No town names were affected.");
                }
            }
        }

        private static void SetTownsUpperCase(SqlConnection connection, string countryName)
        {
            var setTownUpperCaseQuery = @"
                            UPDATE Towns
                               SET Name = UPPER(Name)
                             WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

            using (var command = new SqlCommand(setTownUpperCaseQuery, connection))
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@countryName", countryName);

                command.ExecuteNonQuery();
            }
        }

        private static List<string> GetTowns(SqlConnection connection, string countryName)
        {
            var selectTownsQuery = @"
                     SELECT t.Name 
                       FROM Towns as t
                       JOIN Countries AS c ON c.Id = t.CountryCode
                      WHERE c.Name = @countryName";

            using (var command = new SqlCommand(selectTownsQuery, connection))
            {
                command.Parameters.AddWithValue("@countryName", countryName);
                var reader = command.ExecuteReader();
                
                List<string> towns = new List<string>();

                while (reader.Read())
                {
                    towns.Add((string)reader["Name"]);
                }

                reader.Close();

                return towns;
            }
        }
    }
}
