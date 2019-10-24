namespace _06._Remove_Villain
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());

            var connection = new SqlConnection("Server=DESKTOP-IN4GT0T\\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;");


            using (connection)
            {
                connection.Open();

                string villainName = GetVillainName(connection, villainId);

                if (villainName == null)
                {
                    Console.WriteLine("No such villain was found.");
                    return;
                }

                int minionsReleasedNum = DaleteFromMinionsVillains(connection, villainId);

                DeleteFromVillains(connection, villainId, villainName);
                Console.WriteLine($"{minionsReleasedNum} minions were released.");
            }
        }

        private static void DeleteFromVillains(SqlConnection connection, in int villainId, string villainName)
        {
            string query = @"
                    DELETE FROM Villains
                          WHERE Id = @villainId";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);

                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows == 0)
                {
                    throw new InvalidOperationException("Deleting villain failed");
                }

                Console.WriteLine($"{villainName} was deleted.");
            }
        }

        private static int DaleteFromMinionsVillains(SqlConnection connection, in int villainId)
        {
            string query = @"
                    DELETE FROM MinionsVillains 
                          WHERE VillainId = @villainId";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);

                return command.ExecuteNonQuery();
            }
        }

        private static string GetVillainName(SqlConnection connection, in int villainId)
        {
            string query = @"SELECT Name FROM Villains WHERE Id = @villainId";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);

                return (string)command.ExecuteScalar();
            }
        }
    }
}
