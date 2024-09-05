using Domain.Entity.Products;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    /// <summary>
    /// Seeding data ban đầu.
    /// </summary>
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure the database is created (or already exists)
            context.Database.EnsureCreated();

            // Check if there are any products already present
            if (!context.Products.Any())
            {
                string[] unit = { "Box", "PCS" };
                
                for (int i = 0; i < 2; i++)
                {
                    context.Units.Add(new Unit()
                    {
                        Id = Guid.NewGuid(),
                        Name = unit[i],
                        CreatedDate = DateTime.Now,
                        CreatedBy = "SeedingData",
                        IsActived = true,
                    });
                }
            }

            // Save the changes to the database
            context.SaveChanges();
        }
    }
}
