using System.IO;

namespace SoftUni
{
    using Data;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            using (context)
            {
                var result = RemoveTown(context);

                Console.WriteLine(result);
            }
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var townForDelete = context.Towns
                .FirstOrDefault(t => t.Name == "Seattle");

            var addressesForDelete = context.Addresses
                .Where(a => a.Town.Name == "Seattle");

            var deletedAddressesCount = addressesForDelete.Count();

            context.Employees
                .Where(e => e.Address.Town.Name == "Seattle")
                .ToList()
                .ForEach(a => a.AddressId = null);

            context.Addresses.RemoveRange(addressesForDelete);
            context.Towns.Remove(townForDelete);
            context.SaveChanges();

            return $"{deletedAddressesCount} addresses in Seattle were deleted";
        }
    }
}