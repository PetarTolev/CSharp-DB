namespace CarDealer
{
    using AutoMapper;
    using Data;
    using DTO.Import;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            using (var context = new CarDealerContext())
            {
                //context.Database.EnsureCreated();

                var cars = File.ReadAllText(@"..\..\..\Datasets\cars.json");

                Console.WriteLine(ImportCars(context, cars));
            }
        }

        //Problem 9
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert
                .DeserializeObject<Supplier[]>(inputJson);
            
            context.Suppliers
                .AddRange(suppliers);
            var countOfInsertedEntities = context.SaveChanges();

            return $"Successfully imported {countOfInsertedEntities}.";
        }

        //Problem 10
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

        //Problem 11
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

        //Problem 12
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert
                .DeserializeObject<Customer[]>(inputJson);

            context.Customers.AddRange(customers);
            var countOfInsertedEntities = context.SaveChanges();

            return $"Successfully imported {countOfInsertedEntities}.";
        }

        //Problem 13
        public static string ImportSales(CarDealerContext context, string inputJson)

        {
            var sales = JsonConvert
                .DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            var countOfInsertedEntities = context.SaveChanges();

            return $"Successfully imported {countOfInsertedEntities}.";
        }

        //Problem 14
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            return null;
        }

        //Problem 15
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            return null;
        }

        //Problem 16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            return null;
        }

        //Problem 17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            return null;
        }

        //Problem 18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            return null;
        }

        //Problem 19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            return null;
        }
    }
}