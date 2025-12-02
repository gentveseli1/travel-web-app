using TravelWebApp.Data;
using TravelWebApp.Models;

namespace TravelWebApp.Data
{
    public static class DataSeeder
    {
        public static void SeedDatabase(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var user = new User
                {
                    Email = "admin@test.com",
                    Password = "123456"
                };

                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}
