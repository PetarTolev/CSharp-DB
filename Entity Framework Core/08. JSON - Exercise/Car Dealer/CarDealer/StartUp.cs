namespace CarDealer
{
    using AutoMapper;
    using Data;
    using DTO.Export;
    using DTO.Import;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            using (var context = new CarDealerContext())
            {
                Console.WriteLine(GetSalesWithAppliedDiscount(context));
            }
        }

        //Problem 9 - Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert
                .DeserializeObject<Supplier[]>(inputJson);
            
            context.Suppliers
                .AddRange(suppliers);
            var countOfInsertedEntities = context.SaveChanges();

            return $"Successfully imported {countOfInsertedEntities}.";
        }

        //Problem 10 - Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var supplierIds = context.Suppliers.Select(s => s.Id);

            var parts = JsonConvert
                .DeserializeObject<Part[]>(inputJson)
                .Where(p => supplierIds.Contains(p.SupplierId));
            
            context.Parts
                .AddRange(parts);
            var countOfInsertedEntities = context.SaveChanges();

            return $"Successfully imported {countOfInsertedEntities}.";
        }

        //Problem 11 - Import Cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsImport = JsonConvert.DeserializeObject<CarImportDto[]>(inputJson);
            var carsToAdd = Mapper.Map<CarImportDto[], Car[]>(carsImport);

            context.AddRange(carsToAdd);
            context.SaveChanges();

            HashSet<int> partIds = context.Parts.Select(x => x.Id).ToHashSet();

            HashSet<PartCar> carPartsToAdd = new HashSet<PartCar>();

            foreach (var car in carsImport)
            {
                car.PartsId = car
                    .PartsId
                    .Distinct()
                    .ToList();

                Car currentCar = context.
                    Cars
                    .FirstOrDefault(x => x.Make == car.Make
                                         && x.Model == car.Model
                                         && x.TravelledDistance == car.TravelledDistance);

                if (currentCar == null)
                {
                    continue;
                }

                foreach (var id in car.PartsId)
                {
                    if (!partIds.Contains(id))
                    {
                        continue;
                    }

                    PartCar partCar = new PartCar
                    {
                        CarId = currentCar.Id,
                        PartId = id
                    };

                    if (!carPartsToAdd.Contains(partCar))
                    {
                        carPartsToAdd.Add(partCar);
                    }
                }

                if (carPartsToAdd.Count > 0)
                {
                    currentCar.PartCars = carPartsToAdd;
                    context.PartCars.AddRange(carPartsToAdd);
                    carPartsToAdd.Clear();
                }
            }

            context.SaveChanges();

            return $"Successfully imported {context.Cars.ToList().Count}.";
        }

        //Problem 12 - Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert
                .DeserializeObject<Customer[]>(inputJson);

            context.Customers.AddRange(customers);
            var countOfInsertedEntities = context.SaveChanges();

            return $"Successfully imported {countOfInsertedEntities}.";
        }

        //Problem 13 - Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)

        {
            var sales = JsonConvert
                .DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            var countOfInsertedEntities = context.SaveChanges();

            return $"Successfully imported {countOfInsertedEntities}.";
        }

        //Problem 14 - Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToList();

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return json;
        }

        //Problem 15 - Export Cars From Make Toyota
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToList();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return json;
        }

        //Problem 16 - Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter != true)
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToList();

            var json = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return json;
        }

        //Problem 17 - Export Cars With Their List Of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new CarPartsExportDto()
                {
                    Car = new CarExportDto()
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance
                    },
                    Parts = c.PartCars
                        .Select(pc => new PartExportDto()
                        {
                            Name = pc.Part.Name,
                            Price = $"{pc.Part.Price:F2}"
                        })
                        .ToArray()
                })
                .ToArray();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return json;
        }

        //Problem 18 - Export Total Sales By Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.SpentMoney)
                .ThenByDescending(c => c.BoughtCars)
                .ToList();

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };

            var json = JsonConvert.SerializeObject(customers, serializerSettings);

            return json;
        }

        //Problem 19 - Export Sales With Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new SalesExportDto
                {
                    Car = new CarExportDto
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    CustomerName = s.Customer.Name,
                    Discount = $"{s.Discount:F2}",
                    Price = $"{s.Car.PartCars.Sum(pc => pc.Part.Price):F2}",
                    PriceWithDiscount =
                        $"{s.Car.PartCars.Sum(pc => pc.Part.Price) - s.Car.PartCars.Sum(pc => pc.Part.Price) * (s.Discount / 100m):f2}"
                })
                .Take(10)
                .ToList();

            var json = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return json;
        }
    }
}