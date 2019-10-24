namespace _08._Increase_Minion_Age
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            int[] inputIds = Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

            SqlConnection connection = new SqlConnection("Server=DESKTOP-IN4GT0T\\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;");
            
            using (connection)
            {
                connection.Open();

                IncrementAgeOfMinions(connection, inputIds);

                PrintMinionsNameAndAge(connection);
            }
        }

        private static void PrintMinionsNameAndAge(SqlConnection connection)
        {
            string query = @"SELECT Name, Age FROM Minions";

            using (var command = new SqlCommand(query, connection))
            {
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string name = (string) reader["Name"];
                    int age = (int) reader["Age"];
                    Console.WriteLine($"{name} {age}");
                }
            }
        }

        private static void IncrementAgeOfMinions(SqlConnection connection, int[] inputIds)
        {
            string query = @" 
                    UPDATE Minions
                       SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                     WHERE Id = @Id";

            using (var command = new SqlCommand(query, connection))
            {
                foreach (var id in inputIds)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
