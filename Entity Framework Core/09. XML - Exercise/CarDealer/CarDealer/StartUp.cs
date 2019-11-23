namespace CarDealer
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Dtos.Import;
    using Models;
    using Data;
    using Dtos.Export;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new CarDealerContext())
            {
                Mapper.Initialize(x => x.AddProfile<CarDealerProfile>());

                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                #region Files

                var suppliersFile = File.ReadAllText(@"../../../Datasets/suppliers.xml");
                var carsFile = File.ReadAllText(@"../../../Datasets/cars.xml");
                var customersFile = File.ReadAllText(@"../../../Datasets/customers.xml");
                var partsFile = File.ReadAllText(@"../../../Datasets/parts.xml");
                var salesFile = File.ReadAllText(@"../../../Datasets/sales.xml");

                #endregion

                Console.WriteLine(GetCarsWithDistance(context));
            }
        }

        //Problem 9 - Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportSupplierDto[]),
                new XmlRootAttribute("Suppliers"));

            var suppliersDto = (ImportSupplierDto[])serializer.Deserialize(new StringReader(inputXml));
            var suppliers = Mapper.Map<IEnumerable<ImportSupplierDto>, IEnumerable<Supplier>>(suppliersDto);

            context.Suppliers.AddRange(suppliers);
            var countOfAddedEntities = context.SaveChanges();

            return $"Successfully imported {countOfAddedEntities}";
        }

        //Problem 10 - Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportPartDto[]),
                new XmlRootAttribute("Parts"));

            var partsDto = (ImportPartDto[])serializer.Deserialize(new StringReader(inputXml));
            var parts = Mapper.Map<IEnumerable<ImportPartDto>, IEnumerable<Part>>(partsDto);

            var suppliersId = context.Suppliers.Select(s => s.Id).ToList();
            var validParts = parts.Where(p => suppliersId.Contains(p.SupplierId));

            context.Parts.AddRange(validParts);
            var countOfAddedEntities = context.SaveChanges();

            return $"Successfully imported {countOfAddedEntities}";
        }

        //Problem 11 - Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCarDto[]),
                new XmlRootAttribute("Cars"));

            var carDtos = (ImportCarDto[])serializer.Deserialize(new StringReader(inputXml));
            var validCars = new List<Car>();

            foreach (var carDto in carDtos)
            {
                var car = Mapper.Map<Car>(carDto);

                foreach (var part in carDto.Parts)
                {
                    var partCarExists = car
                                            .PartCars
                                            .FirstOrDefault(p => p.PartId == part.PartId) != null;

                    if (!partCarExists && context.Parts.Any(p => p.Id == part.PartId))
                    {
                        var partCar = new PartCar
                        {
                            CarId = car.Id,
                            PartId = part.PartId
                        };

                        car.PartCars.Add(partCar);
                    }
                }

                validCars.Add(car);
            }

            context.Cars.AddRange(validCars);
            var countOfAddedEntities = context.SaveChanges();

            return $"Successfully imported {context.Cars.Count()}";
        }

        //Problem 12 - Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCustomerDto[]),
                new XmlRootAttribute("Customers"));

            var customerDtos = (ImportCustomerDto[])serializer.Deserialize(new StringReader(inputXml));
            var customers = Mapper.Map<IEnumerable<ImportCustomerDto>, IEnumerable<Customer>>(customerDtos);

            context.Customers.AddRange(customers);
            var countOfAddedEntities = context.SaveChanges();

            return $"Successfully imported {countOfAddedEntities}";
        }

        //Problem 13 - Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportSaleDto[]), new XmlRootAttribute("Sales"));
            var salesDto = (ImportSaleDto[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var sales = Mapper.Map<IEnumerable<ImportSaleDto>, IEnumerable<Sale>>(salesDto);

            var carIds = context.Cars.Select(c => c.Id).ToArray();
            var validSales = sales.Where(s => carIds.Contains(s.CarId)).ToArray();

            context.Sales.AddRange(validSales);
            var countOfAddedEntities = context.SaveChanges();

            return $"Successfully imported {countOfAddedEntities}";
        }

        //Problem 14 - Export Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ProjectTo<ExportCarWithDistanceDto>()
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCarWithDistanceDto[]),
                new XmlRootAttribute("cars"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 15 - Export Cars From Make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "BMW")
                .ProjectTo<ExportCarsFromBmwDto>()
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCarsFromBmwDto[]),
                new XmlRootAttribute("cars"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 16 - Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new ExportLocalSuppliersDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportLocalSuppliersDto[]),
                new XmlRootAttribute("suppliers"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), suppliers, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 17 - Export Cars With Their List Of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c =>
                    new ExportCarsWithPartsDto
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance,
                        Parts = c.PartCars.Select(pc =>
                                new PartDto
                                {
                                    Name = pc.Part.Name,
                                    Price = pc.Part.Price
                                })
                            .OrderByDescending(x => x.Price)
                            .ToArray()
                    })
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCarsWithPartsDto[]),
                new XmlRootAttribute("cars"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 18 - Export Total Sales By Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new ExportSalesByCustomerDto
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportSalesByCustomerDto[]),
                new XmlRootAttribute("customers"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), customers, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 19 - Export Sales With Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new ExportSalesWithDiscountDto
                {
                    Car = new CarDto
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    CustomerName = s.Customer.Name,
                    Discount = s.Discount,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = s.Car.PartCars.Sum(pc => pc.Part.Price) - s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100
                })
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportSalesWithDiscountDto[]),
                new XmlRootAttribute("sales"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), sales, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}