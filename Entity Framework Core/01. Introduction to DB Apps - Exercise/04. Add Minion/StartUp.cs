namespace _04._Add_Minion
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            string[] minionInformation = Console.ReadLine().Split(' ').ToArray();
            string minionName = minionInformation[1];
            int minionAge = int.Parse(minionInformation[2]);
            string minionTownName = minionInformation[3];

            string[] villainInformation = Console.ReadLine().Split(' ').ToArray();
            string villainName = villainInformation[1];

            using (var connection = new SqlConnection("Server=DESKTOP-IN4GT0T\\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;"))
            {
                connection.Open();

                int? townId = GetTownId(connection, minionTownName);

                if (townId == null)
                {
                    AddTown(connection, minionTownName);
                    townId = GetTownId(connection, minionTownName);
                }

                int? minionId = GetMinionId(connection, minionName);

                if (minionId == null)
                {
                    AddMinion(connection, minionName, minionAge, townId);
                    minionId = GetMinionId(connection, minionName);
                }

                int? villainId = GetVillainId(connection, villainName);

                if (villainId == null)
                {
                    AddVillain(connection, villainName);
                    villainId = GetVillainId(connection, villainName);
                }

                var insertMinionsVillainsQuery = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";

                using (var insertMinionsVillainsCommand = new SqlCommand(insertMinionsVillainsQuery, connection))
                {
                    insertMinionsVillainsCommand.Parameters.AddWithValue("@villainId", villainId);
                    insertMinionsVillainsCommand.Parameters.AddWithValue("@minionId", minionId);

                    int affectedRows = 0;

                    try
                    {
                        affectedRows = insertMinionsVillainsCommand.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"{minionName} is already a slave of {villainName}");
                    }


                    if (affectedRows > 0)
                    {
                        Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                    }
                }
            }
        }

        private static void AddVillain(SqlConnection connection, string villainName)
        {
            var insertVillainQuery =
                @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

            using (var insertVillainCommand = new SqlCommand(insertVillainQuery, connection))
            {
                insertVillainCommand.Parameters.AddWithValue("@villainName", villainName);
                insertVillainCommand.ExecuteNonQuery();
                Console.WriteLine($"Villain {villainName} was added to the database.");
            }
        }

        private static int? GetVillainId(SqlConnection connection, string villainName)
        {
            var getVillainIdQuery = @"SELECT Id FROM Villains WHERE Name = @Name";

            using (var getVillainIdCommand = new SqlCommand(getVillainIdQuery, connection))
            {
                getVillainIdCommand.Parameters.AddWithValue("@Name", villainName);
                int? villainId = (int?) getVillainIdCommand.ExecuteScalar();

                return villainId;
            }
        }

        private static void AddMinion(SqlConnection connection, string minionName, in int minionAge, int? townId)
        {
            var insertMinionQuery = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)";

            using (var insertMinionCommand = new SqlCommand(insertMinionQuery, connection))
            {
                insertMinionCommand.Parameters.AddWithValue("@nam", minionName);
                insertMinionCommand.Parameters.AddWithValue("@age", minionAge);
                insertMinionCommand.Parameters.AddWithValue("@townId", townId);

                insertMinionCommand.ExecuteNonQuery();
            }
        }

        private static int? GetMinionId(SqlConnection connection, string minionName)
        {
            var getMinionIdQuery = @"SELECT Id FROM Minions WHERE Name = @Name";

            using (var getMinionIdCommand = new SqlCommand(getMinionIdQuery, connection))
            {
                getMinionIdCommand.Parameters.AddWithValue("@Name", minionName);

                int? minionId = (int?) getMinionIdCommand.ExecuteScalar();

                return minionId;
            }
        }

        public static void AddTown(SqlConnection connection, string minionTownName)
        {
            var insertTownQuery = @"INSERT INTO Towns (Name) VALUES (@townName)";

            using (var insertVillainsCommand = new SqlCommand(insertTownQuery, connection))
            {
                insertVillainsCommand.Parameters.AddWithValue("@townName", minionTownName);
                insertVillainsCommand.ExecuteNonQuery();
                Console.WriteLine($"Town {minionTownName} was added to the database.");
            }
        }

        public static int? GetTownId(SqlConnection connection, string minionTownName)
        {
            var getTownIdQuery = @"SELECT Id FROM Towns WHERE Name = @townName";

            using (var getTownIdCommand = new SqlCommand(getTownIdQuery, connection))
            {
                getTownIdCommand.Parameters.AddWithValue("@townName", minionTownName);

                int? minionTownId = (int?) getTownIdCommand.ExecuteScalar();

                return minionTownId;
            }
        }
    }
}