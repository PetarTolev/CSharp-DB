namespace _07._Print_All_Minion_Names
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class StartUp
    {

        public static SqlConnection connection = new SqlConnection("Server=DESKTOP-IN4GT0T\\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;");

        public static void Main(string[] args)
        {
            List<string> names = GetNames();

            for (int i = 0; i < names.Count / 2; i++)
            {
                Console.WriteLine(names[0 + i]);
                Console.WriteLine(names[names.Count - 1 - i]);
            }

            if (names.Count % 2 != 0)
            {
                Console.WriteLine(names[names.Count / 2]);
            }
        }

        private static List<string> GetNames()
        {
            using (connection)
            {
                connection.Open();

                var query = @"SELECT Name FROM Minions";

                using (var command = new SqlCommand(query, connection))
                {
                    var reader = command.ExecuteReader();

                    List<string> names = new List<string>();

                    while (reader.Read())
                    {
                        names.Add((string)reader["Name"]);
                    }

                    return names;
                }
            }
        }
    }
}
