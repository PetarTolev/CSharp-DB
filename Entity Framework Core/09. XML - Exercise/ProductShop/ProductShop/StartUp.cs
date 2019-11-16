namespace ProductShop
{
    using AutoMapper;
    using Data;
    using Dtos.Export;
    using Dtos.Import;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new ProductShopContext())
            {
                Mapper.Initialize(x => { x.AddProfile<ProductShopProfile>(); });

                #region files

                var usersFile = File.ReadAllText(@"..\..\..\Datasets\users.xml");
                var productsFile = File.ReadAllText(@"..\..\..\Datasets\products.xml");
                var categoriesFile = File.ReadAllText(@"..\..\..\Datasets\categories.xml");
                var categoriesProductsFile = File.ReadAllText(@"..\..\..\Datasets\categories-products.xml");

                #endregion

                Console.WriteLine(GetUsersWithProducts(context));
            }
        }

        //Problem 1 - Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportUserDto[]),
                new XmlRootAttribute("Users"));

            var usersDto = (ImportUserDto[]) serializer.Deserialize(new StringReader(inputXml));
            var users = Mapper.Map<IEnumerable<ImportUserDto>, IEnumerable<User>>(usersDto);

            context.Users.AddRange(users);
            var countOfInsertedEntities = context.SaveChanges();

            return $"Successfully imported {countOfInsertedEntities}";
        }

        //Problem 2 - Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportProductDto[]),
                new XmlRootAttribute("Products"));

            var productsDto = (ImportProductDto[]) serializer.Deserialize(new StringReader(inputXml));
            var products = Mapper.Map<IEnumerable<ImportProductDto>, IEnumerable<Product>>(productsDto);

            context.Products.AddRange(products);
            var countOfInsertedEntities = context.SaveChanges();

            return $"Successfully imported {countOfInsertedEntities}";
        }

        //Problem 3 - Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCategoryDto[]),
                new XmlRootAttribute("Categories"));

            var categoriesDto = (ImportCategoryDto[]) serializer.Deserialize(new StringReader(inputXml));
            var categories = Mapper.Map<IEnumerable<ImportCategoryDto>, IEnumerable<Category>>(categoriesDto);

            context.Categories.AddRange(categories);
            var countOfInsertedEntities = context.SaveChanges();

            return $"Successfully imported {countOfInsertedEntities}";
        }

        //Problem 4 -  Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCategoryProductsDto[]),
                new XmlRootAttribute("CategoryProducts"));

            var categoryProductsDto = (ImportCategoryProductsDto[])(serializer
                .Deserialize(new StringReader(inputXml)));

            var categoryProducts = Mapper.Map<IEnumerable<ImportCategoryProductsDto>,
                IEnumerable<CategoryProduct>>(categoryProductsDto);

            var categories = context.Categories.Select(c => c.Id);
            var products = context.Products.Select(p => p.Id);

            var validCategoryProducts = categoryProducts
                .Where(cp => categories.Contains(cp.CategoryId)
                             && products.Contains(cp.ProductId));

            context.CategoryProducts.AddRange(validCategoryProducts);
            var countOfInsertedEntities = context.SaveChanges();

            return $"Successfully imported {countOfInsertedEntities}";
        }

        //Problem 5 - Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 &&
                            p.Price <= 1000)
                .Select(p => new ExportProductsInRangeDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                })
                .OrderBy(p => p.Price)
                .Take(10)
                .ToArray();
            
            XmlSerializer serializer = new XmlSerializer(typeof(ExportProductsInRangeDto[]), 
                new XmlRootAttribute("Products"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new []
            {
                new XmlQualifiedName("", ""), 
            });

            serializer.Serialize(new StringWriter(sb), products, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 6 - Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var products = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .Select(u =>
                    new ExportUsersWithSoldProductsDto
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        SoldProducts = u.ProductsSold
                            .Select(ps =>
                                new SoldProductsDto
                                {
                                    Name = ps.Name,
                                    Price = ps.Price
                                })
                            .ToArray()
                    })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportUsersWithSoldProductsDto[]),
                new XmlRootAttribute("Users"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName("", ""),
            });

            serializer.Serialize(new StringWriter(sb), products, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 7 - Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c =>
                    new ExportCategoriesByProductsCountDto
                    {
                        Name = c.Name,
                        Count = c.CategoryProducts.Count,
                        AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                        TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                    })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCategoriesByProductsCountDto[]),
                new XmlRootAttribute("Categories"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName("", ""),
            });

            serializer.Serialize(new StringWriter(sb), categories, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 8 - Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderByDescending(u => u.ProductsSold.Count)
                .Select(u => new ExportUserDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new ExportSoldProductsDto
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold
                            .OrderByDescending(ps => ps.Price)
                            .Select(p =>
                                new ExportProductDto
                                {
                                    Name = p.Name,
                                    Price = p.Price
                                })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                    }
                })
                .Take(10)
                .ToArray();

            var result = new ExportUsersWithProductsDto
            {
                Count = context.Users.Count(u => u.ProductsSold.Any()),
                Users = users
            };

            XmlSerializer serializer = new XmlSerializer(typeof(ExportUsersWithProductsDto),
                new XmlRootAttribute("Users"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName("", ""),
            });

            serializer.Serialize(new StringWriter(sb), result, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}