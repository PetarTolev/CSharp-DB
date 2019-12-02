using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using FastFood.App;
using FastFood.Data;
using FastFood.Models;

[TestFixture]
public class Export_000_001
{
	private IServiceProvider serviceProvider;

	[SetUp]
	public void Setup()
	{
		var services = new ServiceCollection();

		services
			.AddDbContext<FastFoodDbContext>(
				options => options.UseInMemoryDatabase("FastFood")
			);

		Mapper.Reset();
		Mapper.Initialize(cfg => cfg.AddProfile<FastFoodProfile>());

		this.serviceProvider = services.BuildServiceProvider();
	}

	[Test]
	public void ExportAllOrdersByEmployeeZeroTest1()
	{
		var context = serviceProvider.GetService<FastFoodDbContext>();

		SeedDatabase(context);

		var expectedOutput = JToken.Parse("{'Name':'Avery Rush','Orders':[{'Customer':'Stacey','Items':[{'Name':'Cheeseburger','Price':6.00,'Quantity':5},{'Name':'Double Cheeseburger','Price':6.50,'Quantity':3},{'Name':'Luigi','Price':2.10,'Quantity':5},{'Name':'Bacon Deluxe','Price':9.00,'Quantity':1}],'TotalPrice':69.00},{'Customer':'Pablo','Items':[{'Name':'Double Cheeseburger','Price':6.50,'Quantity':3},{'Name':'Bacon Deluxe','Price':9.00,'Quantity':5}],'TotalPrice':64.50},{'Customer':'Bobbie','Items':[{'Name':'Tuna Salad','Price':3.00,'Quantity':2},{'Name':'Crispy Fries','Price':2.00,'Quantity':5},{'Name':'Fries','Price':1.50,'Quantity':2}],'TotalPrice':19.00},{'Customer':'Joann','Items':[{'Name':'Minion','Price':2.20,'Quantity':2},{'Name':'Bacon Deluxe','Price':9.00,'Quantity':1}],'TotalPrice':13.40}],'TotalMade':165.90}"
		);

		var actualOutput =
			JToken.Parse(FastFood.DataProcessor.Serializer.ExportOrdersByEmployee(context, "Avery Rush", "ToGo"));

		var jsonsAreEqual = JToken.DeepEquals(expectedOutput, actualOutput);
		Assert.That(jsonsAreEqual, Is.True, "ExportOrdersByEmployee output is incorrect!");
	}

	private static void SeedDatabase(FastFoodDbContext context)
	{
		var itemsJson = "[{'Name':'Hamburger','Price':5.00},{'Name':'Mario','Price':2.20},{'Name':'Batman','Price':3.00},{'Name':'Minion','Price':2.20},{'Name':'Just Lettuce','Price':0.40},{'Name':'Tuna Salad','Price':3.00},{'Name':'Cesar Salad','Price':2.10},{'Name':'Cake','Price':2.50},{'Name':'Cookie','Price':0.60},{'Name':'Ice Cream','Price':1.10},{'Name':'Purple Drink','Price':1.30},{'Name':'Orange Drink','Price':1.20},{'Name':'Cola','Price':1.20},{'Name':'Curly Fries','Price':2.20},{'Name':'Crispy Fries','Price':2.00},{'Name':'Fries','Price':1.50},{'Name':'Chicken Tenders','Price':4.00},{'Name':'Tasty Basket','Price':7.00},{'Name':'Cheeseburger','Price':6.00},{'Name':'Quarter Pounder','Price':5.50},{'Name':'Double Cheeseburger','Price':6.50},{'Name':'Daily Double','Price':7.50},{'Name':'Ranger Burger','Price':5.50},{'Name':'BBQ Burger','Price':8.00},{'Name':'Luigi','Price':2.10},{'Name':'Bacon Deluxe','Price':9.00},{'Name':'Premium chicken sandwich','Price':7.00},{'Name':'Snack Wrap','Price':4.40},{'Name':'Premium Chicken Wrap','Price':5.40},{'Name':'Chicken Nuggets','Price':2.50},{'Name':'Crispy Chicken Deluxe','Price':3.50},{'Name':'Grilled Chicken Deluxe','Price':4.00},{'Name':'Triple Cheeseburger','Price':10.00},{'Name':'Bowser','Price':2.00}]";
		var employeesWithOrdersJson = "[{'Name':'Magda Bjork','Orders':[]},{'Name':'Iris Foushee','Orders':[{'Customer':'Guillermo','OrderItems':[{'ItemId':33,'Quantity':2}],'Type':'ForHere'}]},{'Name':'Eric Toole','Orders':[]},{'Name':'Justin Brazil','Orders':[]},{'Name':'Felisa Frier','Orders':[{'Customer':'Leona','OrderItems':[{'ItemId':34,'Quantity':4}],'Type':'ForHere'}]},{'Name':'Lakiesha Huffines','Orders':[]},{'Name':'Emory Lemos','Orders':[]},{'Name':'Lanita Palmore','Orders':[]},{'Name':'Janiece Owens','Orders':[]},{'Name':'Maryland Palm','Orders':[]},{'Name':'Sunday Eastep','Orders':[]},{'Name':'Lucius Rotz','Orders':[]},{'Name':'Stanton Dahl','Orders':[{'Customer':'Roberta','OrderItems':[{'ItemId':17,'Quantity':4},{'ItemId':20,'Quantity':1},{'ItemId':30,'Quantity':3}],'Type':'ForHere'}]},{'Name':'Carmon Sesco','Orders':[]},{'Name':'Willette Ugalde','Orders':[]},{'Name':'Rose Blizzard','Orders':[]},{'Name':'Caridad Cuenca','Orders':[]},{'Name':'Tran Bullion','Orders':[]},{'Name':'Fred Higby','Orders':[]},{'Name':'Elizabet Trentham','Orders':[{'Customer':'Enrique','OrderItems':[{'ItemId':4,'Quantity':5},{'ItemId':6,'Quantity':2},{'ItemId':18,'Quantity':4},{'ItemId':22,'Quantity':3}],'Type':'ForHere'}]},{'Name':'Shirleen Vonruden','Orders':[{'Customer':'Enrique','OrderItems':[{'ItemId':2,'Quantity':4},{'ItemId':10,'Quantity':4},{'ItemId':17,'Quantity':3},{'ItemId':24,'Quantity':1}],'Type':'ToGo'}]},{'Name':'Oscar Dolan','Orders':[]},{'Name':'Annett Lewallen','Orders':[{'Customer':'Pablo','OrderItems':[{'ItemId':5,'Quantity':4},{'ItemId':8,'Quantity':3},{'ItemId':31,'Quantity':5}],'Type':'ForHere'}]},{'Name':'Camille Peller','Orders':[]},{'Name':'Katie Nilsen','Orders':[]},{'Name':'Connie Barbosa','Orders':[]},{'Name':'Erich Hennigan','Orders':[]},{'Name':'Shin Vallejos','Orders':[{'Customer':'Ray','OrderItems':[{'ItemId':31,'Quantity':2}],'Type':'ForHere'}]},{'Name':'Avery Rush','Orders':[{'Customer':'Stacey','OrderItems':[{'ItemId':19,'Quantity':5},{'ItemId':21,'Quantity':3},{'ItemId':25,'Quantity':5},{'ItemId':26,'Quantity':1}],'Type':'ToGo'},{'Customer':'Joann','OrderItems':[{'ItemId':4,'Quantity':2},{'ItemId':26,'Quantity':1}],'Type':'ToGo'},{'Customer':'Pablo','OrderItems':[{'ItemId':21,'Quantity':3},{'ItemId':26,'Quantity':5}],'Type':'ToGo'},{'Customer':'Bobbie','OrderItems':[{'ItemId':6,'Quantity':2},{'ItemId':15,'Quantity':5},{'ItemId':16,'Quantity':2}],'Type':'ToGo'}]},{'Name':'Coral Points','Orders':[]},{'Name':'Xiao Raley','Orders':[]},{'Name':'Kym Douse','Orders':[{'Customer':'Yolanda','OrderItems':[{'ItemId':16,'Quantity':1},{'ItemId':20,'Quantity':5},{'ItemId':24,'Quantity':4}],'Type':'ForHere'}]},{'Name':'Kendra Stangl','Orders':[]},{'Name':'Lura Yeldell','Orders':[]},{'Name':'Mohammad Norton','Orders':[]},{'Name':'Tamika Thornsberry','Orders':[]},{'Name':'Prince Betton','Orders':[]},{'Name':'Nancie Mcquarrie','Orders':[]},{'Name':'Classie Mettler','Orders':[{'Customer':'Darryl','OrderItems':[{'ItemId':6,'Quantity':3},{'ItemId':11,'Quantity':1},{'ItemId':16,'Quantity':5},{'ItemId':32,'Quantity':4},{'ItemId':33,'Quantity':4}],'Type':'ToGo'}]},{'Name':'Denita Providence','Orders':[{'Customer':'Bobbie','OrderItems':[{'ItemId':29,'Quantity':2}],'Type':'ToGo'}]},{'Name':'Jerica Rupe','Orders':[]},{'Name':'Nolan Jablonski','Orders':[]},{'Name':'Mikki Vasques','Orders':[]},{'Name':'Ariane Sloan','Orders':[{'Customer':'Daniel','OrderItems':[{'ItemId':4,'Quantity':4},{'ItemId':11,'Quantity':1},{'ItemId':19,'Quantity':3},{'ItemId':28,'Quantity':2}],'Type':'ForHere'}]},{'Name':'Herta Balser','Orders':[{'Customer':'Miguel','OrderItems':[{'ItemId':15,'Quantity':5},{'ItemId':26,'Quantity':1},{'ItemId':29,'Quantity':3}],'Type':'ToGo'}]},{'Name':'Jacqualine Clune','Orders':[]},{'Name':'Mervin Langone','Orders':[]},{'Name':'Dorethea Mumford','Orders':[{'Customer':'Daniel','OrderItems':[{'ItemId':23,'Quantity':4},{'ItemId':24,'Quantity':5}],'Type':'ForHere'}]},{'Name':'Maxwell Shanahan','Orders':[{'Customer':'Garry','OrderItems':[{'ItemId':5,'Quantity':4},{'ItemId':17,'Quantity':4},{'ItemId':20,'Quantity':2},{'ItemId':27,'Quantity':2}],'Type':'ForHere'},{'Customer':'Roberta','OrderItems':[{'ItemId':3,'Quantity':2},{'ItemId':6,'Quantity':1},{'ItemId':7,'Quantity':2},{'ItemId':11,'Quantity':5},{'ItemId':27,'Quantity':2}],'Type':'ForHere'}]},{'Name':'Jolanda Discher','Orders':[{'Customer':'Ray','OrderItems':[{'ItemId':18,'Quantity':2}],'Type':'ForHere'}]}]";

		var items = JsonConvert.DeserializeObject<Item[]>(itemsJson);
		var employees = JsonConvert.DeserializeObject<Employee[]>(employeesWithOrdersJson);

		context.Items.AddRange(items);
		context.Employees.AddRange(employees);

		context.SaveChanges();
	}
}