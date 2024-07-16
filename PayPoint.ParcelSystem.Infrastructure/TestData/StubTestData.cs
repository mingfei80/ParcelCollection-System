using PayPoint.ParcelSystem.Domain.Models;

namespace PayPoint.ParcelSystem.Infrastructure.TestData;

public static class StubTestData
{
    public static List<Product> Products()
    {
        return new List<Product>
            {
                new Product { ProductId = 1, Name = "XBox One", SupplierId = 1 },
                new Product { ProductId = 2, Name = "T-shirt", SupplierId = 2 },
                new Product { ProductId = 3, Name = "Single Bed Sheet", SupplierId = 4 },
                new Product { ProductId = 4, Name = "Pixel 8", SupplierId = 1 },
                new Product { ProductId = 5, Name = "Kindle", SupplierId = 1 },
                new Product { ProductId = 6, Name = "Jean", SupplierId = 2 },
                new Product { ProductId = 7, Name = "Calculator", SupplierId = 1 },
                new Product { ProductId = 8, Name = "Box Set", SupplierId = 7 },
                new Product { ProductId = 9, Name = "Mickey Doll", SupplierId = 5 },
                new Product { ProductId = 10, Name = "Boohoo Shirt", SupplierId = 6 },
            };
    }

    public static List<Supplier> Suppliers()
    {
        return new List<Supplier>
            {
                new Supplier
                {
                    SupplierId = 1,
                    Name = "Amazon.co.uk",
                    LeadTime = 1
                },
                new Supplier
                {
                    SupplierId = 2,
                    Name = "John Lewis",
                    LeadTime = 2
                },
                new Supplier
                {
                    SupplierId = 3,
                    Name = "ASOS",
                    LeadTime = 1
                },
                new Supplier
                {
                    SupplierId = 4,
                    Name = "Debenhams",
                    LeadTime = 3
                },
                new Supplier
                {
                    SupplierId = 5,
                    Name = "Disney",
                    LeadTime = 6
                }
                ,
                new Supplier
                {
                    SupplierId = 6,
                    Name = "Boohoo",
                    LeadTime = 13
                }
            };
    }
}
