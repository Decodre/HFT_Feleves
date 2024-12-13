using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XH0PTT_HSZF_2024251.Application;
using XH0PTT_HSZF_2024251.Model;
using XH0PTT_HSZF_2024251.Model.Dtos;
using XH0PTT_HSZF_2024251.Persistance.MsSql;

namespace XH0PTT_HSZF_2024251.Test
{
    [TestFixture]
    public class GotTests
    {
        private GotDbContext ctx;
        private IHouseDataProvider hDataProvider;
        private IHouseService hService;
        private ICharacterDataProvider cDataProvider;
        private ICharacterService cService;


        [SetUp]
        public void SetUp()
        {
            var host = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) => {
                services.AddScoped<GotDbContext>();

                services.AddSingleton<ICharacterDataProvider, CharacterDataProvider>();
                services.AddSingleton<IHouseDataProvider, HouseDataProvider>();

                services.AddSingleton<ICharacterService, CharacterService>();
                services.AddSingleton<IHouseService, HouseService>();
            }).Build();

            host.Start();

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            ctx = serviceProvider.GetRequiredService<GotDbContext>();

            cDataProvider = serviceProvider.GetRequiredService<ICharacterDataProvider>();
            hDataProvider = serviceProvider.GetRequiredService<IHouseDataProvider>();
            cService = serviceProvider.GetRequiredService<ICharacterService>();
            hService = serviceProvider.GetRequiredService<IHouseService>();


            SeedTestData();
        }

        //[TearDown]
        //public void TearDown()
        //{
        //    ctx.Database.EnsureDeleted();
        //    ctx.Dispose();
        //}

        private void SeedTestData()
        {
            var starkHouse = new House("Stark", "Eddard Stark", 5000, 800, "Winterfell")
            {
                Characters = new List<Character>
            {
                new Character("Arya Stark", 15),
                new Character("Jon Snow", 21)
            }
            };
            starkHouse.Allies = new List<string> { "Lannister" };

            var lannisterHouse = new House("Lannister", "Tywin Lannister", 10000, 900, "Casterly Rock")
            {
                Characters = new List<Character>
            {
                new Character("Tyrion Lannister", 10),
                new Character("Jaime Lannister", 25)
            }
            };
            lannisterHouse.Allies = new List<string>();

            ctx.Houses.Add(starkHouse);
            ctx.Houses.Add(lannisterHouse);
            ctx.SaveChanges();
        }

        [Test]
        public void AddHouse_ShouldAddHouseToDatabase()
        {
            var newHouse = new House("Targaryen", "Daenerys Targaryen", 15000, 300, "Dragonstone");
            hService.AddHouse(newHouse);

            var result = hDataProvider.GetHouses().FirstOrDefault(h => h.Name == "Targaryen");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Targaryen"));
        }

        [Test]
        public void GetHouses_ShouldReturnAllHouses()
        {
            var result = hDataProvider.GetHouses().ToList();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void CharactersByBattleCount_Test()
        {
            var result = cService.CharactersByBattleCount();

            Assert.That(result.First().Name, Is.EqualTo("Jaime Lannister"));
            Assert.That(result.Last().Name, Is.EqualTo("Tyrion Lannister"));
        }

        [Test]
        public void HouseWithMostAllies_Test()
        {
            HouseWithMostAlliesDto result = hService.HouseWithMostAllies();

            Assert.That(result.Name, Is.EqualTo("Stark"));
            Assert.That(result.AlliesCount, Is.EqualTo(1));

        }

        [Test]
        public void AverageBattleCount_Test()
        {
            double result = hService.AverageBattleCount("Stark");

            Assert.That(result, Is.EqualTo(18));
        }

        [Test]
        public void HousesByEstablishmentYear_Test()
        {
            List<HousesByEstablishmentYearDto> result = hService.HousesByEstablishmentYear();

            Assert.That(result[0].EstablishedYear, Is.EqualTo(800));
            Assert.That(result[0].Seat, Is.EqualTo("Winterfell"));
        }

        [Test]
        public void TotalAlliesBattleCount_Test()
        {
            List<TotalAlliesBattleCountDto> result = hService.TotalAlliesBattleCount("Stark");

            Assert.That(result[0].HouseName, Is.EqualTo("Lannister"));
            Assert.That(result[0].TotalBattleCount, Is.EqualTo(35));
        }

        [Test]
        public void CreateArmyCountReport_Test()
        {
            hService.CreateHouseFolders();
            hService.CreateArmyCountReport();

            string expectedFilePath = Path.Combine("Westeros", "Stark", "allies_armyCount_Stark.txt");

            Assert.That(File.Exists(expectedFilePath), Is.True);
        }

        [Test]
        public void CreateJson_Test()
        {
            hService.CreateHouseFolders();
            hService.CreateJson();

            string expectedFilePath = Path.Combine("Westeros", "Stark", "house_Stark.json");

            Assert.That(File.Exists(expectedFilePath), Is.True);
        }

        [Test]
        public void CreateHouseFolders_Test()
        {
            hService.CreateHouseFolders();

            string expectedFilePath = Path.Combine("Westeros", "Stark", "house_Stark.json");

            Assert.That(Directory.Exists("Westeros/Stark"), Is.True);
            Assert.That(Directory.Exists("Westeros/Lannister"), Is.True);
        }

    }
}
