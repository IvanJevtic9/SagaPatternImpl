using Saga.Inventory.Entities;

namespace Saga.Inventory.Data
{
    public static class PrepDb
    {
        public static async Task PopulateDatabase(IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetService<UnitOfWork>();

            if((await unitOfWork.Product.GetFirstOrDefaultAsync()) == null)
            {
                await SeedData(unitOfWork);
            }
        }

        private static async Task SeedData(UnitOfWork unitOfWork)
        {
            await unitOfWork.Product.AddRangeAsync(new List<Product>
            {
                new Product
                {
                    Name = "Programiranje 1",
                    Price = 1000,
                    Quantity = 100,
                    Description = "Knjiga - I semestar"
                },
                new Product
                {
                    Name = "Programiranje 2",
                    Price = 1000,
                    Quantity = 100,
                    Description = "Knjiga - II semestar"
                },
                new Product
                {
                    Name = "UOR",
                    Price = 2200,
                    Quantity = 40,
                    Description = "Knjiga - I semestar"
                },
                new Product
                {
                    Name = "UAR",
                    Price = 1500,
                    Quantity = 30,
                    Description = "Knjiga - II semestar"
                },
                new Product
                {
                    Name = "KIIA",
                    Price = 700,
                    Quantity = 20,
                    Description = "Knjiga - II semestar"
                },
                new Product
                {
                    Name = "AISP",
                    Price = 1500,
                    Quantity = 100,
                    Description = "Knjiga - I semestar"
                },
                new Product
                {
                    Name = "UNM",
                    Price = 1000,
                    Quantity = 100,
                    Description = "Knjiga - V semestar"
                },
                new Product
                {
                    Name = "VIS",
                    Price = 600,
                    Quantity = 100,
                    Description = "Knjiga - VI semestar"
                },
                new Product
                {
                    Name = "A1",
                    Price = 900,
                    Quantity = 120,
                    Description = "Knjiga - II semestar"
                },
                new Product
                {
                    Name = "A2",
                    Price = 990,
                    Quantity = 150,
                    Description = "Knjiga - III semestar"
                },
                new Product
                {
                    Name = "DS1",
                    Price = 500,
                    Quantity = 80,
                    Description = "Knjiga - I semestar"
                },
                new Product
                {
                    Name = "LA",
                    Price = 500,
                    Quantity = 100,
                    Description = "Knjiga - I semestar "
                },
                new Product
                {
                    Name = "DS2",
                    Price = 700,
                    Quantity = 100,
                    Description = "Knjiga - II semestar"
                },
                new Product
                {
                    Name = "RG",
                    Price = 1300,
                    Quantity = 100,
                    Description = "Knjiga - V semestar"
                },
                new Product
                {
                    Name = "OOP",
                    Price = 700,
                    Quantity = 200,
                    Description = "Knjiga - IV semestar"
                }
            });
            await unitOfWork.SaveChangesAsync();
        }
    }
}
