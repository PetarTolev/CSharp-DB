namespace _03._Minion_Names
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var villainId = int.Parse(Console.ReadLine());

            using (var connection = new SqlConnection("Server=DESKTOP-IN4GT0T\\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;"))
            {
                connection.Open();

                var villainNameQuery = @"SELECT Name FROM Villains WHERE Id = @Id";

                using (var command = new SqlCommand(villainNameQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", villainId); 

                    string villainName = (string) command.ExecuteScalar();

                    if (villainName == null)
                    {
                        Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                        return;
                    }

                    Console.WriteLine($"Villain: {villainName}");
                }

                var minionsQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

                using (var command = new SqlCommand(minionsQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", villainId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("no minions");
                            return;
                        }

                        while (reader.Read())
                        {
                            long row = (long) reader["RowNum"];
                            string name = (string) reader["Name"];
                            int age = (int) reader["Age"];

                            Console.WriteLine($"{row}. {name} {age}");
                        }
                    }
                }
            }
        }
    }
}
