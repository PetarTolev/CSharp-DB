namespace VaporStore.DataProcessor
{
    using Data;
    using System.Linq;

    public static class Bonus
	{
		public static string UpdateEmail(VaporStoreDbContext context, string username, string newEmail)
        {
            var user = context.Users
                .FirstOrDefault(u => u.Username == username);

            var emails = context.Users
                .Select(u => u.Email)
                .ToArray();

            if (user == null)
            {
                return $"User {username} not found";
            }

            if (emails.Contains(newEmail))
            {
                return $"Email {newEmail} is already taken";
            }

            user.Email = newEmail;
            return $"Changed {username}'s email successfully";
        }
	}
}
